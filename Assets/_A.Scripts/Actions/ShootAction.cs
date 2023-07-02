using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ShootAction : BaseAbility
{
    public static event EventHandler<OnSHootEventArgs> OnAnyShoot;

    public event EventHandler<OnSHootEventArgs> OnShoot;

    public class OnSHootEventArgs : EventArgs
    {
        public Unit targetUnit;
        public Unit shootingUnit;
    }

    [Header("Shoot")]
    //[SerializeField] private StatusEffect _skillEffect;
    private int maxShootDistance = 9;
    [SerializeField] private float aimingStateTime = 1f, shootingStateTime = 0.1f, coolOffStateTime = 0.1f, rotateToTargetSpeed = 10f;
    [Tooltip("Relevant for raycasting when this Unit shoots")]
    [SerializeField] private float unitShoulderHeight = 1.7f;
    [SerializeField] private LayerMask obstacleLayerMask;

    private Unit targetUnit;
    private bool canShootBullt;
    private enum State { Aiming, Shooting, Cooloff }
    private float stateTimer;
    private State state;

    void Update()
    {
        if (!_isActive)
            return;
        stateTimer -= Time.deltaTime;

        switch (state)
        {
            case State.Aiming:
                Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
                transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
                break;
            case State.Shooting:
                if (canShootBullt)
                {
                    Shoot(damage);
                    canShootBullt = false;
                }
                break;
            case State.Cooloff:
                break;
        }

        if (stateTimer <= 0f)
            NextState();
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

    public Unit GetTargetUnit() { return targetUnit; }

    public int GetMaxShootDistance() { return maxShootDistance; }

    public int GetTargetCountAtPosition(GridPosition gridPosition)
    {
        return GetValidActionGridPositionList(gridPosition).Count;
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        GridPosition _unitGridPosition = GetUnit().GetGridPosition();
        return GetValidActionGridPositionList(_unitGridPosition);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f), };
    }//action value resides here (preference on who to do action on)

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);

        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        state = State.Aiming;
        stateTimer = aimingStateTime;
        canShootBullt = true;

        ActionStart(actionComplete);
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

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If grid valid
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxShootDistance) // shooting range check
                    continue;

                //if need to visualize shooting range uncomment v
                //_validGridPositionList.Add(testGridPosition);
                //continue;

                if (!LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If grid position has no unit
                    continue;

                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(testGridPosition);

                if (targetUnit.IsEnemy() == GetUnit().IsEnemy() && !_AbilityProperties.Contains(AbilityProperties.Heal))// Both units on the same team
                    continue;

                Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(unitGridPosition);
                Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
                float shotDistance = Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition());

                if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, shotDistance, obstacleLayerMask)) // If blocked by an Obstacle
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    private void Shoot(float damage)
    {
        OnShoot?.Invoke(this, new OnSHootEventArgs { targetUnit = targetUnit, shootingUnit = GetUnit() });
        OnAnyShoot?.Invoke(this, new OnSHootEventArgs { targetUnit = targetUnit, shootingUnit = GetUnit() });

        if (_AbilityProperties.Contains(AbilityProperties.AreaOfEffect))
        {
            foreach (var unit in AOEManager.Instance.GetUnitsInRange())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, _statusEffect, _AbilityProperties, statusEffectChance, statusEffectDuration);
                }
            }
            return;
        }
        else
            targetUnit.Damage(damage, postureDamage, hitChance, critChance, _statusEffect, _AbilityProperties, statusEffectChance, statusEffectDuration);
    }

}