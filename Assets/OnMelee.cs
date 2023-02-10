using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnMelee : MonoBehaviour
{

    public GameObject MeleeVFX;

    public IEnumerator PlaySlashAnim()
    {
        MeleeVFX.SetActive(true);
        yield return new WaitForSeconds(2);
        MeleeVFX.SetActive(false);
    }
}
