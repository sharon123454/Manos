using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BaseHeal : BaseAbility
{
    public static event EventHandler OnAnyHealHit;

    public event EventHandler OnHealActionStarted;
    public event EventHandler OnHealActionCompleted;

    [Header("Heal")]
    [Range(1f, 600f)]
    [SerializeField] private float healValue = 10;

    private Unit targetUnit;
    public float GetHealValue() { return healValue; }

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

        OnAnyHealHit?.Invoke(this, EventArgs.Empty);
        if (_AbilityProperties.Contains(AbilityProperties.AreaOfEffect))
        {
            foreach (var unit in AOEManager.Instance.GetUnitsInRange())
            {
                if (!unit.IsEnemy())
                {
                    unit.Heal(GetHealValue());
                    print(unit.name + "was healed");
                }
            }
            return;
        }
        targetUnit.Heal(GetHealValue());
    }
    protected override void OnActionEndChange()
    {
        base.OnActionEndChange();
        OnHealActionCompleted?.Invoke(this, EventArgs.Empty);
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        targetUnit.Heal(GetHealValue());
        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 200, };
    }

}