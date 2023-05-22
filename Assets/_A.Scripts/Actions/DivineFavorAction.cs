using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class DivineFavorAction : BaseAction
{
    public static event EventHandler OnDivineActive;
    private void Update()
    {
        if (!_isActive) { return; }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        //unit.GetUnitStats().ReduceStatusEffectCooldowns();
        OnDivineActive.Invoke(this, null);
        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 0, };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition _unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { _unitGridPosition };
    }

}