using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private int actionPoints = 2;
    [SerializeField] private bool isEnemy;
    [SerializeField] private bool usedBonusAction;
    [SerializeField] private bool usedAction;
    //[SerializeField] private bool canUseAttackOfOpportunity; //prolly here

    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private int actionPointsMax;

    private void Awake()
    {
        actionPointsMax = actionPoints;
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        healthSystem.OnDeath += HealthSystem_OnDeath;

        OnAnyUnitSpawned?.Invoke(this, EventArgs.Empty);
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
    }

    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is T)
                return (T)baseAction;

        return null;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (baseAction._isBonusAction && !usedBonusAction)
        {
            if (!CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(true);
                // baseAction.usedAction = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else if (!baseAction._isBonusAction && !usedAction)
        {
            if (!CanSpendActionPointsToTakeAction(baseAction))
            {
                SpendActionPoints(false);
                //baseAction.usedAction = true;
                return true;
            }
            else
            {
                return false;
            }
        }
        else
        {
            return false;
        }
    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return baseAction.GetIfUsedAction();
    }

    public BaseAction[] GetBaseActionArray() { return baseActionArray; }

    public Vector3 GetWorldPosition() { return transform.position; }

    public GridPosition GetGridPosition() { return gridPosition; }

    public int GetActionPoints()
    {
        if (usedAction)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }
    
    public int GetBonusActionPoints() {
        if (usedBonusAction)
        {
            return 0;
        }
        else
        {
            return 1;
        }
    }

    public bool IsEnemy() { return isEnemy; }

    public float GetHealthNormalized()
    {
        return healthSystem.GetHealthNormalized();
    }

    //public void TakeAttackOfOppertunity(Unit unit)
    //{
    //    if (!canUseAttackOfOpportunity)
    //        return;

    //    ShootAction meleeAction = GetAction<ShootAction>();
    //    //meleeAction.TakeAction(unit.GetGridPosition(), TakeAttackOfOppertunity);
    //    canUseAttackOfOpportunity = false;
    //}

    public void Damage(float damage,float hitChance)
    {
        healthSystem.TakeDamage(damage, hitChance);
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() ||
            !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints = actionPointsMax;
            usedBonusAction = false;
            usedAction = false;
            //canUseAttackOfOpportunity = true;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void SpendActionPoints(bool isBonusAction)
    {
        if (isBonusAction)
        {
            usedBonusAction = true;
        }
        else
        {
            usedAction = true;
        }
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

}