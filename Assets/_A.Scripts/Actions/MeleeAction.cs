using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MeleeAction : BaseAbility
{
    public static event EventHandler OnAnyMeleeHit;
    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;

    protected override void StartOfActionUpdate()
    {
        base.StartOfActionUpdate();

        Vector3 aimDir = (GetTargetUnit().GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
    }
    protected override void ExecutionOfActionUpdate() { base.ExecutionOfActionUpdate(); }
    protected override void EndOfActionUpdate() { base.EndOfActionUpdate(); }

    protected override void OnActionStartChange() { base.OnActionStartChange(); }
    protected override void OnActionExecutionChange()
    {
        base.OnActionExecutionChange();
        OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
        if (IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            foreach (Unit unit in AOEManager.Instance.GetUnitsInRange())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, GetAbilityProperties(), statusEffectChance, statusEffectDuration);
                }
            }
            return;
        }
        else
        {
            GetTargetUnit().Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, GetAbilityProperties(), statusEffectChance, statusEffectDuration);
        }
    }
    protected override void OnActionEndChange()
    {
        base.OnActionEndChange();
        OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);

        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 200, };
    }

}