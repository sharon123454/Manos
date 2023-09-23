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
    [SerializeField] private float healValue;

    public float GetHealValue() { return healValue; }

    protected override void StartOfActionUpdate()
    {
        base.StartOfActionUpdate();
        if (GetTargetUnit())
        {
            Vector3 aimDir = (GetTargetUnit().GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
        else
        {
            //Vector3 aimDir = (targetDirection - GetUnit().GetWorldPosition()).normalized;
            //transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
    }
    protected override void ExecutionOfActionUpdate() { base.ExecutionOfActionUpdate(); }
    protected override void EndOfActionUpdate() { base.EndOfActionUpdate(); }

    protected override void OnActionStartChange() { base.OnActionStartChange(); }
    protected override void OnActionExecutionChange()
    {
        base.OnActionExecutionChange();

        if (IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            foreach (Unit unit in AOEManager.Instance.GetUnitsInRange())
            {
                unit.Heal(GetHealValue());
            }
        }
        else
        {
            GetTargetUnit().Heal(GetHealValue());
        }

        OnAnyHealHit?.Invoke(this, EventArgs.Empty);
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

        //targetUnit.Heal(GetHealValue());
        ActionStart(actionComplete);
    }
    //public override void TakeAction(Vector3 mousePosition, Action actionComplete)
    //{
    //    targetUnit = null;
    //    base.TakeAction(mousePosition, actionComplete);

    //    ActionStart(actionComplete);
    //}

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 200, };
    }

}