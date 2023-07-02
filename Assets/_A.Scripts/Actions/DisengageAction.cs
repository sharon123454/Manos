using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class DisengageAction : BaseAction
{
    public event EventHandler OnDisengageActivate;

    private void Update()
    {
        if (!_isActive) { return; }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        GetUnit().Disengage();
        OnDisengageActivate?.Invoke(this, EventArgs.Empty);
        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 0, };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition _unitGridPosition = GetUnit().GetGridPosition();
        return new List<GridPosition> { _unitGridPosition };
    }

}