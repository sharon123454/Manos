using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray _ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return _rayCastHit.point;
    }

}