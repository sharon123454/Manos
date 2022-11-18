using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float camMoveSpeed = 10f, camRotationSpeed = 100f, zoomAmount = 1f, zoomSpeed = 5f;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W))
            inputMoveDir.z += 1;
        if (Input.GetKey(KeyCode.S))
            inputMoveDir.z -= 1;
        if (Input.GetKey(KeyCode.A))
            inputMoveDir.x -= 1;
        if (Input.GetKey(KeyCode.D))
            inputMoveDir.x += 1;

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * camMoveSpeed * Time.deltaTime;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = Vector3.zero;
        if (Input.GetKey(KeyCode.Q))
            rotationVector.y += 1;
        if (Input.GetKey(KeyCode.E))
            rotationVector.y -= 1;

        transform.eulerAngles += rotationVector * camRotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()
    {
        if (Input.mouseScrollDelta.y > 0)
            targetFollowOffset.y += zoomAmount;
        if (Input.mouseScrollDelta.y < 0)
            targetFollowOffset.y -= zoomAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

        cinemachineTransposer.m_FollowOffset =
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);
    }

}