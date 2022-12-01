using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

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

    public void CreateDebugObjects(Transform debugPrefab)
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

public struct GridPosition : IEquatable<GridPosition>
{
    public int _x;
    public int _z;

    public GridPosition(int x, int z) { this._x = x; this._z = z; }

    public override bool Equals(object obj) { return obj is GridPosition position && _x == position._x && _z == position._z; }

    public bool Equals(GridPosition other) { return this == other; }

    public override int GetHashCode()
    {
        int hashCode = 929260398;
        hashCode = hashCode * -1521134295 + _x.GetHashCode();
        hashCode = hashCode * -1521134295 + _z.GetHashCode();
        return hashCode;
    }

    public override string ToString() { return $"X: {_x}, Z: {_z}"; }

    public static bool operator ==(GridPosition a, GridPosition b) { return a._x == b._x && a._z == b._z; }

    public static bool operator !=(GridPosition a, GridPosition b) { return !(a == b); }

    public static GridPosition operator +(GridPosition a, GridPosition b) { return new GridPosition(a._x + b._x, a._z + b._z); }

    public static GridPosition operator -(GridPosition a, GridPosition b) { return new GridPosition(a._x - b._x, a._z - b._z); }

}