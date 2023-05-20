using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

[RequireComponent(typeof(DecalProjector))]
public class GridVisual : MonoBehaviour
{
    [SerializeField] private Outline _Outline;

    private DecalProjector _decalProjector;
    private Color _transparant = new(0, 0, 0, 0);

    private void Awake()
    {
        _decalProjector = GetComponent<DecalProjector>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Mouse") && _Outline.OutlineColor != _transparant)//checking transperancy is troll need real fix but works
        {
            _decalProjector.enabled = true;

            if (UnitActionSystem.Instance.GetSelectedMoveAction())
            {
                GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(MouseWorld.GetPosition());

                if (LevelGrid.Instance.IsValidGridPosition(mouseGridPosition))
                {
                    //find and draw path to mouse grid position
                    FollowMouse.Instance.DrawLineOnPath(
                        PathFinding.Instance.FindPath(UnitActionSystem.Instance.GetSelectedUnit().GetGridPosition(),
                        mouseGridPosition, out int pathLength));
                }
            }
        }
    }
    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Mouse"))
        //Debug.Log("on stay");
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Mouse"))
            _decalProjector.enabled = false;
    }

}