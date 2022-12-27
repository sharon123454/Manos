using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class DashAction : MoveAction
{
    [SerializeField] private int maxDashDistance = 10;

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        List<GridPosition> _validGridPositionList = new List<GridPosition>();
        GridPosition _unitGridPosition = unit.GetGridPosition();

        for (int x = -maxDashDistance; x <= maxDashDistance; x++)
        {
            for (int z = -maxDashDistance; z <= maxDashDistance; z++)
            {
                GridPosition offsetGridPosition = new GridPosition(x, z);
                GridPosition testGridPosition = _unitGridPosition + offsetGridPosition;

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition)) // If Off Grid positions
                    continue;

                if (_unitGridPosition == testGridPosition) // If My Position
                    continue;

                if (LevelGrid.Instance.HasAnyUnitOnGridPosition(testGridPosition)) // If Grid Position Occupied w another unit
                    continue;

                _validGridPositionList.Add(testGridPosition);
            }
        }

        return _validGridPositionList;
    }
}