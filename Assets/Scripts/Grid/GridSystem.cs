using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GridSystem<TGridObject>
{
    private TGridObject[,,] gridObjectArray;
    private int _width, _length, _height;
    private float _cellSize;

    public GridSystem(int width, int length,int height, float cellSize, Func<GridSystem<TGridObject>, GridPosition, TGridObject> createGridObject)
    {
        this._width = width;
        this._length = length;
        this._height = height;
        this._cellSize = cellSize;

        gridObjectArray = new TGridObject[width, length,height];
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition _gridPosition = new GridPosition(x, z,height);
                gridObjectArray[x,z, height] = createGridObject(this, _gridPosition);
            }
        }
    }

    public Vector3 GetWorldPosition(GridPosition gridPosition)
    {
        return new Vector3(gridPosition._x, gridPosition._y, gridPosition._z ) * _cellSize;
    }

    public GridPosition GetGridPosition(Vector3 worldPosition)
    {
        return new GridPosition(
            Mathf.RoundToInt(worldPosition.x / _cellSize), Mathf.RoundToInt(worldPosition.z / _cellSize),Mathf.RoundToInt(worldPosition.y / _cellSize));
    }

    public void CreateDebugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z,_height);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public TGridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition._x, gridPosition._z,gridPosition._y];
    }

    public bool IsValidGridPosition(GridPosition gridPosition)
    {
        return gridPosition._x >= 0 &&
               gridPosition._z >= 0 &&
               gridPosition._x < _width &&
               gridPosition._z < _length;
    }

    public int GetWidth() { return _width; }
    public int GetHeight() { return _height; }

    public int GetLength() { return _length; }

}