using System.Collections.Generic;
using UnityEngine;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private List<LayerMask> Unit;
    string[] layerNames = { "MousePlane", "Unit",};

    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

        Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, LayerMask.GetMask(instance.layerNames));
        return _rayCastHit.point;
    }

}