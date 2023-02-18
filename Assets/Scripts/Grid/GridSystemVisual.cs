using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class GridSystemVisual : MonoBehaviour
{
    public static GridSystemVisual Instance { get; private set; }

    [Serializable]
    public struct GridVisualTypeMaterail
    {
        public GridVisualType gridVidualType;
        public Material material;
    }

    public enum GridVisualType { White, Red, RedSoft, Blue, Green, Yellow }

    [SerializeField] private Transform GridSystemVisualSinglePrefab;
    [SerializeField] private List<GridVisualTypeMaterail> gridVisualTypeMaterialList;
    [SerializeField] private int _adjacent = 1;
    [SerializeField] private int _close = 4;
    [SerializeField] private int _far = 6;
    [SerializeField] private int _veryFar = 9;

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

                if (Physics.Raycast(myWorldPos + new Vector3(transform.position.x,0,transform.position.z) + Vector3.up * 10, Vector3.down, out ray, 2000, PathFinding.Instance.obstacleLayerMask)) { }

                else if (Physics.Raycast(myWorldPos + new Vector3(transform.position.x, 0, transform.position.z) + Vector3.up * 10, Vector3.down, out ray, 20000, PathFinding.Instance.floorGridLayer))
                {
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(GridSystemVisualSinglePrefab, new Vector3(myWorldPos.x, ray.point.y, myWorldPos.z), Quaternion.identity);

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

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case DashAction dashAction:
                gridVisualType = GridVisualType.White;

                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case ShootAction shootAction:
                FilterByRange(shootAction.GetRange(), selectedUnit);
                break;
            case MeleeAction meleeAction:
                FilterByRange(meleeAction.GetRange(), selectedUnit);
                break;
            case PunctureAction punctureAction:
                FilterByRange(punctureAction.GetRange(), selectedUnit);
                break;
            case MurderAction murderAction:
                FilterByRange(murderAction.GetRange(), selectedUnit);
                break;
            case AOEAction aOEAction:
                FilterByRange(aOEAction.GetRange(), selectedUnit);
                break;
            case ArrowVolleyAction volleyAction:
                FilterByRange(volleyAction.GetRange(), selectedUnit);
                print(volleyAction.GetRange().ToString());
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case DisengageAction disengageAction:
                gridVisualType = GridVisualType.Green;
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case BlockAction blockAction:
                gridVisualType = GridVisualType.Green;
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;
            case DodgeAction dodgeAction:
                gridVisualType = GridVisualType.Green;
                ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
                break;

        }
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

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType, Effectiveness type)
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

        ShowGridPositionList(gridPosList, gridVisualType, type);
    }
    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType, Effectiveness type)
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

        ShowGridPositionList(gridPosList, gridVisualType);
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
        foreach (GridVisualTypeMaterail gridVisualTypeMaterial in gridVisualTypeMaterialList)
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
                MeleeRange(selectedUnit, _adjacent);
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
        }
    }

    private void MeleeRange(Unit selectedUnit, int adjacent)
    {
        ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), adjacent, GridVisualType.RedSoft, Effectiveness.Effective);
    }

    private void CloseRange(Unit selectedUnit, int close, int far)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Yellow, Effectiveness.Miss);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Green, Effectiveness.Miss);
    }

    private void MediumRange(Unit selectedUnit, int adjacent, int close, int far, int veryFar)
    {

        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Yellow, Effectiveness.Inaccurate);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Red, Effectiveness.Miss);
        HideGridPositionRange(selectedUnit.GetGridPosition(), adjacent);
    }

    private void LongRange(Unit selectedUnit, int close, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective);
        HideGridPositionRange(selectedUnit.GetGridPosition(), close);
    }

    private void EffectiveAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green, Effectiveness.Effective);
    }

    private void InaccurateAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Yellow, Effectiveness.Miss);
    }

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) { UpdateGridVisual(); }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) { UpdateGridVisual(); }

}