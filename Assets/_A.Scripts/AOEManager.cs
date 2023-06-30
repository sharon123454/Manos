using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum MeshShape { None, Sphere, Cube }
[Serializable]
public struct AOE_MeshType { public MeshShape ShapeType; public Mesh Mesh; }
public class AOEManager : MonoBehaviour
{
    public static AOEManager Instance { get; private set; }
    public static event EventHandler<Unit> OnAnyUnitEnteredAOE; //connect for visual changes on the units
    public static event EventHandler<Unit> OnAnyUnitExitedAOE;

    //public MeshShape shapeOnClickTEST = MeshShape.Cube;
    [SerializeField] private AOE_MeshType[] meshArrayType;

    private List<Unit> _inRangeUnits;
    private MeshCollider _collider;
    private MeshFilter _meshVisual;
    private float _clampRange = 3f;
    private bool _isAOEActive;
    private Vector3 _mousePos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
        _inRangeUnits = new List<Unit>();
        _collider = GetComponent<MeshCollider>();
        _meshVisual = GetComponent<MeshFilter>();
    }

    private void Update()
    {
        if (_isAOEActive)
        {
            _mousePos = MouseWorld.GetPosition();
            transform.position = transform.parent.position + Vector3.ClampMagnitude(_mousePos - transform.parent.position, _clampRange);
        }
        else
        {
            if (transform.position != Vector3.zero)
            {
                transform.position = Vector3.zero;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        Unit unit;
        if (unit = other.gameObject.GetComponent<Unit>())
            if (!_inRangeUnits.Contains(unit))
            {
                _inRangeUnits.Add(unit);
                OnAnyUnitEnteredAOE?.Invoke(this, unit);
            }
    }
    private void OnTriggerExit(Collider other)
    {
        Unit unit;
        if (unit = other.gameObject.GetComponent<Unit>())
            if (_inRangeUnits.Contains(unit))
            {
                _inRangeUnits.Remove(unit);
                OnAnyUnitExitedAOE?.Invoke(this, unit);
            }
    }

    public List<Unit> GetUnitsInRange() { return _inRangeUnits; }
    public void SetIsAOEActive(bool isActive, Vector3 centerZonePosition, MeshShape AOEMeshType, float rangeMultiplicator, AbilityRange abilityRange)
    {
        if (!isActive || AOEMeshType == MeshShape.None) { DisableAOE(); return; }

        InitAOE(centerZonePosition, AOEMeshType, rangeMultiplicator, abilityRange);
    }

    private void InitAOE(Vector3 aOEPositiion, MeshShape typeOfShape, float rangeMultiplicator, AbilityRange abilityRange)//make use of range for range clamp and not range multiplicator
    {
        if (meshArrayType.Length > 0)
        {
            foreach (var shape in meshArrayType)
            {
                if (typeOfShape == shape.ShapeType)
                {
                    if (!shape.Mesh)
                    {
                        Debug.Log($"{name}: {typeOfShape} Mesh reference is missing");
                        return;
                    }

                    transform.localScale = Vector3.one * LevelGrid.Instance.GetCellSize() * rangeMultiplicator;
                    transform.parent.position = aOEPositiion;
                    _clampRange = rangeMultiplicator;
                    _collider.sharedMesh = shape.Mesh;
                    _meshVisual.mesh = shape.Mesh;
                }
            }
        }

        _isAOEActive = true;
    }
    private void DisableAOE()
    {
        transform.parent.position = Vector3.zero;
        transform.localScale = Vector3.one;
        _isAOEActive = false;
        _clampRange = 1f;
    }

}