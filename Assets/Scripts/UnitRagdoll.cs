using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class UnitRagdoll : MonoBehaviour
{
    [SerializeField] private Transform ragdollRootBone;
    [SerializeField] private float ragdollForce = 300f, ragdollRange = 10f;

    public void Setup(Transform originalRootBone)
    {
        MathcAllChildTransforms(originalRootBone, ragdollRootBone);

        Vector3 randomDir = new Vector3(Random.Range(-1f, +1f), 0, Random.Range(-1f, +1f));
        ApplyForceToRagdoll(ragdollRootBone, ragdollForce, transform.position + randomDir, ragdollRange);
    }

    private void MathcAllChildTransforms(Transform root, Transform clone)
    {
        foreach (Transform child in root)
        {
            Transform cloneChild = clone.Find(child.name);

            if (cloneChild != null)
            {
                cloneChild.position = child.position;
                cloneChild.rotation = child.rotation;

                MathcAllChildTransforms(child, cloneChild);
            }
        }
    }

    private void ApplyForceToRagdoll(Transform root, float forceAmount, Vector3 forcePosition, float forceRange)
    {
        foreach (Transform child in root)
        {
            if (child.TryGetComponent<Rigidbody>(out Rigidbody childRigidBody))
            {
                childRigidBody.AddExplosionForce(forceAmount, forcePosition, forceRange);
            }

            ApplyForceToRagdoll(child, forceAmount, forcePosition, forceRange);
        }
    }

}