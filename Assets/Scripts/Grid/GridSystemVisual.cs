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

    private GridSystemVisualSingle[,] gridSystemVisualSingleArray;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        gridSystemVisualSingleArray = new GridSystemVisualSingle[LevelGrid.Instance.GetWidth(),LevelGrid.Instance.GetLength()];

        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
        {
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
            {
                for (int y = 0; y < LevelGrid.Instance.GetHeight(); y++)
                {
                    GridPosition gridPosition = new GridPosition(x, z, y);
                    Transform gridSystemVisualSingleTransform =
                        Instantiate(GridSystemVisualSinglePrefab, LevelGrid.Instance.GetWorldPosition(gridPosition), Quaternion.identity);

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

                ShowGridPositionRange(selectedUnit.GetGridPosition(), shootAction.GetMaxShootDistance(), GridVisualType.RedSoft);
                break;
            case SpinAction spinAction:
                gridVisualType = GridVisualType.Blue;
                break;
        }

        ShowGridPositionList(selectedAction.GetValidActionGridPositionList(), gridVisualType);
    }

    public void HideAllGridPosition()
    {
        for (int x = 0; x < LevelGrid.Instance.GetWidth(); x++)
            for (int z = 0; z < LevelGrid.Instance.GetLength(); z++)
                gridSystemVisualSingleArray[x, z].Hide(); ;
    }

    public void ShowGridPositionList(List<GridPosition> gridPositions, GridVisualType gridVisualType)
    {
        foreach (GridPosition position in gridPositions)
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