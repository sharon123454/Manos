using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BlockAction : BaseAction
{
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

    public override string GetActionName(){ return "Block"; }

}