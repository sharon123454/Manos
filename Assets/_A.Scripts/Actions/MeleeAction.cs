using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MeleeAction : BaseAbility
{
    public static event EventHandler OnAnyMeleeHit;

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
            targetUnit.Damage(damage, postureDamage, hitChance, critChance, GetStatusEffects(), GetAbilityProperties(), statusEffectChance, statusEffectDuration, enemyEffectivess);
        }
        else
        {
            foreach (var unit in GetAoETargets())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, GetStatusEffects(), GetAbilityProperties(), statusEffectChance, statusEffectDuration, enemyEffectivess);
                }
            }
        }

        OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
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