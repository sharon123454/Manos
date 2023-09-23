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

    private bool canShootBullt;

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

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        //error fix
        //error fix
        //if (targetUnit == null && !canOnlyHitEnemy) { return base.GetValidActionGridPositionList(); } //target unit was found null so added check but its missing range
        //implementation in baseAbility- GetValidActionGridPositionList()
        //error fix
        //error fix

        #region Range Obstacle Check
        //if (GetUnit().name != "Amarok")
        //{
        //    Vector3 unitWorldPosition = LevelGrid.Instance.GetWorldPosition(GetUnit().GetGridPosition());

        //    Vector3 shootDir = (targetUnit.GetWorldPosition() - unitWorldPosition).normalized;
        //    float shotDistance = Vector3.Distance(unitWorldPosition, targetUnit.GetWorldPosition());

        //    if (Physics.Raycast(unitWorldPosition + Vector3.up * unitShoulderHeight, shootDir, shotDistance, obstacleLayerMask)) // If blocked by an Obstacle
        //        return null;
        //}
        #endregion

        canShootBullt = true;
        targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);//normal range attack implementation
        base.TakeAction(gridPosition, actionComplete);

        ActionStart(actionComplete);
    }
    //public override void TakeAction(Vector3 mousePosition, Action actionComplete)//aoe implementation
    //{
    //    canShootBullt = true;
    //    targetUnit = null;
    //    base.TakeAction(mousePosition, actionComplete);

    //    ActionStart(actionComplete);
    //}

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)//action value resides here (preference on who to do action on)
    {
        Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(gridPosition);

        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 100 + Mathf.RoundToInt((1 - targetUnit.GetHealthNormalized()) * 100f), };
    }

    private void Shoot(float damage)
    {
        if (IsXPropertyInAction(AbilityProperties.AreaOfEffect))
        {
            foreach (Unit unit in AOEManager.Instance.GetUnitsInRange())
            {
                if (unit.IsEnemy())
                {
                    unit.Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, GetAbilityProperties(), statusEffectChance, statusEffectDuration);
                }
            }
        }
        else
        {
            GetTargetUnit().Damage(damage, postureDamage, hitChance, critChance, EnemyStatusEffects, GetAbilityProperties(), statusEffectChance, statusEffectDuration);
        }
        OnShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = GetTargetUnit(), ShootingUnit = GetUnit() });
        OnAnyShoot?.Invoke(this, new OnSHootEventArgs { TargetUnit = GetTargetUnit(), ShootingUnit = GetUnit() });
    }

}