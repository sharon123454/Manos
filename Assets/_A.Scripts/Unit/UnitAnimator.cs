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

        if (TryGetComponent<DodgeAction>(out DodgeAction dodgeAction))
        {
            dodgeAction.OnDodgeAction += DodgeAction_OnDodgeAction;
        }

        if (TryGetComponent<BlockAction>(out BlockAction blockAction))
        {
            blockAction.OnBlockAction += BlockAction_OnBlockAction;
        }

        if (TryGetComponent<DisengageAction>(out DisengageAction disengageAction))
        {
            disengageAction.OnDisengage += DisengageAction_OnDisengage;
        }

        //if (TryGetComponent<ResponsibilityAction>(out ResponsibilityAction resposiblityAction))
        //{
        //    resposiblityAction.OnDivineActive += ResposiblityAction_OnDivineActive;
        //}
    }

    private void Start()
    {
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (transform.GetComponent<Unit>() == unit)
            animator.SetBool("IsDead", true);
    }

    private void DodgeAction_OnDodgeAction(object sender, EventArgs e)
    {
        animator.SetTrigger("Dodge");
    }

    private void BlockAction_OnBlockAction(object sender, EventArgs e)
    {

    }

    private void DisengageAction_OnDisengage(object sender, EventArgs e)
    {

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

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.targetUnit.GetWorldPosition();
        targetUnitShootAtPosition.y = shootPointTransform.position.y;

        bulletProjectile.SetUp(targetUnitShootAtPosition);
    }
    
    private void ResposiblityAction_OnDivineActive(object sender, EventArgs e)
    {
        animator.SetFloat("CastBlend",0);
        animator.SetTrigger("Shoot");
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