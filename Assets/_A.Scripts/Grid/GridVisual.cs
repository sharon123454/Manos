using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class GridVisual : MonoBehaviour
{
    private static DecalProjector _lastActiveGridDecal;
    private static MeshRenderer _lastActiveGridMesh;

    private DecalProjector _decalProjector;
    private MeshRenderer _gridVisual;

    private void Awake()
    {
        _decalProjector = GetComponent<DecalProjector>();
        _gridVisual = GetComponent<MeshRenderer>();
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        MoveAction selectedMoveAction = UnitActionSystem.Instance.GetSelectedMoveAction();

        if (!TurnSystem.Instance.IsPlayerTurn() || !selectedUnit
            || !selectedMoveAction || !other.CompareTag("Mouse"))
        { FollowMouse.Instance.TryResetLines(); return; }

        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

        if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, LayerMask.GetMask("MousePlane")))
        {
            if (_rayCastHit.point == null) { return; }

            GridPosition mouseGridPosition = LevelGrid.Instance.GetGridPosition(_rayCastHit.point);

            if (LevelGrid.Instance.HasAnyUnitOnGridPosition(mouseGridPosition)) { return; }

            if (LevelGrid.Instance.IsValidGridPosition(mouseGridPosition))
            {
                List<GridPosition> path = PathFinding.Instance.FindPath(selectedUnit.GetGridPosition(), mouseGridPosition, out int pathLength);

                if (path == null || path.Count > selectedMoveAction.GetMoveValue()) { return; }

                FollowMouse.Instance.DrawLineOnPath(path);
            }
            else
                return;

            SingleActivationGridLogic();
        }
    }

    private void OnTriggerStay(Collider other)
    {
        //if (other.CompareTag("Mouse"))
        //Debug.Log("on stay");
    }
    private void OnTriggerExit(Collider other)
    {
        //if (other.CompareTag("Mouse"))
        //Debug.Log("on Exit");
    }

    public bool IsVisualActive()
    {
        return _gridVisual.enabled;
    }
    public void ShowGridVisual()
    {
        if (_gridVisual)
            _gridVisual.enabled = true;
    }
    public void UpdateVisualGridColor(Color color)
    {
        if (_gridVisual)
            _gridVisual.material.color = color;
    }
    public void HideGridVisual()
    {
        if (_gridVisual)
            _gridVisual.enabled = false;
    }

    private void SingleActivationGridLogic()
    {
        if (_decalProjector)
        {
            if (_lastActiveGridDecal)
                _lastActiveGridDecal.enabled = false;

            _decalProjector.enabled = true;
            _lastActiveGridDecal = _decalProjector;
        }
        if (_gridVisual)
        {
            if (_lastActiveGridMesh)
                _lastActiveGridMesh.enabled = false;

            ShowGridVisual();
            _lastActiveGridMesh = _gridVisual;
        }
    }

}