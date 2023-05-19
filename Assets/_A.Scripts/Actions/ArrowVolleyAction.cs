using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ArrowVolleyAction : BaseAbility
{
    [SerializeField] private Transform aOEProjectilePrefab;
    [SerializeField] private int maxThrowDistance = 7;

    private void Update()
    {
        if (!_isActive)
            return;
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        base.TakeAction(gridPosition, actionComplete);
        Transform aOEProjectileTransform = Instantiate(aOEProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        AOEProjectile aOEProjectile = aOEProjectileTransform.GetComponent<AOEProjectile>();
        aOEProjectile.Setup(gridPosition, OnAOEBehaviourComplete, _abilityEffect,_AbilityProperties, statusEffectChance, statusEffectDuration);

        ActionStart(actionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 0, };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();

        GridPosition unitGridPosition = unit.GetGridPosition();

        for (int x = -maxThrowDistance; x <= maxThrowDistance; x++)
        {
            for (int z = -maxThrowDistance; z <= maxThrowDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If grid valid
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                if (testDistance > maxThrowDistance) // shooting range check
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    public override string GetActionName() { return "Volley"; }

    private void OnAOEBehaviourComplete() { ActionComplete(); }

}