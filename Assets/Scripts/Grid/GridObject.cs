using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private List<Unit> _unitList;
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
        _unitList = new List<Unit>();
    }

    public void AddUnit(Unit unit) { _unitList.Add(unit); }

    public void RemoveUnit(Unit unit) { _unitList.Remove(unit); }

    public List<Unit> GetUnitList() { return _unitList; }

    public override string ToString()
    {
        string unitString = "";
        foreach (Unit unit in _unitList)
            unitString+= unit + "\n";

        return _gridPosition.ToString() + "\n" + unitString;
    }

}