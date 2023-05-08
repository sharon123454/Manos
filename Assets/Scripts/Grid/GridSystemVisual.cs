using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UIElements;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterial
    {
        public GridVisualType gridVidualType;
        public Material material;

    }

    public enum GridVisualType { White, Red, RedSoft, Blue, Green, Yellow, Transparent }

    [SerializeField] private Transform GridSystemVisualSinglePrefab;
    [SerializeField] private Transform GridParent, adjacentParent, closeParent, farParent, veryFarParent;
    [SerializeField] private List<GridVisualTypeMaterial> gridVisualTypeMaterialList;
    [SerializeField] private int _adjacent = 1;
    [SerializeField] private int _close = 4;
    [SerializeField] private int _far = 6;
    [SerializeField] private int _veryFar = 9;

    [SerializeField] private Color _adjacentColor = Color.white;
    [SerializeField] private Color _closeColor = Color.white;
    [SerializeField] private Color _farColor = Color.white;
    [SerializeField] private Color _veryFarColor = Color.white;

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

                if (Physics.Raycast(myWorldPos + new Vector3(transform.position.x, 0, transform.position.z) + Vector3.up * 10, Vector3.down, out ray, 2000, PathFinding.Instance.obstacleLayerMask)) { }

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
        if (selectedAction.GetCooldown() > 0)
            return;
        else
            HideAllGridPosition();

        GridVisualType gridVisualType;

        switch (selectedAction.ReturnRange())
        {
            case AbilityRange.Move:
                gridVisualType = GridVisualType.White;
                FilterByRange(AbilityRange.ResetGrid, selectedUnit);
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case AbilityRange.Melee:
                FilterByRange(AbilityRange.Melee, selectedUnit);
                break;
            case AbilityRange.Close:
                FilterByRange(AbilityRange.Close, selectedUnit);
                break;
            case AbilityRange.Medium:
                FilterByRange(AbilityRange.Medium, selectedUnit);
                break;
            case AbilityRange.Long:
                FilterByRange(AbilityRange.Long, selectedUnit);
                break;
            case AbilityRange.EffectiveAtAll:
                FilterByRange(AbilityRange.EffectiveAtAll, selectedUnit);
                break;
            case AbilityRange.InaccurateAtAll:
                FilterByRange(AbilityRange.InaccurateAtAll, selectedUnit);
                break;
        }
        #region OLD
        //switch (selectedAction)
        //{
        //    default:
        //    case MoveAction moveAction:
        //        gridVisualType = GridVisualType.White;
        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;
        //    case DashAction dashAction:
        //        gridVisualType = GridVisualType.White;

        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;
        //    case ShootAction shootAction:
        //        FilterByRange(shootAction.GetRange(), selectedUnit);
        //        break;
        //    case MeleeAction meleeAction:
        //        FilterByRange(meleeAction.GetRange(), selectedUnit);
        //        break;
        //    case PunctureAction punctureAction:
        //        FilterByRange(punctureAction.GetRange(), selectedUnit);
        //        break;
        //    case MurderAction murderAction:
        //        FilterByRange(murderAction.GetRange(), selectedUnit);
        //        break;
        //    case AOEAction aOEAction:
        //        FilterByRange(aOEAction.GetRange(), selectedUnit);
        //        break;
        //    case ArrowVolleyAction volleyAction:
        //        FilterByRange(volleyAction.GetRange(), selectedUnit);
        //        break;
        //    case PommelStrike pommelStrike:
        //        FilterByRange(pommelStrike.GetRange(), selectedUnit);
        //        break;
        //    case MendAction mendAction:
        //        FilterByRange(mendAction.GetRange(), selectedUnit);
        //        break;
        //    case StunBolt stunBolt:
        //        FilterByRange(stunBolt.GetRange(), selectedUnit);
        //        break;
        //    case BrakeALegAction brakeALegAction:
        //        FilterByRange(brakeALegAction.GetRange(), selectedUnit);
        //        break;
        //    case SpinAction spinAction:
        //        gridVisualType = GridVisualType.Blue;
        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;
        //    case DisengageAction disengageAction:
        //        gridVisualType = GridVisualType.Green;
        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;
        //    case BlockAction blockAction:
        //        gridVisualType = GridVisualType.Green;
        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;
        //    case DodgeAction dodgeAction:
        //        gridVisualType = GridVisualType.Green;
        //        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
        //        break;

        //}
        #endregion

    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
                if (gridSystemVisualSingleArray[x, z] != null)
                    gridSystemVisualSingleArray[x, z].Hide();
    }

    private void HideGridPositionRange(GridPosition gridPosition, int range)
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

        HideGridPositionList(gridPosList);
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType, Effectiveness type, Color color)
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

        switch (range)
        {
            case 1://_adjacent
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(adjacentParent, true);
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                    }
                break;
            case 4://_close
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(closeParent, true);
                    }
                break;
            case 6://_far
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(farParent, true);
                    }
                break;
            case 9://_veryFar
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(veryFarParent, true);
                    }
                break;
            default:
                break;
        }



        ShowGridPositionList(gridPosList, gridVisualType, type);
    }
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType, Effectiveness type, Color color)
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

        switch (range)
        {
            case 1://_adjacent
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(adjacentParent, true);
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                    }
                break;
            case 4://_close
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(closeParent, true);
                    }
                break;
            case 6://_far
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(farParent, true);
                    }
                break;
            case 9://_veryFar
                foreach (var position in gridPosList)
                    if (gridSystemVisualSingleArray[position._x, position._z] != null)
                    {
                        gridSystemVisualSingleArray[position._x, position._z].SetGridOutLineColor(color);
                        gridSystemVisualSingleArray[position._x, position._z].transform.SetParent(veryFarParent, true);
                    }
                break;
            default:
                break;
        }


        ShowGridPositionList(gridPosList, gridVisualType, type);
    }

    public void HideGridPositionList(List<GridPosition> gridPositions)
    {
        foreach (GridPosition position in gridPositions)
            if (gridSystemVisualSingleArray[position._x, position._z] != null)
                gridSystemVisualSingleArray[position._x, position._z].Hide();
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType, Effectiveness type)
    {
        foreach (GridPosition position in gridPositions)

            if (gridSystemVisualSingleArray[position._x, position._z] != null)
            {
                Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(position);
                if (targetUnit != null && targetUnit.IsEnemy())
                {
                    targetUnit.SetGridEffectivness(type);
                }
                gridSystemVisualSingleArray[position._x, position._z].Show(GetGridVisualMaterial(gridVisualType));

            }
        //{
        //    Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(position);
        //    if (targetUnit != null)
        //    {
        //        targetUnit.SetGridEffectivness(Effectiveness.Miss);
        //    }
        //}
    }
    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        int localCount = 0;
        foreach (GridPosition position in gridPositions)
        {
            if (gridSystemVisualSingleArray[position._x, position._z] != null)
            {
                //Unit targetUnit = LevelGrid.Instance.GetUnitAtGridPosition(position);
                //if (targetUnit != null)
                //    targetUnit.changef(type);
                gridSystemVisualSingleArray[position._x, position._z].Show(GetGridVisualMaterial(gridVisualType));
            }
        }

    }

    private Material GetGridVisualMaterial(GridVisualType gridVisualType)
    {
        foreach (GridVisualTypeMaterial gridVisualTypeMaterial in gridVisualTypeMaterialList)
        {
            if (gridVisualTypeMaterial.gridVidualType == gridVisualType)
                return gridVisualTypeMaterial.material;
        }

        Debug.Log($"Could not find GridVisualTypeMaterial for GridVisualType {gridVisualType}");
        return null;
    }

    private void FilterByRange(AbilityRange AbilityRange, Unit selectedUnit)
    {
        switch (AbilityRange)
        {
            case AbilityRange.Melee:
                MeleeRange(selectedUnit, _adjacent, _veryFar);
                break;
            case AbilityRange.Close:
                CloseRange(selectedUnit, _close, _far);
                break;
            case AbilityRange.Medium:
                MediumRange(selectedUnit, _adjacent, _close, _far, _veryFar);
                break;
            case AbilityRange.Long:
                LongRange(selectedUnit, _close, _veryFar);
                break;
            case AbilityRange.EffectiveAtAll:
                EffectiveAtAllRanges(selectedUnit, _veryFar);
                break;
            case AbilityRange.InaccurateAtAll:
                InaccurateAtAllRanges(selectedUnit, _veryFar);
                break;
            case AbilityRange.ResetGrid:
                ResetGrid(selectedUnit);
                break;
        }
    }

    private void MeleeRange(Unit selectedUnit, int adjacent, int veryFar)
    {
        ResetGrid(selectedUnit);
        ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), veryFar, GridVisualType.White, Effectiveness.Inaccurate,Color.white);
        ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), adjacent, GridVisualType.Green, Effectiveness.Effective,Color.white);
    }

    private void CloseRange(Unit selectedUnit, int close, int far)
    {
        ResetGrid(selectedUnit);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Yellow, Effectiveness.Miss,Color.yellow);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Green, Effectiveness.Miss,Color.green);
    }

    private void MediumRange(Unit selectedUnit, int adjacent, int close, int far, int veryFar)
    {

        ResetGrid(selectedUnit);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective,Color.green);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Yellow, Effectiveness.Inaccurate, Color.yellow);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Red, Effectiveness.Miss, Color.red);
        HideGridPositionRange(selectedUnit.GetGridPosition(), adjacent);
    }

    private void LongRange(Unit selectedUnit, int close, int veryFar)
    {
        ResetGrid(selectedUnit);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective, Color.green);
        HideGridPositionRange(selectedUnit.GetGridPosition(), close);
    }

    private void EffectiveAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ResetGrid(selectedUnit);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective, Color.green);
    }

    private void InaccurateAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ResetGrid(selectedUnit);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Yellow, Effectiveness.Miss, Color.yellow);
    }
    private void ResetGrid(Unit selectedUnit)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), 0, GridVisualType.Transparent, Effectiveness.Miss, Color.yellow);
    }
    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) { UpdateGridVisual(); }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) { UpdateGridVisual(); }

}