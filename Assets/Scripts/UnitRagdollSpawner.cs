using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitRagdollSpawner : MonoBehaviour
{
    [SerializeField] private Transform originalRootBone;
    [SerializeField] private Transform ragdollPrefab;

    private UnitStats unitStats;

    private void Awake()
    {
        unitStats = GetComponent<UnitStats>();

        unitStats.OnDeath += HealthSystem_OnDeath;
    }

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        Transform ragdollTransform = Instantiate(ragdollPrefab, transform.position, transform.rotation);
        UnitRagdoll unitRagdoll = ragdollTransform.GetComponent<UnitRagdoll>();
        unitRagdoll.Setup(originalRootBone);
    }

}