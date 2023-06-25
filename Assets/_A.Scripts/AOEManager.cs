using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public enum MeshShape { Sphere, Cube }
[Serializable]
public struct AOE_MeshType { public MeshShape ShapeType; public Mesh Mesh; }
public class AOEManager : MonoBehaviour
{
    public static AOEManager Instance { get; private set; }
    public static event EventHandler<Unit> OnAnyUnitEnteredAOE; //connect for visual changes on the units
    public static event EventHandler<Unit> OnAnyUnitExitedAOE;

    public MeshShape shapeOnClickTEST = MeshShape.Cube;
    [SerializeField] private AOE_MeshType[] meshArrayType;

    private List<Unit> _inRangeUnits;
    private MeshCollider _collider;
    private MeshFilter _meshVisual;

    private void Update()//testing
    {
        //if (ManosInputController.Instance.Click.IsPressed())
        //{
        //    SetAOE(shapeOnClickTEST);
        //}
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
        _inRangeUnits = new List<Unit>();
        _collider = GetComponent<MeshCollider>();
        _meshVisual = GetComponent<MeshFilter>();
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

    public void SetAOE(MeshShape typeOfShape)
    {
        foreach (var shape in meshArrayType)
        {
            if (typeOfShape == shape.ShapeType)
            {
                _meshVisual.mesh = shape.Mesh;
                _collider.sharedMesh = shape.Mesh;
            }
        }
    }

}