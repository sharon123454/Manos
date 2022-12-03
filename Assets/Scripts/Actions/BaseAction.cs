using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    protected Action onActionComplete;
    protected bool isActive;
    protected Unit unit;

    protected virtual void Awake() { unit = GetComponent<Unit>(); }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract string GetActionName();

}