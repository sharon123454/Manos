using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum Effectiveness
{
    Effective/* 0 - 1 */,
    Inaccurate/* 0 - 4, 5-9 */,
    Miss/* 2-4, 5 - 9, 10-15 */,
}
public struct GridPosition : IEquatable<GridPosition>
{
    public int _x;
    public int _z;
    public StatusEffect _currentEffect;
    private Effectiveness _effectiveness;
    public GridPosition(int x, int z)
    {
        this._x = x; this._z = z;
        _currentEffect = StatusEffect.None;
        _effectiveness = Effectiveness.Miss;
    }

    public void SetEffectiveRange(Effectiveness type) { _effectiveness = type; }
    public Effectiveness GetEffectiveRange() { return _effectiveness; }

    public override string ToString() { return $"X: {_x}, Z: {_z}"; }

    public override int GetHashCode()
    {
        int hashCode = 929260398;
        hashCode = hashCode * -1521134295 + _x.GetHashCode();
        hashCode = hashCode * -1521134295 + _z.GetHashCode();
        return hashCode;
    }
    public bool Equals(GridPosition other) { return this == other; }
    public override bool Equals(object obj) { return obj is GridPosition position && _x == position._x && _z == position._z; }

    #region Operators
    public static bool operator ==(GridPosition a, GridPosition b) { return a._x == b._x && a._z == b._z; }
    public static bool operator !=(GridPosition a, GridPosition b) { return !(a == b); }
    public static GridPosition operator +(GridPosition a, GridPosition b) { return new GridPosition(a._x + b._x, a._z + b._z); }
    public static GridPosition operator -(GridPosition a, GridPosition b) { return new GridPosition(a._x - b._x, a._z - b._z); }
    #endregion

}