using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestoryOnActive : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(DestoryAfterActive());
    }

    IEnumerator DestoryAfterActive()
    {
        yield return new WaitForSeconds(0.5f);

        Destroy(gameObject);
    }
}
