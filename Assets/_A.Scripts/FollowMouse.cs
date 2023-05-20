using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class FollowMouse : MonoBehaviour
{
    [SerializeField] LayerMask groundMask;

    void Update()
    {
        Ray _ray = Camera.main.ScreenPointToRay(ManosInputController.Instance.GetPointerPosition());

        if (Physics.Raycast(_ray, out RaycastHit _rayCastHit, float.MaxValue, groundMask))
        {
            transform.position = _rayCastHit.point;
            Debug.DrawRay(_ray.origin, _ray.direction * 10, Color.red,0.01f);
        }
    }

}