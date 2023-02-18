using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using TMPro;

public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;
    [SerializeField] private bool usedBonusAction;
    [SerializeField] private bool usedAction;
    [SerializeField] private bool isStunned;

    public static event EventHandler<string> SendConsoleMessage;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [HideInInspector] public UnitStatusEffects unitStatusEffects;
    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private UnitStats unitStats;

    //serializeField to see changes live (unnecessary)
    [SerializeField] private bool canMakeAttackOfOpportunity = true;
    private bool engagedInCombat;

    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();
        baseActionArray = GetComponents<BaseAction>();
        unitStatusEffects = GetComponent<UnitStatusEffects>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        unitStats.OnDeath += HealthSystem_OnDeath;

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

    public void SetEngagementInCombat(bool engagementInCombat)
    {
        engagedInCombat = engagementInCombat;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        #region Move Or Dash

        if (baseAction.GetActionName() == "Move" || baseAction.GetActionName() == "Dash")
        {
            Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
            if (selectedunit.unitStatusEffects.ContainsEffect(StatusEffect.Root))
            {
                return false;
            }
        }

        #endregion


        #region Bonus Action
        if (baseAction.GetIsBonusAction() && !usedBonusAction)
        {
            if (!CanSpendActionPointsToTakeAction(baseAction) && baseAction.GetCooldown() == 0 && !isStunned)
            {
                if (TurnSystem.Instance.IsPlayerTurn())
                    if (!MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost()))
                    {
                        return false;
                    }

                if (!TurnSystem.Instance.IsPlayerTurn())
                    if (!MagicSystem.Instance.CanEnemySpendFavorToTakeAction(baseAction.GetFavorCost()))
                    {
                        return false;
                    }

                SendConsoleMessage?.Invoke(this, $"{transform.name} used {baseAction.GetActionName()}.");
                SpendActionPoints(true);
                // baseAction._usedAction = true;
                return true;
            }
            else
                return false;
        }


        #endregion


        #region Action
        else if (!baseAction.GetIsBonusAction() && !usedAction && baseAction.GetCooldown() == 0 && !isStunned)
        {
            if (!CanSpendActionPointsToTakeAction(baseAction))
            {
                if (TurnSystem.Instance.IsPlayerTurn())
                    if (!MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost()))
                    {
                        return false;
                    }


                if (!TurnSystem.Instance.IsPlayerTurn())
                    if (!MagicSystem.Instance.CanEnemySpendFavorToTakeAction(baseAction.GetFavorCost()))
                    {
                        return false;
                    }


                SendConsoleMessage?.Invoke(this, $"{transform.name} used {baseAction.GetActionName()}.");
                SpendActionPoints(false);
                //baseAction._usedAction = true;
                return true;
            }
            else
                return false;
        }
        else
            return false;

        #endregion

    }

    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return baseAction.GetIfUsedAction();
    }

    public BaseAction[] GetBaseActionArray() { return baseActionArray; }

    public bool ReturnSkillActionType(BaseAction baseAction)
    {
        return baseAction.GetIfUsedAction();
    }

    public UnitStats GetUnitStats() { return unitStats; }
    public Vector3 GetWorldPosition() { return transform.position; }
    public Effectiveness SetGridEffectivness(Effectiveness effective) { return gridPosition.range = effective; }
    public Effectiveness GetGridEffectivness() { return gridPosition.range; }

    public StatusEffect GetGridStatusEffect() { return gridPosition.currentEffect; }
    public StatusEffect SetGridStatusEffect(StatusEffect currenEffect) { return gridPosition.currentEffect = currenEffect; }

    public GridPosition GetGridPosition() { return gridPosition; }

    public bool GetStunStatus() { return isStunned; }
    public bool ChangeStunStatus(bool newStatus) { return isStunned = newStatus; }
    public bool GetUsedActionPoints()
    {
        if (usedAction)
            return true;
        else
            return false;
    }

    public bool GetUsedBonusActionPoints()
    {
        if (usedBonusAction)
            return true;
        else
            return false;
    }

    public bool IsEnemy() { return isEnemy; }

    public float GetHealthNormalized()
    {
        return unitStats.GetHealthNormalized();
    }

    public float GetPostureNormalized()
    {
        return unitStats.GetPostureNormalized();
    }

    public void Damage(float damage, float postureDamage, float hitChance, float abilityCritChance, StatusEffect abilityEffect, int AbilityhitChance, int Duration)
    {
        unitStats.TryTakeDamage(damage, postureDamage, hitChance, abilityCritChance, abilityEffect, AbilityhitChance, Duration);
    }
    //public void StatusEffect()
    //{
    //    unitStats.TryToTakeStatusEffect();
    //}

    public void Dodge() { unitStats.Dodge(); }

    public void Block() { unitStats.Block(); }

    public void Disengage() { SetEngagementInCombat(false); }

    public void TryTakeAttackOfOppertunity(Unit rangeLeavingUnit)
    {
        if (!canMakeAttackOfOpportunity)
            return;

        if (!rangeLeavingUnit.engagedInCombat)
            return;

        if (!IsUnitUsingMelee())//will need refactor if weapon is changable
            return;

        MeleeAction unitMeleeAction = null;

        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is MeleeAction)
                unitMeleeAction = baseAction as MeleeAction;

        if (unitMeleeAction)
            unitMeleeAction.TakeAction(rangeLeavingUnit.GetGridPosition(), AttackOfOppertunityComplete);
    }

    private void AttackOfOppertunityComplete()
    {
        canMakeAttackOfOpportunity = false;
        print("AOO is complete");
    }

    private void SpendActionPoints(bool isBonusAction)
    {
        if (isBonusAction)
            usedBonusAction = true;
        else
            usedAction = true;

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    private bool IsUnitUsingMelee()
    {
        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is MeleeAction)
                return baseAction.enabled;

        return false;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() ||
            !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            usedBonusAction = false;
            usedAction = false;
            canMakeAttackOfOpportunity = true;
            unitStats.ResetUnitStats();
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

}