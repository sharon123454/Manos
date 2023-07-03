using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using Cinemachine;
using UnityEngine;
using System;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance { get; private set; }

    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float camMoveSpeed = 10f, camRotationSpeed = 100f;
    [Range(1, 10)]
    [SerializeField] private float camOrbitSpeed = 1f;
    [SerializeField] private float MIN_ZOOM = -4f, MAX_ZOOM = 5f;
    [SerializeField] private float zoomAmount = 1f, zoomSpeed = 5f, zoomDampening = 1.5f;
    [SerializeField] private float lerpDistanceFromUnit = 1.5f, lerpSpeed = 2.5f;
    [SerializeField] private bool useScreenEdge = true;
    [Range(0f, 1f)]
    [SerializeField] private float edgePercentageToMove = 0.05f;

    private float _zoomHeight;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void OnEnable()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        ManosInputController.Instance.Zoom.performed += ManosInputController_OnZoomChanged;
        _zoomHeight = transform.localPosition.y;
    }

    void Update()
    {
        UpdateMovement();
        UpdateRotation();
        UpdateZoom();

        if (useScreenEdge)
            CheckMouseAtScreenEdge();
    }

    private void OnDisable()
    {
        ManosInputController.Instance.Zoom.performed -= ManosInputController_OnZoomChanged;
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
    }

    private void CheckMouseAtScreenEdge()
    {
        Vector2 mousePos = ManosInputController.Instance.GetPointerPosition();
        Vector3 moveVector = Vector3.zero;

        if (mousePos.x < edgePercentageToMove * Screen.width)
            moveVector -= transform.right;
        else if (mousePos.x > (1 - edgePercentageToMove) * Screen.width)
            moveVector += transform.right;

        if (mousePos.y < edgePercentageToMove * Screen.height)
            moveVector -= transform.forward;
        else if (mousePos.y > (1 - edgePercentageToMove) * Screen.height)
            moveVector += transform.forward;

        if (moveVector == Vector3.zero)
            return;

        transform.position += camMoveSpeed * Time.deltaTime * moveVector;
    }

    private void UpdateMovement()
    {
        Vector3 moveVector = ManosInputController.Instance.GetMoveDirection(transform);

        if (moveVector == Vector3.zero)
            return;

        StopAllCoroutines();
        transform.position += camMoveSpeed * Time.deltaTime * moveVector;
    }

    private void UpdateRotation()
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

    private void UpdateZoom()
    {
        if (transform.localPosition.y == _zoomHeight)
            return;

        Vector3 zoomTarget = new Vector3(transform.localPosition.x, _zoomHeight, transform.localPosition.z);
        zoomTarget -= zoomSpeed * (_zoomHeight - transform.localPosition.y) * cinemachineVirtualCamera.transform.forward.normalized;

        transform.localPosition = Vector3.Lerp(transform.localPosition, zoomTarget, Time.deltaTime * zoomDampening);
    }

    private void ManosInputController_OnZoomChanged(InputAction.CallbackContext inputValue)
    {
        float value = -inputValue.ReadValue<Vector2>().y / 120;//input is 120 so divided to get 1

        if (Mathf.Abs(value) > 0.1f)
        {
            _zoomHeight += value * zoomAmount;
            _zoomHeight = Mathf.Clamp(_zoomHeight, MIN_ZOOM, MAX_ZOOM);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        StartCoroutine(LerpToUnit(UnitActionSystem.Instance.GetSelectedUnit().transform.position));
    }

    private IEnumerator LerpToUnit(Vector3 unitPos)
    {
        while (MathF.Abs(Vector3.Distance(transform.position, unitPos)) > lerpDistanceFromUnit)
        {
            transform.position = Vector3.Lerp(transform.position, new Vector3(unitPos.x, transform.position.y, unitPos.z), lerpSpeed * Time.deltaTime);
            yield return null;
        }
    }

}