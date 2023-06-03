using UnityEngine.Rendering.Universal;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[RequireComponent(typeof(DecalProjector))]
public class UnitSelectedVisual : MonoBehaviour
{
    [SerializeField] private Unit unit;

    private DecalProjector decalProjector;

    private void Awake()
    {
        decalProjector = GetComponent<DecalProjector>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;

        UpdateVisual();
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs empty) 
    {
        UpdateVisual();
    }

    private void UpdateVisual()
    {
        if (UnitActionSystem.Instance.GetSelectedUnit() == unit)
            decalProjector.enabled = true;
        else
            decalProjector.enabled = false;
    }

}