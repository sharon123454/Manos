using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitAnimator : MonoBehaviour
{
    [SerializeField] private Animator animator;
    [SerializeField] private Transform bulletProjectilePrefab;
    [SerializeField] private Transform shootPointTransform;

    private VFXLibrary vfxLibrary;

    private void Awake()
    {
        vfxLibrary = animator.gameObject.GetComponent<VFXLibrary>();

        BaseHeal[] _healActions = GetComponents<BaseHeal>();
        MoveAction[] _moveActions = GetComponents<MoveAction>();
        RangedAction[] _shootActions = GetComponents<RangedAction>();
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
            foreach (RangedAction shootAction in _shootActions)
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
            dodgeAction.OnDodgeActivate += DodgeAction_OnDodgeAction;
        }

        if (TryGetComponent<BlockAction>(out BlockAction blockAction))
        {
            blockAction.OnBlockActivate += BlockAction_OnBlockAction;
        }

        if (TryGetComponent<DisengageAction>(out DisengageAction disengageAction))
        {
            disengageAction.OnDisengageActivate += DisengageAction_OnDisengage;
        }

    }

    public void OnActionStarted(string actionName)
    {
        animator.Play($"{actionName}_Anim");
    }

    public void OnStatusEffectRecieved(StatusEffect statusEffect)
    {
        vfxLibrary.OnStatusEffectRecieved(statusEffect);
    }
    public void OnStatusEffectRemoved(StatusEffect statusEffect)
    {
        vfxLibrary.OnStatusEffectRemoved(statusEffect);
    }
    public void OnDamaged()
    {
        animator.SetFloat("GetHitBlend", 0);
        animator.SetTrigger("Hit");
        vfxLibrary.PlayTakeDamage();
    }
    public void OnCriticallyHit()
    {
        animator.SetFloat("GetHitBlend", 1);
        animator.SetTrigger("Hit");
        vfxLibrary.PlayTakeDamage();
    }
    public void OnDead()
    {
        animator.SetBool("IsDead", true);
    }
    public void OnDodge()
    {
        animator.SetTrigger("Dodge");
    }

    private void DodgeAction_OnDodgeAction(object sender, EventArgs e)
    {
    }
    private void BlockAction_OnBlockAction(object sender, EventArgs e)
    {

    }
    private void DisengageAction_OnDisengage(object sender, EventArgs e)
    {

    }

    private void ShootAction_OnShoot(object sender, RangedAction.OnSHootEventArgs e)
    {
        animator.SetFloat("CastBlend", 0);
        animator.SetTrigger("Shoot");

        Transform bulletProjectileTransform = Instantiate(bulletProjectilePrefab, shootPointTransform.position, Quaternion.identity);
        BulletProjectile bulletProjectile = bulletProjectileTransform.GetComponent<BulletProjectile>();

        Vector3 targetUnitShootAtPosition = e.TargetUnit.GetWorldPosition();
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
    }
    private void BaseHeal_OnHealActionCompleted(object sender, EventArgs e)
    {
    }

}