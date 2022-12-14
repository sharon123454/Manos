using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MeleeAction : BaseAction
{
    public static event EventHandler OnAnyMeleeHit;

    public event EventHandler OnMeleeActionStarted;
    public event EventHandler OnMeleeActionCompleted;

    [SerializeField] private float damage = 25;
    [SerializeField] private int maxMeleeDistance = 1;
    [SerializeField] private float beforeHitStateTime = 0.7f, afterHitStateTime = 0.5f, rotateToTargetSpeed = 10f;

    private Unit targetUnit;

    private enum State { SwingBeforeHit, SwingAfterHit, }
    private float stateTimer;
    private State state;

    private void Update()
    {
        if (!isActive)
            return;

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.SwingBeforeHit:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
                break;
            case State.SwingAfterHit:
                break;
        }

        if (stateTimer <= 0f)
            NextState();
    }

    private void NextState()
    {
        switch (state)
        {
            case State.SwingBeforeHit:
                state = State.SwingAfterHit;
                stateTimer = afterHitStateTime;

                OnAnyMeleeHit?.Invoke(this, EventArgs.Empty);
                //feed damage through weapon or smtn later on
                targetUnit.Damage(damage);
                break;

            case State.SwingAfterHit:
                OnMeleeActionCompleted?.Invoke(this, EventArgs.Empty);
                ActionComplete();
                break;
        }
    }

    public int GetMaxMeleeDistance() { return maxMeleeDistance; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.SwingBeforeHit;
        stateTimer = beforeHitStateTime;

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

        GridPosition unitGridPosition = unit.GetGridPosition();

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

                if (targetUnit.IsEnemy() == unit.IsEnemy())// Both units on the same team
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    public override string GetActionName() { return "Melee"; }

}