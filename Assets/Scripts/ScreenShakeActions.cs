using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ScreenShakeActions : MonoBehaviour
{
    [SerializeField] float shootShakeIntensity = 5f;

    private void Start()
    {
        ShootAction.OnAnyShoot += ShootAction_OnAnyShoot;
    }

    private void ShootAction_OnAnyShoot(object sender, ShootAction.OnSHootEventArgs e)
    {
        ScreenShake.Instance.Shake(shootShakeIntensity);
    }

}