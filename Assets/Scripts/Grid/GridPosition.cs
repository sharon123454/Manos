using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public struct GridPosition : IEquatable<GridPosition>
{
    public int _x;
    public int _y;
    public int _z;

    public GridPosition(int x, int z, int y) { this._x = x; this._z = z; this._y = y;
    }

    public override bool Equals(object obj) { return obj is GridPosition position && _x == position._x && _z == position._z && position._y == _y; }

    public bool Equals(GridPosition other) { return this == other; }

    public override int GetHashCode()
    {
        int hashCode = 929260398;
        hashCode = hashCode * -1521134295 + _x.GetHashCode();
        hashCode = hashCode * -1521134295 + _z.GetHashCode();
        hashCode = hashCode * -1521134295 + _y.GetHashCode();
        return hashCode;
    }

    public override string ToString() { return $"X: {_x}, Z: {_z}, Y: {_y}"; }

    public static bool operator ==(GridPosition a, GridPosition b) { return a._x == b._x && a._z == b._z; }

    public static bool operator !=(GridPosition a, GridPosition b) { return !(a == b); }

    public static GridPosition operator +(GridPosition a, GridPosition b) { return new GridPosition(a._x + b._x, a._z + b._z, a._y + b._y); }

    public static GridPosition operator -(GridPosition a, GridPosition b) { return new GridPosition(a._x - b._x, a._z - b._z, a._y + b._y); }

}