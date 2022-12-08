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

    [SerializeField] private int maxShootDistance = 7;
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
                    Shoot();
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

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        ActionStart(onActionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = aimingStateTime;
        canShootBullt = true;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();
        GridPosition _unitGridPosition = unit.GetGridPosition();

        for (int x = -maxShootDistance; x <= maxShootDistance; x++)
        {
            for (int z = -maxShootDistance; z <= maxShootDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = _unitGridPosition + offsetGridPosition;

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

    private void Shoot()
    {
        OnShoot?.Invoke(this, new OnSHootEventArgs { targetUnit = targetUnit, shootingUnit = unit });
        targetUnit.Damage();
    }

}