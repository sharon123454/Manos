using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private void Awake()
    {
        if (TryGetComponent<MoveAction>(out MoveAction moveAction))
        {
            moveAction.OnStartMoving += MoveAction_OnStartMoving;
            moveAction.OnStopMoving += MoveAction_OnStopMoving;
        }

        if (TryGetComponent<DashAction>(out DashAction dashAction))
        {
            dashAction.OnStartMoving += DashAction_OnStartMoving;
            dashAction.OnStopMoving += DashAction_OnStopMoving;
        }

        if (TryGetComponent<ShootAction>(out ShootAction shootAction))
        {
            shootAction.OnShoot += ShootAction_OnShoot;
        }

        if (TryGetComponent<MeleeAction>(out MeleeAction meleeAction))
        {
            meleeAction.OnMeleeActionStarted += MeleeAction_OnMeleeActionStarted;
            meleeAction.OnMeleeActionCompleted += MeleeAction_OnMeleeActionCompleted;
        }
    }

    private void MeleeAction_OnMeleeActionCompleted(object sender, EventArgs e)
    {

    }

    private void MeleeAction_OnMeleeActionStarted(object sender, EventArgs e) 
    {
        animator.SetTrigger("MeleeAttack");
    }

    private void ShootAction_OnShoot(object sender, ShootAction.OnSHootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform =  Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }

    private void MoveAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void MoveAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

    private void DashAction_OnStartMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", true);
    }

    private void DashAction_OnStopMoving(object sender, EventArgs e)
    {
        animator.SetBool("IsWalking", false);
    }

}