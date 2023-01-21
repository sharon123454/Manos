using System.Collections.Generic;
using UnityEngine.EventSystems;
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

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitsLayerMask;

    internal BaseAction selectedAction;
    private bool isBusy;
    public BaseAction savedAction;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        SetSelectedUnit(selectedUnit);
    }

    private void Update()
    {
        if (isBusy) { return; }
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        //canceles current action
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (selectedAction is MoveAction)
            {
            }
            else
                SetSelectedAction(selectedUnit.GetAction<MoveAction>());
        }

        if (EventSystem.current.IsPointerOverGameObject()) { return; }
        if (TryHandleUnitSelection()) { return; }

        HandleSelectedAction();
    }

    public Unit GetSelectedUnit() { return selectedUnit; }

    public void SetSelectedAction(BaseAction baseAction)
    {
        selectedAction = baseAction;
        OnSelectedActionChanged?.Invoke(this, EventArgs.Empty);
    }

    public BaseAction GetSelectedAction() { return selectedAction; }

    private void SetSelectedUnit(Unit unit)
    {
        selectedUnit = unit;
        SetSelectedAction(unit.GetAction<MoveAction>()); //default unit action
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool TryHandleUnitSelection()
    {
        if (Input.GetMouseButtonDown(0))
        {
            Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

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
        if (Input.GetMouseButtonDown(0))
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

}