using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class ActionBusyUI : MonoBehaviour
{
    private void Start()
    {
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        if (isBusy)
            Hide();
        else
            Show();
    }

    private void Show() { gameObject.SetActive(true); }

    private void Hide() { gameObject.SetActive(false); }

}