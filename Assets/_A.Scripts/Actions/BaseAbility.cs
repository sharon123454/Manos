using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum AbilityRange
{
    Move,
    Self,
    Melee/* 0 - 1 */,
    Close/* 0 - 4, 5-9 */,
    Medium/* 2-4, 5 - 9, 10-15 */,
    Long/* 5 - 15 */,
    EffectiveAtAll/* 0 - 15 */,
    InaccurateAtAll/* 0-15 */,
    ResetGrid
}

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
    ToBeTauntUnused,//
}

public enum AbilityProperties
{
    Basic,//Ignores Silance Effect
    Finisher,//If the Target has less then 50% before the attack, deal double damage
    Heal,//Give a friendly unit health points
    IgnoreArmor,//The attack damage is done directly to a units health
    CDR,//Reduces your ability's current cooldowns
    AreaOfEffect,// Affects all units within a certain radius of the target point, dealing damage or applying other effects to each unit (physics based)
    Teleport,//The unit instantaneously transport itself to a designated location x tiles away, bypassing obstacles and enemy units that may be in the way
}
public class BaseAbility : BaseAction
{
    public static event EventHandler<int> OnAnySpellCast;

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

    public float GetDamage() { return damage; }
    public int GetCritChance() { return critChance; }
    public float GetAbilityHitChance() { return hitChance; }
    public float GetPostureDamage() { return postureDamage; }
    public int GetStatusChance() { return statusEffectChance; }
    public List<StatusEffect> GetStatusEffect() { return EnemyStatusEffects; }

    // targetUnit.Damage(damage* 2, postureDamage, hitChance, critChance, _abilityEffect, statusEffectChance, statusEffectDuration);

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        // cooldown += addCooldown;
        CastSpell();
        if (selfBuffs.Count > 0)
            SetSelfBuff();

        //gridPosition + unit.GetGridPosition() 
        //HandleAbilityRange();
    }
    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 0 };
    }
    public override List<GridPosition> GetValidActionGridPositionList()
    {
        return new List<GridPosition> { GetUnit().GetGridPosition() };
    }

    protected virtual void CastSpell()
    {
        OnAnySpellCast?.Invoke(this, GetFavorCost());
    }
    public virtual void SetSelfBuff()
    {
        UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.AddStatusEffectToUnit(selfBuffs, selfBuffDuration);
    }
}