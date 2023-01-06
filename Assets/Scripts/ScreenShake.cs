using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using Cinemachine;

public class ScreenShake : MonoBehaviour
{
    private CinemachineImpulseSource cinemachineImpulseSource;

    private void Awake()
    {
        cinemachineImpulseSource = GetComponent<CinemachineImpulseSource>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            cinemachineImpulseSource.GenerateImpulse(10);
        }
    }

}