using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] float shootShakeIntensity = 5f;
    [SerializeField] float aOEShakeIntensity = 8f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
        AOEProjectile.OnAnyAOEHit += AOEProjectile_OnAnyAOEHit;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnSHootEventArgs e)
    {
        ScreenShake.Instance.Shake(shootShakeIntensity);
    }

    private void AOEProjectile_OnAnyAOEHit(object sender, EventArgs e)
    {
        ScreenShake.Instance.Shake(aOEShakeIntensity);
    }

}