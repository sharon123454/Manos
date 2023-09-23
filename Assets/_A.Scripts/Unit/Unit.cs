using System.Collections.Generic;
using UnityEngine;
using System;

public enum UnitType { Player, Enemy, Both }
public class Unit : MonoBehaviour
{
    [SerializeField] private bool isEnemy;

    [Space]
    [Header("Dev Tools:")]
    [SerializeField] private bool usedBonusAction;
    [SerializeField] private bool usedAction;
    [SerializeField] private bool isStunned;
    //[SerializeField] private bool isInCombat;
    /*[SerializeField] */
    private bool canMakeAttackOfOpportunity = true;

    public static event EventHandler<string> SendConsoleMessage;
    public static event EventHandler OnAnyActionPointsChanged;
    public static event EventHandler OnAnyUnitSpawned;
    public static event EventHandler OnAnyUnitDead;

    [HideInInspector] public UnitStatusEffects unitStatusEffects;
    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private UnitAnimator animator;
    private Outline unitOutline;
    private UnitStats statSheet;
    private GameObject unitUI;

    private void Awake()
    {
        if (!isEnemy)
            unitUI = GameObject.Find($"UI_{name}Portrait");

        statSheet = GetComponent<UnitStats>();
        animator = GetComponent<UnitAnimator>();
        baseActionArray = GetComponents<BaseAction>();
        unitStatusEffects = GetComponent<UnitStatusEffects>();
        unitOutline = GetComponentInChildren<Outline>();
    }
    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        statSheet.OnDeath += StatSheet_OnDeath;
        statSheet.OnDodge += StatSheet_OnDodge;
        statSheet.OnHeal += StatSheet_OnHealed;
        statSheet.OnDamaged += StatSheet_OnDamaged;
        statSheet.OnCriticalHit += StatSheet_OnCriticallyHit;
        unitStatusEffects.OnStatusApplied += StatSheet_OnStatusApplied; ;
        unitStatusEffects.OnStatusRemoved += UnitStatusEffects_OnStatusRemoved;

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

    public GameObject GetUnitUI() { return unitUI; }
    public UnitAnimator GetUnitAnimator() { return animator; }
    public float GetHealthNormalized()
    {
        return statSheet.GetHealthNormalized();
    }
    public float GetPostureNormalized()
    {
        return statSheet.GetPostureNormalized();
    }
    public bool IsEnemy() { return isEnemy; }
    public UnitStats GetUnitStats() { return statSheet; }
    public void SetGridEffectiveness(Effectiveness _effectiveness) { gridPosition.SetEffectiveRange(_effectiveness); }
    public Effectiveness GetGridEffectiveness() { return gridPosition.GetEffectiveRange(); }
    public GridPosition GetGridPosition() { return gridPosition; }
    public Vector3 GetWorldPosition() { return transform.position; }

    public BaseAction[] GetBaseActionArray() { return baseActionArray; }
    public T GetAction<T>() where T : BaseAction
    {
        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is T)
                return (T)baseAction;

        return null;
    }

    public bool GetUsedBothActions()
    {
        return usedAction && usedBonusAction;
    }
    public bool GetUsedAction()
    {
        return usedAction;
    }
    public bool GetUsedBonusAction()
    {
        return usedBonusAction;
    }

    public bool TrySpendActionPointsToTakeAction(BaseAction baseAction)
    {
        if (GetIsStunned())
        {
            SendConsoleMessage?.Invoke(this, $"{transform.name} is Stunned, Can't use action.");
            return false;
        }

        #region Move Or Dash
        string actionName = baseAction.GetActionName();

        if (actionName == "Move" || actionName == "Dash")
        {
            Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
            if (selectedunit.unitStatusEffects.ContainsEffect(StatusEffect.Root))
            {
                return false;
            }
        }
        #endregion

        if (baseAction.GetCurrentCooldown() == 0)
        {
            switch (baseAction.GetActionCost())
            {
                case TypeOfAction.Action:
                    #region Action
                    if (!GetUsedAction() && !CanSpendActionPointsToTakeAction(baseAction))
                    {
                        if (TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        if (!TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanEnemySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        SendConsoleMessage?.Invoke(this, $"{transform.name} used {baseAction.GetActionName()}.");
                        SpendActionPoints(0);
                        return true;
                    }
                    #endregion
                    return false;
                case TypeOfAction.BonusAction:
                    #region Bonus Action
                    if (!GetUsedBonusAction() && !CanSpendActionPointsToTakeAction(baseAction))
                    {
                        if (TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        if (!TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanEnemySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        SendConsoleMessage?.Invoke(this, $"{transform.name} used {baseAction.GetActionName()}.");
                        SpendActionPoints(1);
                        return true;
                    }
                    #endregion
                    return false;
                case TypeOfAction.Both:
                    #region Action And Bonus Action
                    if (!GetUsedBonusAction() && !GetUsedAction() && !CanSpendActionPointsToTakeAction(baseAction))
                    {
                        if (TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        if (!TurnSystem.Instance.IsPlayerTurn() && !MagicSystem.Instance.CanEnemySpendFavorToTakeAction(baseAction.GetFavorCost()))
                            return false;

                        SendConsoleMessage?.Invoke(this, $"{transform.name} used {baseAction.GetActionName()}.");
                        SpendActionPoints(2);
                        return true;
                    }
                    #endregion
                    return false;
                default:
                    Debug.Log("I'm not supposed to be called");
                    return false;
            }
        }

        return false;
    }
    public bool CanSpendActionPointsToTakeAction(BaseAction baseAction)
    {
        return baseAction.GetIfUsedAction();
    }

    public bool GetIsStunned() { return isStunned; }
    public void SetStunStatus(bool newStatus) { isStunned = newStatus; }

    public bool ReturnSkillActionType(BaseAction baseAction)
    {
        return baseAction.GetIfUsedAction();
    }

    public StatusEffect GetGridStatusEffect() { return gridPosition._currentEffect; }
    public StatusEffect SetGridStatusEffect(StatusEffect currenEffect) { return gridPosition._currentEffect = currenEffect; }

    private void SpendActionPoints(int type)
    {
        if (type == 0) { usedAction = true; }
        if (type == 1) { usedBonusAction = true; }
        if (type == 2) { usedAction = true; usedBonusAction = true; }

        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public void Heal(float healValue)
    {
        statSheet.Heal(healValue);
        SendConsoleMessage?.Invoke(this, $"{name} was healed for {healValue}");
    }
    public void Block() { statSheet.Block(); }
    public void Dodge() { statSheet.Dodge(); }
    public void Damage(float damage, float postureDamage, float hitChance, float abilityCritChance, List<StatusEffect> abilityEffect, List<AbilityProperties> AP, int AbilityhitChance, int Duration)
    {
        statSheet.TryTakeDamage(damage, postureDamage, hitChance, abilityCritChance, abilityEffect, AP, AbilityhitChance, Duration);
    }

    #region Attack of Oppertunity
    public void Disengage() { SetEngagementInCombat(false); }
    public void SetEngagementInCombat(bool engagementInCombat)
    {
        //isInCombat = engagementInCombat;
    }
    public void TryTakeAttackOfOppertunity(Unit rangeLeavingUnit)
    {
        if (!canMakeAttackOfOpportunity)
            return;

        //if (!rangeLeavingUnit.isInCombat)
        //return;

        //if (!IsUnitUsingMelee())//will need refactor if weapon is changable
        //    return;

        MeleeAction unitMeleeAction = null;

        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is MeleeAction)
                unitMeleeAction = baseAction as MeleeAction;

        //if (unitMeleeAction)
        //    unitMeleeAction.TakeAction(rangeLeavingUnit.GetGridPosition(), AttackOfOppertunityComplete);
    }
    private void AttackOfOppertunityComplete()
    {
        canMakeAttackOfOpportunity = false;
        print("AOO is complete");
    }
    private bool IsUnitUsingMelee()
    {
        foreach (BaseAction baseAction in baseActionArray)
            if (baseAction is MeleeAction)
                return baseAction.enabled;

        return false;
    }
    #endregion

    //Events
    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() ||
        !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            usedAction = false;
            usedBonusAction = false;
            if (GetIsStunned()) { usedAction = true; usedBonusAction = true; }
            statSheet.ResetUnitStats();
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
        }
    }
    private void StatSheet_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        unitOutline.enabled = false;
        animator.OnDead();
        OnAnyUnitDead?.Invoke(this, EventArgs.Empty);
    }

    private void StatSheet_OnDamaged(object sender, EventArgs e)
    {
        animator.OnDamaged();
    }
    private void StatSheet_OnCriticallyHit(object sender, EventArgs e)
    {
        animator.OnCriticallyHit();
    }
    private void StatSheet_OnDodge(object sender, EventArgs e)
    {
        animator.OnDodge();
    }
    private void StatSheet_OnHealed(object sender, EventArgs e)
    {
        //animator.
    }
    private void StatSheet_OnStatusApplied(object sender, StatusEffect activatedEffect)
    {
        animator.OnStatusEffectRecieved(activatedEffect);
    }
    private void UnitStatusEffects_OnStatusRemoved(object sender, StatusEffect removedStatus)
    {
        animator.OnStatusEffectRemoved(removedStatus);
    }

}