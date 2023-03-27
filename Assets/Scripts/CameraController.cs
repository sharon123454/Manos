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
    [SerializeField] float MIN_FOLLOW_Y_OFFSET = 2f;
    [SerializeField] float MAX_FOLLOW_Y_OFFSET = 12f;

    private IEnumerator cameraLerp;
    private CinemachineTransposer cinemachineTransposer;
    private Vector3 targetFollowOffset;

    private void Start()
    {
        //   cameraLerp = LerpToUnit();
        cinemachineTransposer = cinemachineVirtualCamera.GetCinemachineComponent<CinemachineTransposer>();
        targetFollowOffset = cinemachineTransposer.m_FollowOffset;
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
    }

    private void Instance_OnSelectedUnitChanged(object sender, System.EventArgs e)
    {
        StopAllCoroutines();
        StartCoroutine(LerpToUnit(UnitActionSystem.Instance.GetSelectedUnit().transform.position));
    }

    public IEnumerator LerpToUnit(Vector3 unitPos)
    {
        float t = 0;
        float duration = 2.5f;
        while (t < duration)
        {
            t += Time.deltaTime / duration;
            transform.position = Vector3.Lerp(transform.position, new Vector3(unitPos.x, transform.position.y, unitPos.z), t / duration);

            yield return null;
        }
    }

    void Update()
    {
        HandleMovement();
        HandleRotation();
        HandleZoom();
    }

    private void HandleMovement()
    {
        Vector3 inputMoveDir = ManosInputController.Instance.GetMoveDirection();

        Vector3 moveVector = transform.forward * inputMoveDir.z + transform.right * inputMoveDir.x;
        transform.position += moveVector * camMoveSpeed * Time.deltaTime;
    }

    private void HandleRotation()//need to fix with new input sys
    {
        Vector3 rotationVector = ManosInputController.Instance.RotateCamBy();
        //if (Input.GetKey(KeyCode.Q))
        //    rotationVector.y += 1;
        //if (Input.GetKey(KeyCode.E))
        //    rotationVector.y -= 1;
        //if (Input.GetMouseButton(1) && Input.GetAxis("Mouse X") > 0)
        //    rotationVector.y -= camOrbitSpeed;
        //if (Input.GetMouseButton(1) && Input.GetAxis("Mouse X") < 0)
        //    rotationVector.y += camOrbitSpeed;

        transform.eulerAngles += rotationVector * camRotationSpeed * Time.deltaTime;
    }

    private void HandleZoom()//need to fix with new input sys
    {
        //if (Input.mouseScrollDelta.y > 0)
        //    targetFollowOffset.y += zoomAmount;
        //if (Input.mouseScrollDelta.y < 0)
        //    targetFollowOffset.y -= zoomAmount;

        targetFollowOffset.y = Mathf.Clamp(targetFollowOffset.y, MIN_FOLLOW_Y_OFFSET, MAX_FOLLOW_Y_OFFSET);

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

}