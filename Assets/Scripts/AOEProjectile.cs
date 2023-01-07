using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class AOEProjectile : MonoBehaviour
{
    [SerializeField] private float damage = 30;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private float moveSpeed = 15f;
    [Tooltip("Unity world units, grid scale needs to be multiplied")]
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private float reachedTargetDistance = 0.2f;

    private Vector3 targetPosition;
    private Action onAOEBehaviourComplete;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - transform.position).normalized;

        transform.position += moveDir * moveSpeed * Time.deltaTime;

        if (Vector3.Distance(transform.position, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius, damageLayer);

            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                    targetUnit.Damage(damage);

            Destroy(gameObject);

            onAOEBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onAOEBehaviourComplete)
    {
        this.onAOEBehaviourComplete = onAOEBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);
    }

}