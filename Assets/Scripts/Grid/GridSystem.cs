using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GridSystem
{
    private GridObject[,] gridObjectArray;
    private int _width, _length;
    private float _cellSize;

    public GridSystem(int width, int length, float cellSize)
    {
        this._width = width;
        this._length = length;
        this._cellSize = cellSize;

        gridObjectArray = new GridObject[width, length];
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition _gridPosition = new GridPosition(x, z);
                gridObjectArray[x,z] = new GridObject(this, _gridPosition);
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

    public void CreateDubugObjects(Transform debugPrefab)
    {
        for (int x = 0; x < _width; x++)
        {
            for (int z = 0; z < _length; z++)
            {
                GridPosition gridPosition = new GridPosition(x,z);

                Transform debugTransform = GameObject.Instantiate(debugPrefab, GetWorldPosition(gridPosition), Quaternion.identity);
                GridDebugObject gridDebugObject = debugTransform.GetComponent<GridDebugObject>();
                gridDebugObject.SetGridObject(GetGridObject(gridPosition));
            }
        }
    }

    public GridObject GetGridObject(GridPosition gridPosition)
    {
        return gridObjectArray[gridPosition._x, gridPosition._z];
    }

}

public struct GridPosition
{
    public int _x;
    public int _z;

    public GridPosition(int x, int z)
    {
        this._x = x;
        this._z = z;
    }

    public override string ToString()
    {
        return $"X: {_x}, Z: {_z}";
    }
}