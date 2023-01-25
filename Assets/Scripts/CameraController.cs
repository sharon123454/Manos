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
    private const float MIN_FOLLOW_Y_OFFSET = 2f;
    private const float MAX_FOLLOW_Y_OFFSET = 12f;
    [SerializeField] private CinemachineVirtualCamera cinemachineVirtualCamera;
    [SerializeField] private float camMoveSpeed = 10f, camRotationSpeed = 100f, zoomAmount = 1f, zoomSpeed = 5f;
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
        Vector3 inputMoveDir = Vector3.zero;
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow) || Input.mousePosition.y >= Screen.height)
        {
            inputMoveDir.z += 1;
            StopAllCoroutines();
        }
        if (Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow) || Input.mousePosition.y <= 0)
        {
            inputMoveDir.z -= 1;
            StopAllCoroutines();
        }
        if (Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow) || Input.mousePosition.x <= 0)
        {
            inputMoveDir.x -= 1;
            StopAllCoroutines();
        }
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow) || Input.mousePosition.x >= Screen.width)
        {
            inputMoveDir.x += 1;
            StopAllCoroutines();
        }

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