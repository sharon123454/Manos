using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    protected Action onActionComplete;
    protected bool isActive;
    protected Unit unit;

    protected virtual void Awake() { unit = GetComponent<Unit>(); }

    protected void ActionComplete() { isActive = false; onActionComplete(); OnAnyActionCompleted?.Invoke(this, EventArgs.Empty); }

    protected void ActionStart(Action onActionComple) { isActive = true; this.onActionComplete = onActionComple; OnAnyActionStarted?.Invoke(this, EventArgs.Empty); }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public virtual int GetActionPointCost() { return 1; }

    public Unit GetUnit() { return unit; }

    public abstract string GetActionName();

}