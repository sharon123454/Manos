using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Testing : MonoBehaviour
{
    [SerializeField] Unit unit;

    private void Awake()
    {
        
    }

    private void Start()
    {

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            GridPosition mouse = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

            List<GridPosition> gridPos = PathFinding.Instance.FindPath(unit.GetGridPosition(), mouse);
            for (int i = 0; i < gridPos.Count - 1; i++)
            {
                Debug.DrawLine
                    (
                    LevelGrid.Instance.GetWorldPosition(gridPos[i]),
                    LevelGrid.Instance.GetWorldPosition(gridPos[i + 1]),
                    Color.red, 10f
                    );
            }
        }
    }
}