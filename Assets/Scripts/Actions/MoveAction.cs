using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private float moveSpeed = 4, rotateSpeed = 7.5f;
    [SerializeField] private int maxMoveDistance = 3;

    private float stoppingDistance = .1f;
    private List<Vector3> positionList;
    private int currentPositionIndex;

    private void Update()
    {
        if (!isActive) { return; }

        Vector3 targetPosition = positionList[currentPositionIndex];
        Vector3 moveDirection = (targetPosition - transform.position).normalized;

        transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);

        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
        }
        else
        {
            currentPositionIndex++;

            if (currentPositionIndex >= positionList.Count)
            {
                OnStopMoving.Invoke(this, EventArgs.Empty);
                ActionComplete();
            }
        }
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        //find path to action grid position
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(unit.GetGridPosition(), gridPosition);

        //reset current position index, and new position list
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        //translate path into world positions and add to unit positions to move
        foreach (GridPosition pathGridPosition in pathGridPositionList)
            positionList.Add(LevelGrid.Instance.GetWorldPosition(pathGridPosition));

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = targetCountAtGridPosition * 10 };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();
        GridPosition _unitGridPosition = unit.GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = _unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If Off Grid positions
                    continue;

                if (_unitGridPosition == testGridPosition) // If My Position
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If Grid Position Occupied w another unit
                    continue;

                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) // If Grid Position has GO with "Obstacle" Tag
                    continue;

                if (PathFinding.Instance.HasPath(_unitGridPosition, testGridPosition)) // If Grid Position is Unreachable
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    public override string GetActionName() { return "Move"; }

}