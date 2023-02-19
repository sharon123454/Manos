using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum AbilityRange 
{
    Melee/* 0 - 1 */,
    Close/* 0 - 4, 5-9 */,
    Medium/* 2-4, 5 - 9, 10-15 */,
    Long/* 5 - 15 */,
    EffectiveAtAll/* 0 - 15 */,
    InaccurateAtAll/* 0-15 */ 
}
public enum Effectiveness
{
    Effective/* 0 - 1 */,
    Inaccurate/* 0 - 4, 5-9 */,
    Miss/* 2-4, 5 - 9, 10-15 */,
}

public enum StatusEffect
{
    None,//Default State
    Stun,//Miss X Turns
    IgnoreArmor,//Ignore Armor
    Root,//Cant Move,but can use Abilities 
    CowardPlague,
    Nullify,
    Heal,//Heal X HP
    GainArmor,//Gain Armor
}
public class BaseAbility : BaseAction
{
    public static event EventHandler<int> OnAnySpellCast;

    [SerializeField] protected AbilityRange range;
    [SerializeField] protected bool isSpell = true;
    [Range(1f, 600f)]
    [SerializeField] protected float damage = 10, postureDamage = 0;
    [Range(0,200)]
    [SerializeField] protected int hitChance = 100, critChance, statusEffectChance, statusEffectDuration;

    [SerializeField] protected StatusEffect _abilityEffect;

    //status effect? what is it?

    public float GetDamage() { return damage; }
    public AbilityRange GetRange() { return range; }
    public int GetCritChance() { return critChance; }
    public int GetStatusChance() { return statusEffectChance; }
    public StatusEffect GetStatusEffect() { return _abilityEffect; }
    public float GetAbilityHitChance() { return hitChance; }
    public float GetPostureDamage() { return postureDamage; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
       // cooldown += addCooldown;
        OnAnySpellCast?.Invoke(this, GetFavorCost());
        //gridPosition + unit.GetGridPosition() 
        //HandleAbilityRange();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        throw new NotImplementedException();
    }

    public override string GetActionName() { return "Ability"; }

    protected virtual void CastSpell()
    {
        OnAnySpellCast?.Invoke(this, GetFavorCost());
    }

}