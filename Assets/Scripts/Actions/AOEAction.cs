using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class AOEAction : BaseAction
{
    [SerializeField] private Transform aOEProjectilePrefab;
    [SerializeField] private int maxThrowDistance = 7;

    private void Update()
    {
        if (!isActive)
            return;
    }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        Transform aOEProjectileTransform = Instantiate(aOEProjectilePrefab, unit.GetWorldPosition(), Quaternion.identity);
        AOEProjectile aOEProjectile = aOEProjectileTransform.GetComponent<AOEProjectile>();
        aOEProjectile.Setup(gridPosition, OnAOEBehaviourComplete);

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
                for (int y = -maxThrowDistance; z <= maxThrowDistance; y++)
                {
                    GridPosition offsetGridPosition = new GridPosition(x, z,y);
                    GridPosition testGridPosition = unitGridPosition + offsetGridPosition;

                    if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If grid valid
                        continue;

                    int testDistance = Mathf.Abs(x) + Mathf.Abs(z);

                    if (testDistance > maxThrowDistance) // shooting range check
                        continue;

                    //if need to visualize shooting range uncomment v
                    //_validGridPositionList.Add(testGridPosition);
                    //continue;

                    _validGridPositionList.Add(testGridPosition);
                }
            }
        }

        return _validGridPositionList;
    }

    public override string GetActionName() { return "AoE"; }

    private void OnAOEBehaviourComplete() { ActionComplete(); }

}