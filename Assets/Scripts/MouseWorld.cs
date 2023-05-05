using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private LayerMask Unit;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());
        Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, instance.Unit);
        return _rayCastHit.point;
    }

}