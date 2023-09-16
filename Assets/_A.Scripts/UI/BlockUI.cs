using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetHoveringOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetHoveringOnUI(false);
    }
}

