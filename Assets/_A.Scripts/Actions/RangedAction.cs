using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class RangedAction : BaseAbility
{
    public static event EventHandler<OnSHootEventArgs> OnAnyShoot;
    public event EventHandler<OnSHootEventArgs> OnShoot;
    public class OnSHootEventArgs : EventArgs
    {
        public Unit TargetUnit;
        public Unit ShootingUnit;
    }

    //[SerializeField] private StatusEffect _skillEffect;
    [Header("Range")]
    [Tooltip("Relevant for raycasting when this Unit shoots")]
    [SerializeField] private float unitShoulderHeight = 1.7f;
    [Tooltip("Relevant for raycasting when this Unit shoots")]
    [SerializeField] private LayerMask obstacleLayerMask;

    private Unit targetUnit;
    private bool canShootBullt;

    protected override void StartOfActionUpdate()
    {
        base.StartOfActionUpdate();
        Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
        transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
    }
    protected override void ExecutionOfActionUpdate()
    {
        base.ExecutionOfActionUpdate();
        if (canShootBullt)
        {
            Shoot(damage);
            canShootBullt = false;
        }
    }
    protected override void EndOfActionUpdate() { base.EndOfActionUpdate(); }

    public Unit GetTargetUnit() { return targetUnit; }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        //error fix
        //error fix
        if (targetUnit == null && !canOnlyHitEnemy) { return base.GetValidActionGridPositionList(); } //target unit was found null so added check but its missing range
                                                                                                      //implementation in baseAbility- GetValidActionGridPositionList()
        //error fix
        //error fix

        #region Range Obstacle Check
        Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(GetUnit().GetGridPosition());

        Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
        float shotDistance = Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition());

        if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, shotDistance, obstacleLayerMask)) // If blocked by an Obstacle
            return null;
        #endregion

        return base.GetValidActionGridPositionList();
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)//action value resides here (preference on who to do action on)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f), };
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        canShootBullt = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
        base.TakeAction(gridPosition, actionComplete);

        ActionStart(actionComplete);
    }

    private void Shoot(float damage)
    {
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
        else
            targetUnit.Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, _AbilityProperties, statusEffectChance, statusEffectDuration);

        OnShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = targetUnit, ShootingUnit = GetUnit() });
        OnAnyShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = targetUnit, ShootingUnit = GetUnit() });
    }

}