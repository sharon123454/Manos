using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

// Types of effects added after use of Ability
public enum StatusEffect
{
    None,//Default State
    Stun,//Miss X Turns
    Silence,//While affected the target cant use abilities
    Root,//Cant Move,but can use Abilities 
    ArmorBreak,//Ignore Armor
    GainArmor,//Gain Armor
    Haste,//Double the move Speed
    Blind,//Halves the unit's chance to hit attacks  ((Affect on posture broken units?))
    Undying,//Affected unit's hp Cant go below 1 HP for x turns
    Regeneration,//Affected unit regains a set amount of HP at the end of the unit's turn
    Corruption,//Affected unit suffers damage at the beginning of their turn
    CowardPlague,//If the affected enemy unit exits status inflicter's melee range they receive the ability's damage and posture damage again
    Nullify,//??
    Invisibility,//
    Taunt,//
    ToBeTauntUnused,//
}
// Transition states in the life of an Ability
public enum AbilityState
{
    StartOfAction,// Prepare Ability parameters and visual state
    ExecutingAction,// Execute Ability function
    EndOfAction// Ease back out of ability state
}
public class BaseAbility : BaseAction
{
    //public static event EventHandler OnAnyAbiltyHit;

    [Header("Ability")]
    [SerializeField] protected float damage = 10f;
    [SerializeField] protected float postureDamage = 0f;
    [SerializeField] protected int hitChance = 100, critChance = 25;
    [SerializeField] protected int statusEffectChance = 100, statusEffectDuration = 1;
    [SerializeField] protected List<StatusEffect> EnemyStatusEffects;
    [Header("Self Casting")]
    [Space]
    [SerializeField] protected int selfBuffDuration = 1;
    [SerializeField] protected List<StatusEffect> selfBuffs;

    [Header("Dev tools")]
    [SerializeField] private float startOfActionTime = 1f;
    [SerializeField] private float executingActionTime = 0.1f;
    [SerializeField] private float endOfActionTime = 0.1f;

    protected Unit targetUnit;
    //protected Vector3 targetDirection;
    //protected List<Unit> aOETargetUnits = new List<Unit>(5);
    protected float rotateToTargetSpeed = 10f;

    private float _actionStateTimer;
    private AbilityState _state;

    void Update()
    {
        if (!_isActive)
            return;

        _actionStateTimer -= Time.deltaTime;

        switch (_state)
        {
            case AbilityState.StartOfAction:
                StartOfActionUpdate();
                break;
            case AbilityState.ExecutingAction:
                ExecutionOfActionUpdate();
                break;
            case AbilityState.EndOfAction:
                EndOfActionUpdate();
                break;
        }

        if (_actionStateTimer <= 0f)
            NextState();
    }

    public Unit GetTargetUnit() { return targetUnit; }
    public float GetDamage() { return damage; }
    public int GetCritChance() { return critChance; }
    public float GetAbilityHitChance() { return hitChance; }
    public float GetPostureDamage() { return postureDamage; }
    public int GetStatusChance() { return statusEffectChance; }
    public List<StatusEffect> GetStatusEffects() { return EnemyStatusEffects; }

    // targetUnit.Damage(damage* 2, postureDamage, hitChance, critChance, _abilityEffect, statusEffectChance, statusEffectDuration);

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        _state = AbilityState.StartOfAction;
        _actionStateTimer = startOfActionTime;
        // cooldown += addCooldown;
        if (selfBuffs.Count > 0)
            SetSelfBuff();
        //gridPosition + unit.GetGridPosition() 
        //HandleAbilityRange();

        //if (AOEPrefab)
        //{
        //    AOEPrefab.Init(GetUnit(), LevelGrid.Instance.GetWorldPosition(gridPosition), isFollowingUnit, AOEActiveTurns, GetStatusEffects(), GetMeshScaleMultiplicator());
        //}
    }
    //public override void TakeAction(Vector3 mousePosition, Action actionComplete)
    //{
    //    _state = AbilityState.StartOfAction;
    //    _actionStateTimer = startOfActionTime;
    //    targetDirection = mousePosition;

    //    if (selfBuffs.Count > 0)
    //        SetSelfBuff();
    //}

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 0 };
    }
    public override List<GridPosition> GetValidActionGridPositionList()//implements ranges
    {
        switch (GetRange())
        {
            case ActionRange.Move:
                return null;
            case ActionRange.Self:
                return new List<GridPosition> { GetUnit().GetGridPosition() };
            case ActionRange.Melee:
                return GetGridPositionListByRange(1);
            case ActionRange.Close:
                return GetGridPositionListByRange(6);
            case ActionRange.Medium:
            case ActionRange.Long:
            case ActionRange.EffectiveAtAll:
            case ActionRange.InaccurateAtAll:
                print($"Ability range: {GetRange()}");
                return GetGridPositionListByRange(9);
            case ActionRange.ResetGrid:
                Debug.Log($"Ability {name}: has No Valid Grid");
                return null;
            default:
                Debug.Log("Range isn't implamented");
                return null;
        }
    }
    private List<GridPosition> GetGridPositionListByRange(int includeRange)
    {
        GridPosition _unitGridPosition = GetUnit().GetGridPosition();
        List<GridPosition> _validGridPositionList = new List<GridPosition>();

        for (int x = -includeRange; x <= includeRange; x++)
        {
            for (int z = -includeRange; z <= includeRange; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = _unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If grid valid
                    continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If grid position has no unit
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (GetUnit().IsEnemy() && targetUnit.GetUnitStats().getUnitStatusEffects().unitActiveStatusEffects.Contains(StatusEffect.Taunt))
                {
                    _validGridPositionList.Clear();
                    _validGridPositionList.Add(testGridPosition);
                    break;
                }

                if (targetUnit.IsEnemy() == GetUnit().IsEnemy() && !IsXPropertyInAction(AbilityProperties.Heal))// Both units on the same team
                    continue;

                if (!targetUnit.IsEnemy() == !GetUnit().IsEnemy() && targetUnit.unitStatusEffects.unitActiveStatusEffects.Contains(StatusEffect.Invisibility))// Both units on the same team
                    continue;

                if (ValidationGridChecks())//Childrens special exception
                    _validGridPositionList.Add(testGridPosition);
            }
        }
        return _validGridPositionList;
    }

    public virtual void SetSelfBuff()
    {
        foreach (StatusEffect effect in selfBuffs)
            UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.AddStatusEffectToUnit(effect, selfBuffDuration);
    }

    protected virtual bool ValidationGridChecks() { return true; }

    protected virtual void StartOfActionUpdate()
    {
        //if (IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        //{
        //    foreach (Unit unit in AOEManager.Instance.GetUnitsInRange())
        //    {
        //        if (!unit.IsEnemy())
        //        {
        //            aOETargetUnits.Add(unit);
        //            print(unit.name + "was added by AOE");
        //        }
        //    }
        //}
    }
    protected virtual void ExecutionOfActionUpdate() { }
    protected virtual void EndOfActionUpdate() { /*aOETargetUnits.Clear();*/ }

    protected virtual void OnActionStartChange() { }
    protected virtual void OnActionExecutionChange() { }
    protected virtual void OnActionEndChange() { }

    private void NextState()
    {
        switch (_state)
        {
            case AbilityState.StartOfAction:
                _state = AbilityState.ExecutingAction;
                _actionStateTimer = executingActionTime;
                OnActionStartChange();
                break;
            case AbilityState.ExecutingAction:
                _state = AbilityState.EndOfAction;
                _actionStateTimer = endOfActionTime;
                OnActionExecutionChange();
                break;
            case AbilityState.EndOfAction:
                OnActionEndChange();
                ActionComplete();
                break;
        }
    }

}