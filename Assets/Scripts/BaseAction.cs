using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BaseAction : MonoBehaviour
{
    protected Unit unit;
    protected bool isActive;

    private void Awake()
    {
        unit = GetComponent<Unit>();
    }

}