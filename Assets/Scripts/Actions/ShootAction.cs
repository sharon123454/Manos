using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ShootAction : BaseAction
{
    public event EventHandler<OnSHootEventArgs> OnShoot;
    public class OnSHootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [SerializeField] private int maxShootDistance = 5;
    [SerializeField] private float aimingStateTime = 1f, shootingStateTime = 0.1f, coolOffStateTime = 0.1f, rotateToTargetSpeed = 10f;

    private enum State { Aiming, Shooting, Cooloff }
    private bool canShootBullt;
    private float stateTimer;
    private Unit targetUnit;
    private State state;

    void Update()
    {
        if (!isActive) { return; }

        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - unit.GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
                break;
            case State.Shooting:
                if (canShootBullt)
                {
                    //feed damage through weapon or smtn later on
                    Shoot(1);
                    canShootBullt = false;
                }
                break;
            case State.Cooloff:
                break;
            default:
                break;
        }

        if (stateTimer <= 0f)
            NextState();
    }

    public Unit GetTargetUnit() { return targetUnit; }

    public int GetMaxShootDistance() { return maxShootDistance; }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition _unitGridPosition = unit.GetGridPosition();
        return GetValidActionGridPositionList(_unitGridPosition);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f), };
    }//action value resides here (preference on who to do action on)

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = aimingStateTime;
        canShootBullt = true;

        ActionStart(onActionComplete);
    }

    public List<GridPosition> GetValidActionGridPositionList(GridPosition unitGridPosition)
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z); // range check

                if (testDistance > maxShootDistance)
                    continue;

                //if need to visualize shooting range uncomment v
                //_validGridPositionList.Add(testGridPosition);
                //continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If Grid Position Empty, no unit
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == unit.IsEnemy())// Both units on the same team
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    public override string GetActionName() { return "Shoot"; }

    private void Shoot(float damage)
    {
        OnShoot?.Invoke(this, new OnSHootEventArgs { targetUnit = targetUnit, shootingUnit = unit });
        targetUnit.Damage(damage);
    }

    private void NextState()
    {
        switch (state)
        {
            case State.Aiming:
                state = State.Shooting;
                stateTimer = shootingStateTime;
                break;
            case State.Shooting:
                state = State.Cooloff;
                stateTimer = coolOffStateTime;
                break;
            case State.Cooloff:
                ActionComplete();
                break;
        }
    }

}