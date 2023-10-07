using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<Unit> OnSelectedUnitChanged;
    public event EventHandler<bool> OnBusyChanged;

    [SerializeField] private LayerMask unitsLayerMask;

    internal BaseAction savedAction;
    internal bool isBusy;

    private BaseAction selectedAction;
    private BaseAbility selectedBaseAbility;
    private MoveAction selectedMoveAction;
    private Unit selectedUnit;
    private bool hoveringUI = false;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }
    private void Start() { StartCoroutine(DelayOnStart()); }
    private void Update()
    {
        if (isBusy) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        //canceles current action
        if (ManosInputController.Instance.Space.IsPressed())// Move to buttonUI
        {
            if (selectedAction is MoveAction)
            { }
            //else if (selectedAction is not MoveAction && selectedAction.GetIfUsedAction())
            //    SetSelectedAction(selectedUnit.GetBaseActionArray()[1]);
            //    else if (selectedAction is not MoveAction && selectedAction.GetIfUsedAction() /*&& used bonus action*/)
            //CheckActionUse();
            else
                SetSelectedAction(selectedUnit.GetAction<MoveAction>());//SetSelectedAction(selectedUnit.GetBaseActionArray()[0]);
        }
        if (EventSystem.current.IsPointerOverGameObject() && hoveringUI) { return; }
        if (TryHandleUnitSelection()) { return; }

        HandleSelectedAction();
    }
    private void OnDestroy()
    {
        BaseAction.OnAnyActionCompleted -= BaseAction_OnAnyActionCompleted;
    }

    public void SetSelectedUnit(Unit unit)
    {
        if (unit)
        {
            selectedUnit = unit;
            savedAction = null;
            selectedAction = null;
            OnSelectedUnitChanged?.Invoke(this, selectedUnit);
            //SetSelectedAction(selectedUnit.GetAction<MoveAction>()); //default unit action

            //if (!savedAction)
            //    savedAction = selectedUnit.GetAction<MoveAction>();
        }
    }

    public void SetHoveringOnUI(bool ui) { hoveringUI = ui; }
    public Unit GetSelectedUnit() { return selectedUnit; }
    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        if (baseAction is BaseAbility)
        {
            selectedMoveAction = null;
            selectedBaseAbility = (BaseAbility)baseAction;
        }
        else if (baseAction is MoveAction)
        {
            selectedBaseAbility = null;
            selectedMoveAction = (MoveAction)baseAction;
        }
        else
        {
            selectedBaseAbility = null;
            selectedMoveAction = null;
        }

        if (baseAction && baseAction.IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            AOEManager.Instance.SetIsAOEActive(baseAction.GetIsFollowingMouse(), selectedUnit.transform.position,
            baseAction.GetActionMeshShape(), baseAction.GetMeshScaleMultiplicator(), baseAction.GetRange());
        }
        else
            AOEManager.Instance.DisableAOE();

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }
    public BaseAction GetSavedAction() { return savedAction; }
    public BaseAction GetSelectedAction() { return selectedAction; }
    public MoveAction GetSelectedMoveAction() { return selectedMoveAction; }
    public BaseAbility GetSelectedBaseAbility() { return selectedBaseAbility; }

    private void CheckActionUse()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            if (selectedUnit.GetUsedBothActions())
            {
                print("UNIT USED ALL ABILITES");

                for (int i = 0; i < 3; i++)
                {
                    if (!UnitManager.Instance.GetFriendlyUnitList()[i].GetUsedBothActions())
                    {
                        SetSelectedUnit(UnitManager.Instance.GetFriendlyUnitList()[i]);
                        break;
                    }
                }
                if (UnitManager.Instance.GetFriendlyUnitList()[0].GetUsedBothActions()
                    && UnitManager.Instance.GetFriendlyUnitList()[1].GetUsedBothActions()
                    && UnitManager.Instance.GetFriendlyUnitList()[2].GetUsedBothActions()
                    )
                {
                    TurnSystem.Instance.NextTurn();
                }
            }
        }
    }

    private bool TryHandleUnitSelection()
    {
        if (ManosInputController.Instance.Click.IsPressed())
        {
            Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

            if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, unitsLayerMask))
                if (_rayCastHit.transform.TryGetComponent<Unit>(out Unit _unit))
                {
                    if (_unit == selectedUnit)//Unit is already selected
                        return false;

                    if (_unit.IsEnemy())//Unit is Enemy
                        return false;

                    BaseAction selectedAction = GetSelectedAction();
                    if (selectedAction && (selectedAction.IsXPropertyInAction(AbilityProperties.CDR)//Action Properties that applies on Allies
                        || selectedAction.IsXPropertyInAction(AbilityProperties.AreaOfEffect)
                        || selectedAction.IsXPropertyInAction(AbilityProperties.Teleport)
                        || selectedAction.IsXPropertyInAction(AbilityProperties.Heal)))
                        return false;

                    SetSelectedUnit(_unit);
                    return true;
                }
        }

        return false;
    }
    private void HandleSelectedAction()
    {
        if (ManosInputController.Instance.Click.WasReleasedThisFrame())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());
            Unit unitAtGridPos = LevelGrid.Instance.GetUnitAtGridPosition(mouseGridPosition);

            if (mouseGridPosition == null) { print("Grid Position is Null"); return; }
            if (selectedAction == null) { print("Selected Action is Null Returning"); return; }
            if (selectedAction.GetIfUsedAction()) { print("Selected action been used Returning"); return; }
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) { print("Grid Is Not Valid"); return; }
            //if (mouseGridPosition.GetEffectiveRange() == Effectiveness.Miss) { print("Grid Effectivness is 0"); return; }
            if (unitAtGridPos && unitAtGridPos.GetGridEffectiveness() == Effectiveness.Miss) { print("Unit in grid pos and effectivness is 0"); return; }
            if (!unitAtGridPos && !selectedAction.IsXPropertyInAction(AbilityProperties.AreaOfEffect) && selectedAction.GetRange() != ActionRange.Move) { print("Unit in grid pos is NULL and action selected is NOT AoE and NOT Move"); return; }
            #region AoE grid effectiveness check
            Vector3 mousePos = MouseWorld.GetPosition();
            if (selectedAction.IsXPropertyInAction(AbilityProperties.AreaOfEffect) && Physics.Raycast(mousePos + Vector3.up * 3, Vector3.down * 3, out RaycastHit _rayCastHit, float.MaxValue, LayerMask.GetMask("Grid")))
            {
                //Debug.DrawRay(mousePos + Vector3.up * 3, Vector3.down * 3, Color.green, 10);
                if (_rayCastHit.transform.TryGetComponent<GridVisual>(out GridVisual _gridVisual))
                {
                    if (!_gridVisual.IsVisualActive())
                    {
                        print("Grid Visual AoE is disabled"); return;
                    }
                }
            }
            #endregion
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) { print("Action Points for current ability is insufficent Returning"); return; }//MUST BE LAST (will consume resources)

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);
        }
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
    }
    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private IEnumerator DelayOnStart()
    {
        yield return null;
        yield return null;
        List<Unit> friendlyUnits = UnitManager.Instance.GetFriendlyUnitList();
        if (friendlyUnits.Count > 0)
        {
            SetSelectedUnit(friendlyUnits[0]);
            Debug.Log($"found unit {friendlyUnits[0]} on start");
        }
        else
            Debug.Log($"{name}: unit not found");

        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void BaseAction_OnAnyActionCompleted(object sender, BaseAction actionStarted)
    {
        CheckActionUse();
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        savedAction = null;
    }

}