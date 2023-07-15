using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SPAWN_KNIFE : MonoBehaviour
{
    [SerializeField] private GameObject knife;
    private void Update()
    {
        if (ManosInputController.Instance.ShowAllHUD.inProgress)
        {
            spawnKnife();
        }
    }

    public void spawnKnife()
    {
        knife.transform.position = transform.position;
    }
}
