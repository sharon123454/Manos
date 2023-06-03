using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    public static FollowMouse Instance;

    [SerializeField] private GameObject _DiagLinePrefab;
    [SerializeField] private GameObject _LinePrefab;
    [SerializeField] private LayerMask _GroundMask;
    [SerializeField] private Transform _LineParent;

    private List<GameObject> _diagLinePooler = new List<GameObject>();
    private List<GameObject> _linePooler = new List<GameObject>();

    private void Awake()
    {
        if (Instance && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            GameObject diagLine = Instantiate(_DiagLinePrefab, _LineParent);
            GameObject line = Instantiate(_LinePrefab, _LineParent);
            _diagLinePooler.Add(diagLine);
            _linePooler.Add(line);
        }
    }

    void Update()
    {
        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

        if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, _GroundMask))
        {
            transform.position = _rayCastHit.point;
            Debug.DrawRay(_ray.origin, _ray.direction * 10, Color.red, 0.01f);
        }
    }

    public void DrawLineOnPath(List<GridPosition> _pathGridPositionList)
    {
        for (int i = 0; i < _diagLinePooler.Count; i++)
        {
            _diagLinePooler[i].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
            _linePooler[i].transform.SetLocalPositionAndRotation(Vector3.zero, Quaternion.identity);
        }

        if (_pathGridPositionList.Count > 1)
            for (int i = 0; i < _pathGridPositionList.Count - 1; i++)
            {
                if (_pathGridPositionList[i] != null && _pathGridPositionList[i + 1] != null)
                {
                    GridPosition currentGrid = new GridPosition(_pathGridPositionList[i]._x, _pathGridPositionList[i]._z);
                    GridPosition nextGrid = new GridPosition(_pathGridPositionList[i + 1]._x, _pathGridPositionList[i + 1]._z);
                    Vector3 currentWorldPos = LevelGrid.Instance.GetWorldPosition(currentGrid);
                    Vector3 nextWorldPos = LevelGrid.Instance.GetWorldPosition(nextGrid);
                    Vector3 dir = nextWorldPos - currentWorldPos;

                    float rotateAngle = Mathf.Atan2(dir.x, dir.z) * Mathf.Rad2Deg;

                    Vector3 fixedPosition = new Vector3(currentWorldPos.x, 2.25f, currentWorldPos.z);

                    if (Vector3.Distance(currentWorldPos, nextWorldPos) > 2f)
                    {
                        _diagLinePooler[i].transform.position = fixedPosition;
                        _diagLinePooler[i].transform.rotation =
                            Quaternion.Euler(_diagLinePooler[i].transform.eulerAngles.x, rotateAngle, _diagLinePooler[i].transform.eulerAngles.z);
                    }
                    else
                    {
                        _linePooler[i].transform.position = fixedPosition;
                        _linePooler[i].transform.rotation =
                            Quaternion.Euler(_linePooler[i].transform.eulerAngles.x, rotateAngle, _linePooler[i].transform.eulerAngles.z);
                    }
                }
            }
    }

}