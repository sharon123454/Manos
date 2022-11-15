using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class Unit : MonoBehaviour
{
    [SerializeField] private Animator unitAnimator;
    [SerializeField] private float moveSpeed = 4;
    [SerializeField] private float rotateSpeed = 7.5f;
    private float stoppingDistance = .1f;
    private Vector3 targetPosition;

    private void Update()
    {
        if (Vector3.Distance(transform.position, targetPosition) > stoppingDistance)
        {
            unitAnimator.SetBool("IsWalking", true);

            Vector3 moveDirection = (targetPosition - transform.position).normalized;
            transform.position += moveDirection * moveSpeed * Time.deltaTime;
            transform.forward = Vector3.Lerp(transform.forward, moveDirection, Time.deltaTime * rotateSpeed);
        }
        else
            unitAnimator.SetBool("IsWalking", false);

        if (Input.GetMouseButtonDown(0))
            Move(MouseWorld.GetPosition());

    }

    private void Move(Vector3 _targetPosition)
    {
        this.targetPosition = _targetPosition;
    }

}