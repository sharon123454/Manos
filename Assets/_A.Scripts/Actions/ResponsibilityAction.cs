using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ResponsibilityAction : BaseHeal
{
    public event EventHandler OnDivineActive;

    private void Update()
    {
        if (!_isActive) { return; }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        OnDivineActive?.Invoke(this, EventArgs.Empty);
        GetUnit().GetUnitStats().Heal(healValue);
        ActionStart(actionComplete);
        GetUnit().GetUnitStats().InvokeHPChange();
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