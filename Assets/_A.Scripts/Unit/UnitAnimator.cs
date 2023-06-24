using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private OnMelee vfxLibrary;

    private void Awake()
    {
        vfxLibrary = animator.gameObject.GetComponent<OnMelee>();

        BaseHeal[] _healActions = GetComponents<BaseHeal>();
        MoveAction[] _moveActions = GetComponents<MoveAction>();
        ShootAction[] _shootActions = GetComponents<ShootAction>();
        MeleeAction[] _meleeActions = GetComponents<MeleeAction>();

        if (_moveActions.Length > 0)
            foreach (MoveAction moveAction in _moveActions)
            {
                if (moveAction)
                {
                    moveAction.OnStartMoving += MoveAction_OnStartMoving;
                    moveAction.OnStopMoving += MoveAction_OnStopMoving;
                }
            }

        if (_shootActions.Length > 0)
            foreach (ShootAction shootAction in _shootActions)
                if (shootAction)
                {
                    shootAction.OnShoot += ShootAction_OnShoot;
                }

        if (_meleeActions.Length > 0)
            foreach (MeleeAction meleeAction in _meleeActions)
                if (meleeAction)
                {
                    meleeAction.OnMeleeActionStarted += MeleeAction_OnMeleeActionStarted;
                    meleeAction.OnMeleeActionCompleted += MeleeAction_OnMeleeActionCompleted;
                }

        if (_healActions.Length > 0)
            foreach (BaseHeal healAction in _healActions)
                if (healAction)
                {
                    healAction.OnHealActionStarted += BaseHeal_OnHealActionStarted;
                    healAction.OnHealActionCompleted += BaseHeal_OnHealActionCompleted;
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

    }

    private void Start()
    {
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        Unit.OnAnyUnitDamaged += Unit_OnAnyUnitDamaged;
        Unit.OnAnyUnitCriticallyHit += Unit_OnAnyUnitCriticallyHit;
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (transform.GetComponent<Unit>() == unit)
            animator.SetBool("IsDead", true);
    }
    private void Unit_OnAnyUnitDamaged(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (transform.GetComponent<Unit>() == unit)
        {
            animator.SetFloat("GetHitBlend", 0);
            animator.SetTrigger("Hit");
        }
    }
    private void Unit_OnAnyUnitCriticallyHit(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        if (transform.GetComponent<Unit>() == unit)
        {
            animator.SetFloat("GetHitBlend", 1);
            animator.SetTrigger("Hit");
        }
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

    private void ShootAction_OnShoot(object sender, ShootAction.OnSHootEventArgs e)
    {
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
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

    private void MeleeAction_OnMeleeActionStarted(object sender, EventArgs e)
    {
        animator.SetTrigger("MeleeAttack");
    }
    private void MeleeAction_OnMeleeActionCompleted(object sender, EventArgs e)
    {

    }

    private void BaseHeal_OnHealActionStarted(object sender, EventArgs e)
    {
        animator.SetFloat("CastBlend", 0);
        animator.SetTrigger("Shoot");
    }
    private void BaseHeal_OnHealActionCompleted(object sender, EventArgs e)
    {
        //vfxLibrary.StopHealVFX();
    }

}