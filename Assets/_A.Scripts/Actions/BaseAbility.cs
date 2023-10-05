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
// Transitions in the life of an Ability
public enum AbilityState
{
    StartOfAction,// Prepare Ability parameters and visual state
    ExecutingAction,// Execute Ability function
    EndOfAction// Ease back to out of ability state
}
public class BaseAbility : BaseAction
{
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
    protected Vector3 actionAimDirection;
    private List<Unit> _aoEUnitTargets = new List<Unit>();
    public List<Unit> GetAoETargets() { return _aoEUnitTargets; }
    public void SetTargetByAoE(List<Unit> aoEUnitTargets)
    {
        targetUnit = null;
        _aoEUnitTargets.Clear();
        actionAimDirection = MouseWorld.GetPosition();
        foreach (Unit unit in aoEUnitTargets)
            _aoEUnitTargets.Add(unit);
    }

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

    public float GetDamage() { return damage; }
    public int GetCritChance() { return critChance; }
    public float GetAbilityHitChance() { return hitChance; }
    public float GetPostureDamage() { return postureDamage; }
    public int GetStatusChance() { return statusEffectChance; }
    public List<StatusEffect> GetStatusEffects() { return EnemyStatusEffects; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        //Setting Action to starting state and resetting timer before activation on Update
        _actionStateTimer = startOfActionTime;
        _state = AbilityState.StartOfAction;

        //Activating Action self Buffs
        if (selfBuffs.Count > 0)
            SetSelfBuff();

        //for specific AOEs
        if (AOEPrefab)
            AOEPrefab.Init(GetUnit().IsEnemy(), LevelGrid.Instance.GetWorldPosition(gridPosition), isFollowingUnit, AOEActiveTurns, GetStatusEffects(), GetMeshScaleMultiplicator());

        //overridable for method for children
        OnActionStart();
        //start of the action
        ActionStart(actionComplete);
    }
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
                return GetGridPositionListByRange(9);
            case ActionRange.ResetGrid:
                Debug.Log($"Ability {name}: Has No Valid Grid" + "- ERROR(Switch action Range)");
                return null;
            default:
                Debug.Log($"Ability {name}: Range isn't implamented" + "- ERROR(Switch action Range)");
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

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition) && !IsXPropertyInAction(AbilityProperties.AreaOfEffect)) // If grid position has no unit and not AOE
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);
                if (targetUnit)
                {
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
                }

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

    /// <summary>
    /// Starting state of an action during update
    /// </summary>
    protected virtual void StartOfActionUpdate() { }
    /// <summary>
    /// Executing state of an action during update
    /// </summary>
    protected virtual void ExecutionOfActionUpdate() { }
    /// <summary>
    /// End state of an action during update
    /// </summary>
    protected virtual void EndOfActionUpdate() { }

    /// <summary>
    /// Right before action start state starts updating
    /// </summary>
    protected virtual void OnActionStart() { }
    /// <summary>
    /// Right before action execution starts updates
    /// </summary>
    protected virtual void OnActionExecution() { }
    /// <summary>
    /// Right before action end starts updates
    /// </summary>
    protected virtual void OnActionEnded() { }

    //State Machine Switch
    private void NextState()
    {
        switch (_state)
        {
            case AbilityState.StartOfAction:
                OnActionExecution();
                _state = AbilityState.ExecutingAction;
                _actionStateTimer = executingActionTime;
                break;
            case AbilityState.ExecutingAction:
                OnActionEnded();
                _state = AbilityState.EndOfAction;
                _actionStateTimer = endOfActionTime;
                break;
            case AbilityState.EndOfAction:
                ActionComplete();
                break;
        }
    }

}