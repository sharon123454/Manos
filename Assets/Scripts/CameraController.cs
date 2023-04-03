using System.Collections.Generic;
using System.Collections;
using Cinemachine;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float camMoveSpeed = 10f, camRotationSpeed = 100f, zoomAmount = 1f, zoomSpeed = 5f;
    [Range(1, 10)]
    [SerializeField] private float camOrbitSpeed = 1f;
    [SerializeField] float MIN_ZOOM = 2f, MAX_ZOOM = 12f;
    [SerializeField] float lerpDistanceFromUnit = 1.5f, lerpSpeed = 2.5f;

    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 moveVector = ManosInputController.Instance.GetMoveDirection(transform);

        if (moveVector == Vector3.zero)
            return;

        StopAllCoroutines();

        transform.position += camMoveSpeed * Time.deltaTime * moveVector;
    }

    private void HandleRotation()
    {
        Vector3 rotationVector = ManosInputController.Instance.GetRotateCamBy();
        Vector3 pointerDelta = ManosInputController.Instance.PointerDelta.ReadValue<Vector2>();

        if (ManosInputController.Instance.RotateRight.inProgress && pointerDelta.x < 0)
            rotationVector.y -= camOrbitSpeed;
        if (ManosInputController.Instance.RotateLeft.inProgress && pointerDelta.x > 0)
            rotationVector.y += camOrbitSpeed;

        if (rotationVector == Vector3.zero)
            return;

        transform.eulerAngles += camRotationSpeed * Time.deltaTime * rotationVector;
    }

    private void HandleZoom()//need to fix with new input sys
    {
        //if (Input.mouseScrollDelta.y > 0)
        //    targetFollowOffset.y += zoomAmount;
        //if (Input.mouseScrollDelta.y < 0)
        //    targetFollowOffset.y -= zoomAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_ZOOM, MAX_ZOOM);

        cinemachineTransposer.m_FollowOffset =
            Vector3.Lerp(cinemachineTransposer.m_FollowOffset, targetFollowOffset, Time.deltaTime * zoomSpeed);

        //if (componentBase == null)
        //    componentBase = cinemachineVirtualCamera.GetCinemachineComponent(CinemachineCore.Stage.Body);

        //if (Input.GetAxis("Mouse ScrollWheel") != 0)
        //{
        //    cameraDistance -= Input.GetAxis("Mouse ScrollWheel") * zoomSpeed;

        //    if (componentBase is CinemachineFramingTransposer)
        //    {
        //        framingTransposer = (CinemachineFramingTransposer)componentBase;
        //        framingTransposer.m_CameraDistance -= cameraDistance;
        //    }
        //}
    }

    private IEnumerator LerpToUnit(Vector3 unitPos)
    {
        while (Vector3.Distance(transform.position, unitPos) > lerpDistanceFromUnit)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(unitPos.x, transform.position.y, unitPos.z), lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }

    private void Instance_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        StartCoroutine(LerpToUnit(UnitActionSystem.Instance.GetSelectedUnit().transform.position));
    }

}