using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    [SerializeField] protected bool _isBonusAction;

    protected Action onActionComplete;
    protected Unit unit;
    protected EnemyAIBehaivors aiBehaivor;
    protected bool _isActive;
    protected bool _usedAction;
    protected bool _usedBonusAction;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        if (unit.IsEnemy())
        {
            aiBehaivor = GetComponent<EnemyAIBehaivors>();
        }
    }

    protected void ActionComplete()
    {
        _isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    protected void ActionStart(Action onActionComple)
    {
        _isActive = true;
        this.onActionComplete = onActionComple;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit() { return unit; }
    public virtual bool GetIfUsedAction() { return _usedAction; }
    public virtual bool GetIfUsedBonusAction() { return _usedBonusAction; }
    public virtual bool GetIsBonusAction() { return _isBonusAction; }

    public abstract string GetActionName();

    public abstract List<GridPosition> GetValidActionGridPositionList();

    public abstract EnemyAIAction GetEnemyAIAction(GridPosition gridPosition);

    public virtual bool IsValidActionGridPosition(GridPosition gridPosition)
    {
        List<GridPosition> validGridPositionList = GetValidActionGridPositionList();
        return validGridPositionList.Contains(gridPosition);
    }

    public abstract void TakeAction(GridPosition gridPosition, Action actionComplete);

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