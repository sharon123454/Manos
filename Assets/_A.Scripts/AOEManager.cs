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

    //public MeshShape shapeOnClickTEST = MeshShape.Cube;
    [SerializeField] private AOE_MeshType[] meshArrayType;

    private List<Unit> _inRangeUnits;
    private MeshCollider _collider;
    private MeshFilter _meshVisual;
    private bool isAOEActive;

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
        ////testing
        //if (ManosInputController.Instance.Click.IsPressed())
        //{
        //    SetAOE(shapeOnClickTEST);
        //    isAOEActive = !isAOEActive;
        //}

        if (isAOEActive)
        {
            transform.position = MouseWorld.GetPosition();
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

    public void SetAOE(MeshShape typeOfShape)
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

                    _meshVisual.mesh = shape.Mesh;
                    _collider.sharedMesh = shape.Mesh;
                }
            }
        }
    }

}