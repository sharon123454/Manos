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
    [SerializeField] protected float damage = 10, postureDamage = 0;
    [Range(0,100)]
    [SerializeField] protected int hitChance = 100, critChance, statusEffectChance;
    //status effect? what is it?

    public float GetDamage() { return damage; }
    public float GetPostureDamage() { return postureDamage; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        throw new NotImplementedException();

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

    //private void HandleAbilityRange()
    //{
    //    switch (range)
    //    {
    //        case AbilityRange.Melee:

    //            break;
    //        case AbilityRange.Close:

    //            break;
    //        case AbilityRange.Medium:

    //            break;
    //        case AbilityRange.Long:

    //            break;
    //        case AbilityRange.EffectiveAtAll:

    //            break;
    //        case AbilityRange.InaccurateAtAll:

    //            break;
    //        default:
    //            break;
    //    }
    //}

}