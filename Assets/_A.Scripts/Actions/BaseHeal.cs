using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BaseHeal : BaseAbility
{
    [Header("Heal")]
    // [Range(1f, 600f)]
    [SerializeField] private float healValue;

    public float GetHealValue() { return healValue; }

    protected override void StartOfActionUpdate()
    {
        if (targetUnit)
        {
            Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
        else
        {
            Vector3 aimDir = (actionAimDirection - GetUnit().GetWorldPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
    }

    protected override void OnActionExecution()
    {
        if (targetUnit)
        {
            targetUnit.Heal(GetHealValue(), GetStatusEffects(), statusEffectDuration);
        }
        else
        {
            foreach (var unit in GetAoETargets())
            {
                if (!unit.IsEnemy())
                {
                    unit.Heal(GetHealValue(), GetStatusEffects(), statusEffectDuration);
                }
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        if (!IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            enemyEffectivess = targetUnit.GetUnitStats().GetEffectiveness;
        }
        else
        {
            SetTargetByAoE(AOEManager.Instance.GetUnitsInRange());
        }

        base.TakeAction(gridPosition, actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 200, };
    }

}