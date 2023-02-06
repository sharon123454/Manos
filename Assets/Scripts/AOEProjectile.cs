using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class AOEProjectile : MonoBehaviour
{
    public static event EventHandler OnAnyAOEHit;

    [SerializeField] private float damage = 30;
    [SerializeField] private float hitChance = 100;
    [SerializeField] private float postureDamage = 15;
    [SerializeField] private LayerMask damageLayer;
    [SerializeField] private float moveSpeed = 15f;
    [Tooltip("Unity world units, grid scale needs to be multiplied")]
    [SerializeField] private float damageRadius = 2f;
    [SerializeField] private float reachedTargetDistance = 0.2f;
    [SerializeField] private float hightFromGround = 1f;
    [SerializeField] private Transform aOEHitVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private AnimationCurve arcYAnimationCurve;

    private Vector3 targetPosition;
    private Action onAOEBehaviourComplete;
    private Vector3 positionXZ;
    private float totalDistance;

    private void Update()
    {
        Vector3 moveDir = (targetPosition - positionXZ).normalized;

        positionXZ += moveDir * moveSpeed * Time.deltaTime;

        // distance to target
        float distance = Vector3.Distance(positionXZ, targetPosition);
        // 1-distance as distance decreases, progress distances increases
        float distanceNormalized = 1 - distance / totalDistance;

        float maxHeight = totalDistance / 4;
        float positionY = arcYAnimationCurve.Evaluate(distanceNormalized) * maxHeight;
        transform.position = new Vector3(positionXZ.x, positionY, positionXZ.z);

        if (Vector3.Distance(positionXZ, targetPosition) < reachedTargetDistance)
        {
            Collider[] colliderArray = Physics.OverlapSphere(targetPosition, damageRadius, damageLayer);

            foreach (Collider collider in colliderArray)
                if (collider.TryGetComponent<Unit>(out Unit targetUnit))
                {
                    targetUnit.Damage(damage,postureDamage, hitChance);
                }

            OnAnyAOEHit?.Invoke(this, EventArgs.Empty);

            trailRenderer.transform.parent = null;

            Instantiate(aOEHitVFXPrefab, targetPosition + Vector3.up * hightFromGround, Quaternion.identity);

            Destroy(gameObject);

            onAOEBehaviourComplete();
        }
    }

    public void Setup(GridPosition targetGridPosition, Action onAOEBehaviourComplete)
    {
        this.onAOEBehaviourComplete = onAOEBehaviourComplete;
        targetPosition = LevelGrid.Instance.GetWorldPosition(targetGridPosition);

        positionXZ = transform.position;
        positionXZ.y = 0;
        totalDistance = Vector3.Distance(positionXZ, targetPosition);
    }

}