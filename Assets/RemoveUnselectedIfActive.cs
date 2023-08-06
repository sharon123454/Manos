using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RemoveUnselectedIfActive : MonoBehaviour
{
   [SerializeField] GameObject selectedAction;
    void Update()
    {
        selectedAction.SetActive(false);
    }
    private void OnDisable()
    {
        selectedAction.SetActive(true);
    }
}
