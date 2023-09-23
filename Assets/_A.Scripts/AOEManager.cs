using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum MeshShape { None, Sphere, Cube, Triangle, pointTriangle }
[Serializable]
public struct AOE_MeshType { public MeshShape ShapeType; public Mesh Mesh; }
public class AOEManager : MonoBehaviour
{
    public static AOEManager Instance { get; private set; }
    public static event EventHandler<Unit> OnAnyUnitEnteredAOE; //connect for visual changes on the units
    public static event EventHandler<Unit> OnAnyUnitExitedAOE;

    [SerializeField] private AOE_MeshType[] meshArrayType;
    [SerializeField] private float _meleeRange = 3f;
    [SerializeField] private float _closeRange = 9f;
    [SerializeField] private float _longRange = 15f;

    private List<Unit> _inRangeUnits;
    private MeshCollider _collider;
    private MeshFilter _meshVisual;
    private float _clampRange = 3f;
    private bool _isAOEActive;
    private bool _isFollowingMouse;
    private Vector3 _mousePos;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }
    private void Start()
    {
        _inRangeUnits = new List<Unit>();
        _collider = GetComponent<MeshCollider>();
        _meshVisual = GetComponent<MeshFilter>();
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void Update()
    {
        if (_isAOEActive)
        {
            if (_isFollowingMouse)
            {
                _mousePos = MouseWorld.GetPosition();
                transform.position = transform.parent.position + Vector3.ClampMagnitude(_mousePos - transform.parent.position, _clampRange);
            }
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
    public void SetIsAOEActive(bool isfollowMouse, Vector3 centerZonePosition, MeshShape AOEMeshType, float rangeMultiplicator, ActionRange abilityRange)
    {
        _isFollowingMouse = isfollowMouse;
        InitAOE(centerZonePosition, AOEMeshType, rangeMultiplicator, abilityRange);
    }
    public void DisableAOE()
    {
        transform.parent.position = Vector3.zero;
        transform.localScale = Vector3.one;
        _isAOEActive = false;
        _clampRange = 1f;
    }

    private void InitAOE(Vector3 aOEPositiion, MeshShape typeOfShape, float rangeMultiplicator, ActionRange abilityRange)
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
                    _collider.sharedMesh = shape.Mesh;
                    _meshVisual.mesh = shape.Mesh;
                    _isAOEActive = true;

                    switch (abilityRange)
                    {
                        case ActionRange.Self:
                            if (transform.position != aOEPositiion)
                                transform.position = aOEPositiion;
                            break;
                        case ActionRange.Melee:
                            _clampRange = _meleeRange;
                            break;
                        case ActionRange.Close:
                            _clampRange = _closeRange;
                            break;
                        case ActionRange.Medium:
                        case ActionRange.Long:
                        case ActionRange.InaccurateAtAll:
                        case ActionRange.EffectiveAtAll:
                            _clampRange = _longRange;
                            break;
                        case ActionRange.Move:
                        case ActionRange.ResetGrid:
                        default:
                            _clampRange = 0;
                            break;
                    }
                }
            }
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        DisableAOE();
    }

}