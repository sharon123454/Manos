using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseAbility : BaseAction
{
    //damage, hit chance etc

    public override string GetActionName()
    {
        throw new NotImplementedException();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        throw new NotImplementedException();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        throw new NotImplementedException();
    }
}