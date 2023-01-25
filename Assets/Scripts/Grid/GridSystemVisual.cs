using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEditor;

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
    [SerializeField] private int _far = 9;
    [SerializeField] private int _veryFar = 15;

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

                if (Physics.Raycast(myWorldPos + Vector3.up * 5, Vector3.down, out ray, 2000, PathFinding.Instance.obstacleLayerMask)) { }

                else if (Physics.Raycast(myWorldPos + Vector3.up * 5, Vector3.down, out ray, 2000, PathFinding.Instance.floorGridLayer))
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
        HideAllGridPosition();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();

        GridVisualType gridVisualType;

        switch (selectedAction)
        {
            default:
            case MoveAction moveAction:
                gridVisualType = GridVisualType.White;
                break;
            case ShootAction shootAction:
                gridVisualType = GridVisualType.Red;
                FilterByRange(shootAction.GetRange(), selectedUnit);
                break;
            case MeleeAction meleeAction:
                gridVisualType = GridVisualType.Red;

                ShowGridPositionRangeSquare(selectedUnit.GetGridPosition(), meleeAction.GetMaxMeleeDistance(), GridVisualType.RedSoft);
                break;
            case AOEAction aOEAction:
                gridVisualType = GridVisualType.Yellow;
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    public void FilterByRange(AbilityRange AbilityRange, Unit selectedUnit)
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
                MediumRange(selectedUnit, _veryFar, _far, _close, _adjacent);
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
        ShowGridPositionRange(selectedUnit.GetGridPosition(), adjacent, GridVisualType.Green);
    }

    private void CloseRange(Unit selectedUnit, int close, int far)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Yellow);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Green);
    }

    private void MediumRange(Unit selectedUnit, int adjacent, int close, int far, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Yellow);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), far, GridVisualType.Green);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Yellow);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), adjacent, GridVisualType.Red);
    }

    private void LongRange(Unit selectedUnit, int close, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green);
        ShowGridPositionRange(selectedUnit.GetGridPosition(), close, GridVisualType.Red);
    }

    private void EffectiveAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Green);
    }

    private void InaccurateAtAllRanges(Unit selectedUnit, int veryFar)
    {
        ShowGridPositionRange(selectedUnit.GetGridPosition(), veryFar, GridVisualType.Yellow);
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
                if (gridSystemVisualSingleArray[x, z] != null)
                    gridSystemVisualSingleArray[x, z].Hide();
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition position in gridPositions)
            if (gridSystemVisualSingleArray[position._x, position._z] != null)
                gridSystemVisualSingleArray[position._x, position._z].Show(GetGridVisualMaterial(gridVisualType));
    }

    private void ShowGridPositionRange(GridPosition gridPosition, int range, GridVisualType gridVisualType)
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

        ShowGridPositionList(gridPosList, gridVisualType);
    }

    private void ShowGridPositionRangeSquare(GridPosition gridPosition, int range, GridVisualType gridVisualType)
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

    private void LevelGrid_OnAnyUnitMovedGridPosition(object sender, EventArgs e) { UpdateGridVisual(); }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e) { UpdateGridVisual(); }

}