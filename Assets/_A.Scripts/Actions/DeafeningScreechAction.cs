using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class DeafeningScreechAction : BaseAction
{
    public static event EventHandler OnDivineActive;
    [SerializeField] private int rootDuration;
    [SerializeField] private int postureDamage;
    private void Update()
    {
        if (!_isActive) { return; }

        ActionComplete();
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        foreach (var VARIABLE in UnitManager.Instance.GetFriendlyUnitList())
        {
            VARIABLE.GetUnitStats().getUnitStatusEffects().AddStatusEffectToUnit(StatusEffect.Root, rootDuration);
            VARIABLE.GetUnitStats().RemovePosture(postureDamage);
        }
        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 200, };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition _unitGridPosition = unit.GetGridPosition();
        return new List<GridPosition> { _unitGridPosition };
    }

    public override string GetActionName() { return "DeafeningScreech"; }
}
