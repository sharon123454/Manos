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

    [Header("Range")]
    [Tooltip("Relevant for raycasting when this Unit shoots")]
    [SerializeField] private float unitShoulderHeight = 1.7f;
    [Tooltip("Relevant for raycasting when this Unit shoots")]
    [SerializeField] private LayerMask obstacleLayerMask;

    protected override void StartOfActionUpdate()
    {
        if (targetUnit)
        {
            Vector3 aimDir = (targetUnit.GetWorldPosition() - GetUnit().GetWorldPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
        else
        {
            Vector3 aimDir = (actionAimDirection - GetUnit().GetWorldPosition()).normalized;
            transform.forward = Vector3.Lerp(transform.forward, aimDir, Time.deltaTime * rotateToTargetSpeed);
        }
    }

    protected override void OnActionExecution()
    {
        if (targetUnit)
        {
            targetUnit.Damage(damage, postureDamage, hitChance, critChance, GetStatusEffects(), GetAbilityProperties(), statusEffectChance, statusEffectDuration, enemyEffectivess);

            OnShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = targetUnit, ShootingUnit = GetUnit() });
            OnAnyShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = targetUnit, ShootingUnit = GetUnit() });
        }
        else
        {
            foreach (var unit in GetAoETargets())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, GetStatusEffects(), GetAbilityProperties(), statusEffectChance, statusEffectDuration, enemyEffectivess);

                    OnShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = unit, ShootingUnit = GetUnit() });
                    OnAnyShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = unit, ShootingUnit = GetUnit() });
                }
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        if (!IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);
            enemyEffectivess = targetUnit.GetUnitStats().GetEffectiveness;
        }
        else
        {
            SetTargetByAoE(AOEManager.Instance.GetUnitsInRange());
        }

        base.TakeAction(gridPosition, actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)//action value resides here (preference on who to do action on)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f), };
    }

}