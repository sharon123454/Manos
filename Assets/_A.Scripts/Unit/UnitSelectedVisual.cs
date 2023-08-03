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

        UpdateVisual(UnitActionSystem.Instance.GetSelectedUnit());
    }

    private void OnDestroy()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit newlySelectedUnit) 
    {
        UpdateVisual(newlySelectedUnit);
    }

    private void UpdateVisual(Unit newlySelectedUnit)
    {
        if (newlySelectedUnit == unit)
            decalProjector.enabled = true;
        else
            decalProjector.enabled = false;
    }

}