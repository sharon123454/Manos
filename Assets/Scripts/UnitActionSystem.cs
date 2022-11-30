using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitActionSystem : MonoBehaviour
{
    public static UnitActionSystem Instance { get; private set; }
    public event EventHandler OnSelectedUnitChanged;

    [SerializeField] private Unit selectedUnit;
    [SerializeField] private LayerMask unitsLayerMask;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance= this;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (TryHandleUnitSelection()) return;

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            if (selectedUnit.GetMoveAction().IsValidActionGridPosition(mouseGridPosition))
                selectedUnit.GetMoveAction().Move(mouseGridPosition);

        }
    }

    public Unit GetSelectedUnit() { return selectedUnit; }

    private bool TryHandleUnitSelection()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);

        if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, unitsLayerMask))
            if (_rayCastHit.transform.TryGetComponent<Unit>(out Unit _unit))
            {
                SetSelectedUnit(_unit);
                return true;
            }

        return false;
    }

    private void SetSelectedUnit(Unit _unit)
    {
        selectedUnit = _unit;
        OnSelectedUnitChanged?.Invoke(this, EventArgs.Empty);
    }

}