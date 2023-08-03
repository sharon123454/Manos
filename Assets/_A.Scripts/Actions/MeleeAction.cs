using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MeleeAction : BaseAbility
{
    public static event EventHandler OnAnyMeleeHit;
    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;

    [Header("Melee")]
    [SerializeField] private int maxMeleeDistance = 1;

    private Unit targetUnit;

    protected override void StartOfActionUpdate()
    {
        base.StartOfActionUpdate();

        Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
    }
    protected override void ExecutionOfActionUpdate() { base.ExecutionOfActionUpdate(); }
    protected override void EndOfActionUpdate() { base.EndOfActionUpdate(); }

    protected override void OnActionStartChange() { base.OnActionStartChange(); }
    protected override void OnActionExecutionChange()
    {
        base.OnActionExecutionChange();
        OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
        if (_AbilityProperties.Contains(AbilityProperties.AreaOfEffect))
        {
            foreach (var unit in AOEManager.Instance.GetUnitsInRange())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, _AbilityProperties, statusEffectChance, statusEffectDuration);
                }
            }
            return;
        }
        targetUnit.Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, _AbilityProperties, statusEffectChance, statusEffectDuration);
    }
    protected override void OnActionEndChange()
    {
        base.OnActionEndChange();
        OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public int GetMaxMeleeDistance() { return maxMeleeDistance; }

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

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = GetUnit().GetGridPosition();

        for (int x = -maxMeleeDistance; x <= maxMeleeDistance; x++)
        {
            for (int z = -maxMeleeDistance; z <= maxMeleeDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If grid valid
                    continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If grid position has no unit
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (GetUnit().IsEnemy() && targetUnit.GetUnitStats().getUnitStatusEffects().unitActiveStatusEffects.Contains(StatusEffect.Taunt))
                {
                    _validGridPositionList.Clear();
                    _validGridPositionList.Add(testGridPosition);
                    break;
                }
                if (targetUnit.IsEnemy() == GetUnit().IsEnemy())// Both units on the same team
                    continue;

                if (!targetUnit.IsEnemy() && targetUnit.unitStatusEffects.unitActiveStatusEffects.Contains(StatusEffect.Invisibility))
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

}