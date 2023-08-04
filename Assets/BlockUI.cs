using UnityEngine;
using UnityEngine.EventSystems;

public class BlockUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.IsHoveringOnUI(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.IsHoveringOnUI(false);
    }
}

