using System.Collections.Generic;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }

    public event EventHandler OnSelectedActionChanged;
    public event EventHandler<Unit> OnSelectedUnitChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private LayerMask unitsLayerMask;

    internal BaseAction savedAction;
    private BaseAction selectedAction;
    private BaseAbility selectedBaseAbility;
    private MoveAction selectedMoveAction;
    private Unit selectedUnit;
    private bool isBusy;
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

    private void OnDisable()
    {
        ManosInputController.Instance.SelectActionWithNumbers.performed -= ManosInputController_SetSelectedAction;
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
    public void InvokeAbilityFinished()
    {
        CheckActionUse();
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
        savedAction = null;
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

        if (baseAction != null && baseAction.GetAbilityPropertie().Contains(AbilityProperties.AreaOfEffect))
        {
            AOEManager.Instance.SetIsAOEActive(true, baseAction.GetIsFollowingMouse(),
            selectedUnit.transform.position, baseAction.GetActionMeshShape(), baseAction.GetMeshScaleMultiplicator(), baseAction.GetRange());
        }

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
            if (selectedUnit.UsedAllPoints())
            {
                print("UNIT USED ALL ABILITES");

                for (int i = 0; i < 3; i++)
                {
                    if (!UnitManager.Instance.GetFriendlyUnitList()[i].UsedAllPoints())
                    {
                        SetSelectedUnit(UnitManager.Instance.GetFriendlyUnitList()[i]);
                        break;
                    }
                }
                if (UnitManager.Instance.GetFriendlyUnitList()[0].UsedAllPoints()
                    && UnitManager.Instance.GetFriendlyUnitList()[1].UsedAllPoints()
                    && UnitManager.Instance.GetFriendlyUnitList()[2].UsedAllPoints()
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

                    if (_unit.IsEnemy())
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
            if (selectedAction == null) { print("Selected Action is Null Returning"); return; }
            if (selectedAction.GetIfUsedAction()) { print("Selected action been used Returning"); return; }
            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) { print("Grid Is Not Valid"); return; }
            if (LevelGrid.Instance.GetUnitAtGridPosition(mouseGridPosition) != null && LevelGrid.Instance.GetUnitAtGridPosition(mouseGridPosition).GetGridEffectiveness() == Effectiveness.Miss) { print("Unit in grid pos and effectivness is 0"); return; }
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) { print("Action Points for current ability is insufficent Returning"); return; }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
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

        ManosInputController.Instance.SelectActionWithNumbers.performed += ManosInputController_SetSelectedAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    private void ManosInputController_SetSelectedAction(InputAction.CallbackContext inputValue)
    {
        if (isBusy) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        BaseAction[] availableUnitActions = selectedUnit.GetBaseActionArray();
        int passedInput = (int)inputValue.ReadValue<float>();

        if (passedInput >= availableUnitActions.Length) { return; }

        if (!availableUnitActions[passedInput].GetIsBonusAction() && selectedUnit.GetUsedActionPoints()) { return; }
        else if (availableUnitActions[passedInput].GetIsBonusAction() && selectedUnit.GetUsedBonusActionPoints()) { return; }

        SetSelectedAction(availableUnitActions[passedInput]);
    }

}