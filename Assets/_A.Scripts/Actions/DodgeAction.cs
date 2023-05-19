using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class DodgeAction : BaseAction
{
    public event EventHandler OnDodgeAction;

    private void Update()
    {
        if (!_isActive) { return; }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        unit.Dodge();
        OnDodgeAction?.Invoke(this, EventArgs.Empty);
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

    public override string GetActionName(){ return "Dodge"; }

}