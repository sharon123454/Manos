using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class SpinAction : BaseAction
{
    private float totalSpinAmount;

    void Update()
    {
        if (!_isActive) { return; }

        float spinAddAmount = 5 + Time.deltaTime;
        transform.eulerAngles += new Vector3(0, spinAddAmount, 0);

        totalSpinAmount += spinAddAmount;
        if (totalSpinAmount >= 360)
            ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        totalSpinAmount = 0;

        ActionStart(onActionComplete);
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

    public override string GetActionName() { return "Spin"; }

}