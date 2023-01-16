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
public class BaseAbility : BaseAction
{
    [SerializeField] protected AbilityRange range;
    [SerializeField] protected bool isSpell = true;
    [SerializeField] protected float damage, postureDamage;
    [Range(0,100)]
    [SerializeField] protected int hitChance, critChance, statusEffectChance;
    //status effect? what is it?

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
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

}