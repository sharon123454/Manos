using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BasicActionSystemUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> basicImageSprites;
    public void SelectBasicAbility(GameObject currentAbility)
    {
        foreach (var sprite in basicImageSprites)
        {
            sprite.SetActive(false);
        }
        currentAbility.SetActive(true);
    }
}
