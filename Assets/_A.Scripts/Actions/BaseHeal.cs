using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class BaseHeal : BaseAbility
{
    [Range(1f, 600f)] [SerializeField] protected float healValue = 10;

    public float GetHealValue() { return healValue; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        throw new NotImplementedException();
    }

}