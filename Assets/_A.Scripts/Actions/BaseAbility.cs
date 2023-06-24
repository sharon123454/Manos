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
    ArmorBrake,//Ignore Armor
    Root,//Cant Move,but can use Abilities 
    CowardPlague,//If the affected enemy unit exits status inflicter's melee range they receive the ability's damage and posture damage again
    Nullify,
    Heal,//Heal X HP
    GainArmor,//Gain Armor
    Haste,//Double the move Speed
    Blind,//Halves the unit's chance to hit attacks  ((Affect on posture broken units?))
    Undying,//Affected unit's hp Cant go below 1 HP for x turns
    Regeneration,//Affected unit regains a set amount of HP at the end of the unit's turn
    Corruption,//Affected unit suffers damage at the beginning of their turn
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

    [SerializeField] protected bool isSpell = true;
    [Range(1f, 600f)]
    [SerializeField] protected float damage = 10, postureDamage = 0;
    [Range(0, 200)]
    [SerializeField] protected int hitChance = 100, critChance = 0;
    [SerializeField] protected StatusEffect _statusEffect;
    [Range(0, 200)]
    [SerializeField] protected int statusEffectChance = 0, statusEffectDuration = 1;

    public float GetDamage() { return damage; }
    public int GetCritChance() { return critChance; }
    public float GetAbilityHitChance() { return hitChance; }
    public float GetPostureDamage() { return postureDamage; }
    public int GetStatusChance() { return statusEffectChance; }
    public StatusEffect GetStatusEffect() { return _statusEffect; }

    // targetUnit.Damage(damage* 2, postureDamage, hitChance, critChance, _abilityEffect, statusEffectChance, statusEffectDuration);

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        // cooldown += addCooldown;
        CastSpell();
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

}