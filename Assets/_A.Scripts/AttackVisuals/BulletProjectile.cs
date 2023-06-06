using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BulletProjectile : MonoBehaviour
{
    [SerializeField] private Transform bulletHitVFXPrefab;
    [SerializeField] private TrailRenderer trailRenderer;
    [SerializeField] private float speed = 200f;

    private Vector3 targetPos;

    private void Update()
    {
        Vector3 moveDir = (targetPos - transform.position).normalized;

        float distanceBeforeMoving = Vector3.Distance(transform.position, targetPos);

        transform.position += moveDir * speed * Time.deltaTime;

        float distanceAfterMoving = Vector3.Distance(transform.position, targetPos);

        if (distanceBeforeMoving < distanceAfterMoving)
        {
            transform.position = targetPos;

            if (trailRenderer)
                trailRenderer.transform.parent = null;

            Destroy(gameObject);

            Instantiate(bulletHitVFXPrefab, targetPos, Quaternion.identity);
        }
    }

    public void SetUp(Vector3 targetPosition)
    {
        targetPos = targetPosition;
    }

}