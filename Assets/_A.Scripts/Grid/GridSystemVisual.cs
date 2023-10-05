using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [SerializeField] private Transform GridSystemVisualSinglePrefab;
    [SerializeField] private Transform GridParent;
    [SerializeField] private int _Adjacent = 1;
    [SerializeField] private int _Close = 4;
    [SerializeField] private int _Far = 6;
    [SerializeField] private int _VeryFar = 9;

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(), LevelGrid.Instance.GetLength()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                GridPosition gridPosition = new GridPosition(x, z);

                RaycastHit ray;
                Vector3 myWorldPos = LevelGrid.Instance.GetWorldPosition(gridPosition);

                if (Physics.Raycast(myWorldPos + new Vector3(transform.position.x, 0, transform.position.z) + Vector3.up * 10, Vector3.down, out ray, 2000, PathFinding.Instance.obstacleLayerMask)) { /*ignore if grid has obstecle*/ }
                else if (Physics.Raycast(myWorldPos + new Vector3(transform.position.x, 0, transform.position.z) + Vector3.up * 10, Vector3.down, out ray, 20000, PathFinding.Instance.floorGridLayer))
                {
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(GridSystemVisualSinglePrefab, new Vector3(myWorldPos.x, ray.point.y, myWorldPos.z), Quaternion.identity, GridParent);

                    gridSystemVisualSingleArray[x, z] = gridSystemVisualSingleTransform.GetComponent<GridSystemVisualSingle>();
                }
            }
        }

        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        LevelGrid.Instance.OnAnyUnitMovedGridPosition += LevelGrid_OnAnyUnitMovedGridPosition;
    }

    private void UpdateGridVisual()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
        HideAllVisual();

        if (selectedAction == null) { return; }
        if (selectedAction.GetCurrentCooldown() > 0) { return; }

        FilterByRange(selectedAction.GetRange(), selectedUnit);
    }
    private void FilterByRange(ActionRange AbilityRange, Unit selectedUnit)
    {
        switch (AbilityRange)
        {
            case ActionRange.Move:
                if (UnitActionSystem.Instance.GetSelectedAction() is MoveAction)
                {
                    //was used for getting move distance as range, but is unused for now as we just color
                    //MoveAction _move = UnitActionSystem.Instance.GetSelectedMoveAction();
                    MoveRange(selectedUnit/*, _move.GetMoveValue()*/);
                }
                break;
            case ActionRange.Self:
                SelfRange(selectedUnit);
                break;
            case ActionRange.Melee:
                MeleeRange(selectedUnit, _Adjacent, _VeryFar);
                break;
            case ActionRange.Close:
                CloseRange(selectedUnit, _Close, _Far);
                break;
            case ActionRange.Medium:
                MediumRange(selectedUnit, _Adjacent, _Close, _Far, _VeryFar);
                break;
            case ActionRange.Long:
                LongRange(selectedUnit, _Close, _VeryFar);
                break;
            case ActionRange.EffectiveAtAll:
                EffectiveAtAllRanges(selectedUnit, _VeryFar);
                break;
            case ActionRange.InaccurateAtAll:
                InaccurateAtAllRanges(selectedUnit, _VeryFar);
                break;
            case ActionRange.ResetGrid:
                HideAllVisual();
                break;
            default:
                Debug.Log("Case not Implamented");
                break;
        }
    }

    //Filtering
    private void ShowGridPositionRange(GridPosition gridPosition, int range, Color color, Effectiveness effectiveness)
    {
        List<GridPosition> gridPosList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z); // range check

                if (testDistance > range)
                    continue;

                gridPosList.Add(testGridPosition);
            }
        }

        #region deprecated
        //if (gridPosList.Count > 0)
        //    if (UnitActionSystem.Instance.GetSelectedAction() is MoveAction)
        //    {
        //        foreach (GridPosition position in gridPosList)
        //            if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //            {
        //                gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(adjacentParent, true);
        //            }
        //    }
        //
        //else
        //{
        //    switch (range)//parenting for outline purposes
        //    {
        //        case 1://_adjacent
        //            foreach (GridPosition position in gridPosList)
        //                if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //                {
        //                    gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                    //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(adjacentParent, true);
        //                }
        //            break;
        //        case 4://_close
        //            foreach (GridPosition position in gridPosList)
        //                if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //                {
        //                    gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                    //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(closeParent, true);
        //                }
        //            break;
        //        case 6://_far
        //            foreach (GridPosition position in gridPosList)
        //                if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //                {
        //                    gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                    //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(farParent, true);
        //                }
        //            break;
        //        case 9://_veryFar
        //            foreach (GridPosition position in gridPosList)
        //                if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //                {
        //                    gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                    //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(veryFarParent, true);
        //                }
        //            break;
        //        default:
        //            break;
        //    }
        //}
        #endregion

        ShowGridPositionList(gridPosList, color, effectiveness);
    }
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, Color color, Effectiveness effectiveness)
    {
        List<GridPosition> gridPosList = new List<GridPosition>();

        for (int x = -range; x <= range; x++)
        {
            for (int z = -range; z <= range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);

                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                gridPosList.Add(testGridPosition);
            }
        }

        #region deprecated
        //switch (range)
        //{
        //    case 1://_adjacent
        //        foreach (var position in gridPosList)
        //            if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //            {
        //                gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(adjacentParent, true);
        //            }
        //        break;
        //    case 4://_close
        //        foreach (var position in gridPosList)
        //            if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //            {
        //                gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(closeParent, true);
        //            }
        //        break;
        //    case 6://_far
        //        foreach (var position in gridPosList)
        //            if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //            {
        //                gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(farParent, true);
        //            }
        //        break;
        //    case 9://_veryFar
        //        foreach (var position in gridPosList)
        //            if (gridSystemVisualSingleArray[position._x, position._z] != null)
        //            {
        //                gridSystemVisualSingleArray[position._x, position._z].UpdateGridVisualSingle(color);
        //                //gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(veryFarParent, true);
        //            }
        //        break;
        //    default:
        //        break;
        //}
        #endregion

        ShowGridPositionList(gridPosList, color, effectiveness);
    }

    //Execution
    private void ShowGridPositionList(List<GridPosition> gridPositions, Color gridVisualColor, Effectiveness effectiveness)
    {
        foreach (GridPosition position in gridPositions)
            if (gridSystemVisualSingleArray[position._x, position._z] != null)
            {
                if (LevelGrid.Instance.GetUnitAtGridPosition(position) != null)
                {
                    LevelGrid.Instance.GetUnitAtGridPosition(position).SetGridEffectiveness(effectiveness);
                }
                gridSystemVisualSingleArray[position._x, position._z].Show(gridVisualColor);
            }
    }
    private void HideGridPositionRange(GridPosition gridPosition, int range)
    {
        List<GridPosition> gridPosList = new List<GridPosition>();

        for (int x = -range; x < range; x++)
        {
            for (int z = -range; z < range; z++)
            {
                GridPosition testGridPosition = gridPosition + new GridPosition(x, z);
                if (!LevelGrid.Instance.IsValidGridPosition(testGridPosition))
                    continue;

                int testDistance = Mathf.Abs(x) + Mathf.Abs(z); // range check

                if (testDistance > range)
                    continue;

                gridPosList.Add(testGridPosition);
            }
        }

        HideGridPositionList(gridPosList);
    }
    private void HideGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition position in gridPositions)
            if (gridSystemVisualSingleArray[position._x, position._z] != null)
            {
                if (LevelGrid.Instance.GetUnitAtGridPosition(position) != null)
                {
                    LevelGrid.Instance.GetUnitAtGridPosition(position).SetGridEffectiveness(Effectiveness.Miss);
                }
                gridSystemVisualSingleArray[position._x, position._z].Hide();
            }
    }
    private void HideAllVisual()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
                if (gridSystemVisualSingleArray[x, z] != null)
                {
                    if (LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(gridSystemVisualSingleArray[x, z].transform.position)) != null)
                    {
                        LevelGrid.Instance.GetUnitAtGridPosition(LevelGrid.Instance.GetGridPosition(gridSystemVisualSingleArray[x, z].transform.position)).SetGridEffectiveness(Effectiveness.Miss);
                    }
                    gridSystemVisualSingleArray[x, z].Hide();
                }
    }

    #region  Filter By Range methods
    private void MoveRange(Unit selectedUnit/*, int playerMovement*/)
    {
        HideAllVisual();
    }
    private void SelfRange(Unit selectedUnit)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), 0, Color.cyan, Effectiveness.Effective);
    }
    private void MeleeRange(Unit selectedUnit, int adjacent, int veryFar)
    {
        HideAllVisual();
        HideGridPositionRange(selectedUnit.GetGridPosition(), veryFar);
        ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), adjacent, Color.green, Effectiveness.Effective);
    }
    private void CloseRange(Unit selectedUnit, int close, int far)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, Color.yellow, Effectiveness.Inaccurate);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, Color.green, Effectiveness.Effective);
    }
    private void MediumRange(Unit selectedUnit, int adjacent, int close, int far, int veryFar)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, Color.yellow, Effectiveness.Inaccurate);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, Color.green, Effectiveness.Effective);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, Color.yellow, Effectiveness.Inaccurate);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), adjacent, Color.red, Effectiveness.Miss);
        HideGridPositionRange(selectedUnit.GetGridPosition(), adjacent);
    }
    private void LongRange(Unit selectedUnit, int close, int veryFar)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, Color.green, Effectiveness.Effective);
        HideGridPositionRange(selectedUnit.GetGridPosition(), close);
    }
    private void EffectiveAtAllRanges(Unit selectedUnit, int veryFar)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, Color.green, Effectiveness.Effective);
    }
    private void InaccurateAtAllRanges(Unit selectedUnit, int veryFar)
    {
        HideAllVisual();
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, Color.yellow, Effectiveness.Inaccurate);
    }
    #endregion

    //Events
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) { UpdateGridVisual(); }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) { UpdateGridVisual(); }

}