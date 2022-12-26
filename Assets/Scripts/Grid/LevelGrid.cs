using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class LevelGrid : MonoBehaviour
{
    public static LevelGrid Instance { get; private set; }
    public event EventHandler OnAnyUnitMovedGridPosition;

    [SerializeField] private Transform gridDebugObjectPrefab;
    private GridSystem<GridObject> gridSystem;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        gridSystem = new GridSystem<GridObject>(10, 10, 2f, 
            (GridSystem<GridObject> g, GridPosition gridPosition) => new GridObject(g, gridPosition));
        //gridSystem.CreateDebugObjects(PathFindingDebugObject);
    }

    public int GetWidth() { return gridSystem.GetWidth(); }

    public int GetLength() { return gridSystem.GetLength(); }

    public void AddUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).AddUnit(unit);
    }

    public List<Unit> GetUnitListAtGridPosition(GridPosition gridPosition)
    {
        return gridSystem.GetGridObject(gridPosition).GetUnitList();
    }

    public void RemoveUnitAtGridPosition(GridPosition gridPosition, Unit unit)
    {
        gridSystem.GetGridObject(gridPosition).RemoveUnit(unit);
    }

    public void UnitMovedGridPosition(Unit unit, GridPosition fromGridPosition, GridPosition toGridPosition)
    {
        RemoveUnitAtGridPosition(fromGridPosition, unit);

        AddUnitAtGridPosition(toGridPosition, unit);

        OnAnyUnitMovedGridPosition?.Invoke(this, EventArgs.Empty);
    }

    public GridPosition GetGridPosition(Vector3 worldPosition) { return gridSystem.GetGridPosition(worldPosition); }

    public Vector3 GetWorldPosition(GridPosition gridPosition) { return gridSystem.GetWorldPosition(gridPosition); }

    public bool IsValidGridPosition(GridPosition gridPosition) { return gridSystem.IsValidGridPosition(gridPosition); }

    public bool HasAnyUnitOnGridPosition(GridPosition gridPosition) { GridObject gridObject = gridSystem.GetGridObject(gridPosition); return gridObject.HasAnyUnit(); }

    public Unit GetUnitAtGridPosition(GridPosition gridPosition) { GridObject gridObject = gridSystem.GetGridObject(gridPosition); return gridObject.GetUnit(); }

}