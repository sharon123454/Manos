using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridObject
{
    private GridSystem _gridSystem;
    private GridPosition _gridPosition;

    public GridObject(GridSystem gridSystem, GridPosition gridPosition)
    {
        this._gridSystem = gridSystem;
        this._gridPosition = gridPosition;
    }

    public override string ToString()
    {
        return _gridPosition.ToString();
    }

}