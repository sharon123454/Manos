using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] float MeleeShakeIntensity = 0.1f;
    [SerializeField] float shootShakeIntensity = 5f;
    [SerializeField] float aOEShakeIntensity = 8f;

    private void Start()
    {
        AOEProjectile.OnAnyAOEHit += AOEProjectile_OnAnyAOEHit;
        MeleeAction.OnAnyMeleeHit += MeleeAction_OnAnyMeleeHit;
        RangedAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, RangedAction.OnSHootEventArgs e)
    {
        ScreenShake.Instance.Shake(shootShakeIntensity);
    }

    private void AOEProjectile_OnAnyAOEHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(aOEShakeIntensity);
    }

    private void MeleeAction_OnAnyMeleeHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(MeleeShakeIntensity);
    }

}