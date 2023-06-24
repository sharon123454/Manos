using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MoveAction : BaseAction
{
    public event EventHandler OnStartMoving;
    public event EventHandler OnStopMoving;

    [SerializeField] private int maxMoveDistance = 3;
    [SerializeField] private float visualRotationSpeed = 7.5f;
    [SerializeField] private int visualMovementSpeed = 4;

    private int pathfindingDistanceMultiplier = 10;
    private float stoppingDistance = .1f;
    private List<Vector3> positionList;
    private int currentPositionIndex;
    public bool isTeleport = false;

    private void Update()
    {
        if (isTeleport)
        {
            if (!_isActive) { return; }

            Vector3 targetPosition = positionList[^1];
            // Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.position = targetPosition;

            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                //if (unit.unitStatusEffects.ContainsEffect(StatusEffect.Haste))
                //    transform.position += moveDirection * (moveSpeed * 2) * Time.deltaTime;
                //else
                //    transform.position += moveDirection * moveSpeed * Time.deltaTime;
            }
            else
            {
                currentPositionIndex++;

                if (currentPositionIndex >= positionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
            }
        }
        else 
        {
            if (!_isActive) { return; }

            Vector3 targetPosition = positionList[currentPositionIndex];
            Vector3 moveDirection = (targetPosition - transform.position).normalized;

            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * visualRotationSpeed);

            if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
            {
                if (GetUnit().unitStatusEffects.ContainsEffect(StatusEffect.Haste))
                    transform.position += moveDirection * (visualMovementSpeed * 2) * Time.deltaTime;
                else
                    transform.position += moveDirection * visualMovementSpeed * Time.deltaTime;
            }
            else
            {
                currentPositionIndex++;

                if (currentPositionIndex >= positionList.Count)
                {
                    OnStopMoving?.Invoke(this, EventArgs.Empty);
                    ActionComplete();
                }
            }
        }
        
    }

    public override void TakeAction(GridPosition gridPosition, Action onActionComplete)
    {
        //find path to action grid position
        List<GridPosition> pathGridPositionList = PathFinding.Instance.FindPath(GetUnit().GetGridPosition(), gridPosition, out int pathLength);

        //reset current position index, and new position list
        currentPositionIndex = 0;
        positionList = new List<Vector3>();

        //translate path into world positions and add to unit positions to move
        foreach (GridPosition pathGridPosition in pathGridPositionList)
        {
            RaycastHit ray;
            Vector3 myWorldPos = LevelGrid.Instance.GetWorldPosition(pathGridPosition);

            if (Physics.Raycast(myWorldPos + Vector3.up * 5, Vector3.down, out ray, 2000,
                    PathFinding.Instance.floorGridLayer))
            {
                positionList.Add(new Vector3(myWorldPos.x, ray.point.y, myWorldPos.z));
            }
        }

        OnStartMoving?.Invoke(this, EventArgs.Empty);

        ActionStart(onActionComplete);
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        //  int targetCountAtGridPosition = unit.GetAction<ShootAction>().GetTargetCountAtPosition(gridPosition);
        return new EnemyAIAction { gridPosition = gridPosition, actionValue = 25 };
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();
        GridPosition _unitGridPosition = GetUnit().GetGridPosition();

        for (int x = -maxMoveDistance; x <= maxMoveDistance; x++)
        {
            for (int z = -maxMoveDistance; z <= maxMoveDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = _unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If off grid positions
                    continue;

                if (_unitGridPosition == testGridPosition) // If my position
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If grid position Occupied w another unit
                    continue;

                if (!PathFinding.Instance.IsWalkableGridPosition(testGridPosition)) // If grid position has GO with "Obstacle" tag
                    continue;

                if (!PathFinding.Instance.HasPath(_unitGridPosition, testGridPosition)) // If grid position is Unreachable
                    continue;

                if (PathFinding.Instance.GetPathLength(_unitGridPosition, testGridPosition) > maxMoveDistance * pathfindingDistanceMultiplier) // If path length is too Long (blocking other side of wall grid positions)
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }

    public int GetMoveValue() { return maxMoveDistance; }

}