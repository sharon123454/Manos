using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public abstract class BaseAction : MonoBehaviour
{
    protected Action onActionComplete;
    protected bool isActive;
    protected Unit unit;

    protected virtual void Awake() { unit = GetComponent<Unit>(); }

}