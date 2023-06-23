using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class BaseHeal : BaseAbility
{

    public static event EventHandler OnAnyMeleeHit;

    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;
    [Range(1f, 600f)][SerializeField] protected float healValue = 10;

    [SerializeField] private int maxMeleeDistance = 1;
    [SerializeField] private float beforeHitStateTime = 0.7f, afterHitStateTime = 0.5f, rotateToTargetSpeed = 10f;
    private Unit targetUnit;

    private enum State { RotateToHeal, HealComplete, }
    private float stateTimer;
    private State state;
    public float GetHealValue() { return healValue; }
    private void Update()
    {
        if (!_isActive)
            return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.RotateToHeal:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
                break;
            case State.HealComplete:
                break;
        }

        if (stateTimer <= 0f)
            NextState();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.RotateToHeal:
                state = State.HealComplete;
                stateTimer = afterHitStateTime;

                OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
                if (_AbilityProperties.Contains(AbilityProperties.AreaOfEffect))
                {
                    foreach (var unit in AOEManager.Instance.DetectAttack())
                    {
                        if (!unit.IsEnemy())
                        {
                            unit.Heal(healValue);
                            print(unit.name);
                        }
                    }
                    return;
                }
                targetUnit.Heal(healValue);
                break;

            case State.HealComplete:
                OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.RotateToHeal;
        stateTimer = beforeHitStateTime;
        targetUnit.Heal(healValue);
        OnMeleeActionStarted?.Invoke(this, EventArgs.Empty);
        targetUnit.GetUnitStats().InvokeHPChange();
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


                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

}