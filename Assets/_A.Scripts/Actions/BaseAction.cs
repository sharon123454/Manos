using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    public static event EventHandler OnAnyActionStarted;
    public static event EventHandler OnAnyActionCompleted;

    [SerializeField] protected string actionDescription = "Description...";
    [SerializeField] protected bool _isBonusAction;
    [SerializeField] protected int cooldown, addCooldown;
    [SerializeField] protected int favorCost = 100;
    [SerializeField] protected AbilityRange range;
    [SerializeField] protected List<AbilityProperties> _AbilityProperties;

    protected Action onActionComplete;
    protected Unit unit;
    protected bool _isActive;
    protected bool _usedAction;

    BaseAbility baseAbility;

    protected virtual void Awake()
    {
        unit = GetComponent<Unit>();
        if (this is BaseAbility)
        {
            baseAbility = GetComponent<BaseAbility>();
        }
    }

    protected virtual void Start()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        DivineFavorAction.OnDivineActive += BaseAction_OnDivineActive;
    }

    public AbilityRange ReturnRange() { return range; }
    private void BaseAction_OnDivineActive(object sender, EventArgs e)
    {
        Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
        if (unit == selectedunit)
            if (cooldown > 0)
                cooldown--;
    }

    private void Instance_OnTurnChange(object sender, EventArgs e)
    {
        if (unit.IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
        {
            if (cooldown > 0)
                cooldown--;
        }

        if (!unit.IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            if (cooldown > 0)
                cooldown--;
        }
    }

    public virtual string GetActionDescription() { return actionDescription; }
    public virtual int GetFavorCost() { return favorCost; }

    protected void ActionComplete()
    {
        _isActive = false;
        onActionComplete();
        OnAnyActionCompleted?.Invoke(this, EventArgs.Empty);
        UnitActionSystem.Instance.InvokeAbilityFinished();
    }

    protected void ActionStart(Action onActionComple)
    {

        cooldown += addCooldown;
        _isActive = true;
        this.onActionComplete = onActionComple;
        OnAnyActionStarted?.Invoke(this, EventArgs.Empty);
    }

    public Unit GetUnit() { return unit; }
    public virtual bool GetIfUsedAction() { return _usedAction; }
    public virtual bool GetIsBonusAction() { return _isBonusAction; }
    public virtual int GetCooldown() { return cooldown; }
    public List<AbilityProperties> GetAbilityPropertie() { return _AbilityProperties; }

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