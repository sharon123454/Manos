using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class PathNode
{
    private GridPosition _gridPosition;
    private PathNode cameFromPathNode;
    private bool _isOccupied = false;
    private bool _isWalkable = true;
    private bool _isWalkableForEnemy = true;
    private int _gCost;
    private int _hCost;
    private int _fCost;

    public PathNode(GridPosition gridPosition)
    {
        _gridPosition = gridPosition;
    }

    public int GetGCost() { return _gCost; }
    public int GetFCost() { return _fCost; }
    public int GetHCost() { return _hCost; }
    public bool IsWalkable() { return _isWalkable; }
    public bool IsWalkableForEnemy() { return _isWalkableForEnemy; }
    public bool IsOccupied() { return _isOccupied; }

    public void SetGCost(int gCost) { _gCost = gCost; }
    public void SetHCost(int hCost) { _hCost = hCost; }
    public void CalculateFCost() { _fCost = _gCost + _hCost; }
    public void SetIsWalkable(bool isWalkable) { _isWalkable = isWalkable; }
    public void SetIsWalkableForEnemy(bool isWalkable) { _isWalkableForEnemy = isWalkable; }

    public void SetIsOccupied(bool isOccupied) { _isOccupied = isOccupied; }

    public GridPosition GetGridPosition() { return _gridPosition; }

    public void ResetCameFromPathNode() { cameFromPathNode = null; }

    public PathNode GetCameFromPathNode() { return cameFromPathNode; }

    public void SetCameFromPathNode(PathNode pathNode) { cameFromPathNode = pathNode; }

    public override string ToString() { return _gridPosition.ToString(); }

}