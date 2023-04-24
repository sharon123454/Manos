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
    public event EventHandler OnSelectedUnitChanged;
    public event EventHandler<bool> OnBusyChanged;
    public event EventHandler OnActionStarted;

    [SerializeField] private LayerMask unitsLayerMask;

    internal BaseAction selectedAction;
    internal BaseAction savedAction;
    private Unit selectedUnit;
    private bool isBusy;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void SignToNumerics()
    {
        ManosInputController.Instance.SelectActionWithNumbers.performed += ManosInputController_SetSelectedAction;
    }//invoked on enable as script loads before ManosInputController

    private void OnEnable()
    {
        Invoke("SignToNumerics", 1);
    }

    private void Update()
    {
        if (isBusy) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        //canceles current action
        if (ManosInputController.Instance.Space.IsPressed())
        {

            if (selectedAction is MoveAction) { }
            else
                SetSelectedAction(selectedUnit.GetAction<MoveAction>());
        }

        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (TryHandleUnitSelection()) { return; }

        HandleSelectedAction();
    }

    private void OnDisable()
    {
        ManosInputController.Instance.SelectActionWithNumbers.performed -= ManosInputController_SetSelectedAction;
    }

    public Unit GetSelectedUnit() { return selectedUnit; }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction() { return selectedAction; }
    public BaseAction GetSavedAction() { return savedAction; }

    public void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>()); //default unit action
        if (savedAction == null)
        {
            savedAction = selectedUnit.GetAction<MoveAction>();
        }
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool TryHandleUnitSelection()
    {
        if (ManosInputController.Instance.Click.IsPressed())
        {
            if (GetSelectedAction() is BaseHeal && GetSelectedAction().GetCooldown() == 0)
            {
                return false;
            }
            Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

            if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, unitsLayerMask))
                if (_rayCastHit.transform.TryGetComponent<Unit>(out Unit _unit))
                {
                    if (_unit == selectedUnit)
                    {
                        //Unit is already selected
                        return false;
                    }

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
        if (ManosInputController.Instance.RightClick.IsPressed())
        {
            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (!selectedAction.IsValidActionGridPosition(mouseGridPosition)) { return; }
            if (!selectedUnit.TrySpendActionPointsToTakeAction(selectedAction)) { return; }

            SetBusy();
            selectedAction.TakeAction(mouseGridPosition, ClearBusy);

            OnActionStarted?.Invoke(this, EventArgs.Empty);
        }
    }

    private void ClearBusy()
    {
        isBusy = false;
        OnBusyChanged?.Invoke(this, isBusy);
    }

    private void SetBusy()
    {
        isBusy = true;
        OnBusyChanged?.Invoke(this, isBusy);
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