using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class StoneHover : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
   [SerializeField] private GameObject hoverObject;
    public void OnPointerEnter(PointerEventData eventData)
    {
        hoverObject.SetActive(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        hoverObject.SetActive(false);
    }
}
