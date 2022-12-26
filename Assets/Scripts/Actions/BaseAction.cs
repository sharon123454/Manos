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
    public bool usedAction;
    protected Unit unit;
    public bool _isBonusAction;
    public int actionCost;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
    }

    protected void ActionComplete()
    {
        isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionStart(Action onActionComple)
    {
        isActive = true;
        this.onActionComplete = onActionComple;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

    public virtual int GetActionPointCost() { return actionCost; }
    public virtual bool GetIfUsedAction() { return usedAction; }
    public virtual bool GetIsBonusAction() { return _isBonusAction; }


    public abstract string GetActionName();

    public Unit GetUnit() { return unit; }

    public EnemyAIAction GetBestEnemyAIAction()
    {
        List<EnemyAIAction> _enemyAIActionList = new List<EnemyAIAction>();

        List<GridPosition> _validGridPositionList = GetValidActionGridPositionList();

        foreach (GridPosition gridPosition in _validGridPositionList)
        {
            EnemyAIAction enemyAIAction = GetEnemyAIAction(gridPosition);
            _enemyAIActionList.Add(enemyAIAction);
        }

        if (_enemyAIActionList.Count > 0)
        {
            _enemyAIActionList.Sort((EnemyAIAction a, EnemyAIAction b) => b.actionValue - a.actionValue);
            return _enemyAIActionList[0];
        }
        else
            return null; // No Possible AI Actions
    }

}