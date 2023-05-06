using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MouseWorld : MonoBehaviour
{
    private static MouseWorld instance;
    [SerializeField] private LayerMask mousePlaneLayerMask;
    [SerializeField] private List<LayerMask> Unit;
    private void Awake()
    {
        if (instance != null && instance != this)
            Destroy(gameObject);

        instance = this;
    }

    public static Vector3 GetPosition()
    {
        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

        Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, instance.mousePlaneLayerMask);
        return _rayCastHit.point;
        //if (_rayCastHit.transform.gameObject.layer == instance.Unit[1])
        //{
        //    return _rayCastHit.point;
        //}
        //else
        //{
        //    Physics.Raycast(_ray, out RaycastHit rayCastHit, float.MaxValue, instance.Unit[1]);
        //    return rayCastHit.point;
        //}
    }

}