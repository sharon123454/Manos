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

    protected void ActionComplete() { isActive = false; onActionComplete(); }

    protected void ActionStart(Action onActionComple) { isActive = true; this.onActionComplete = onActionComple; }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost() { return 1; }

    public abstract string GetActionName();

}