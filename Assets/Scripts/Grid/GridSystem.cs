using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GridSystem<TGridObject>
{
    private TGridObject[,] gridObjectArray;
    private int _width, _length;
    private float _cellSize;

    public GridSystem(int width, int length, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this._width = width;
        this._length = length;
        this._cellSize = cellSize;

        gridObjectArray = new TGridObject[width, length];
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition _gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = createGridObject(this, _gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition._x, 0, gridPosition._z) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize));
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition) as GridObject);
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition._x, gridPosition._z];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition._x >= 0 &&
               gridPosition._z >= 0 &&
               gridPosition._x < _width &&
               gridPosition._z < _length;
    }

    public int GetWidth() { return _width; }

    public int GetLength() { return _length; }

}