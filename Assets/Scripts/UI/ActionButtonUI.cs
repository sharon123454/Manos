using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TextMeshProUGUI damageProUgui;
    [SerializeField] TextMeshProUGUI postureProUgui;
    [SerializeField] GameObject selectedGameObject;

    private BaseAction baseAction;

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener(
            (/*anonymouseFunction*/) =>
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        damageProUgui.gameObject.SetActive(true);
        postureProUgui.gameObject.SetActive(true);

        if (baseAction is BaseAbility)
        {
                     damageProUgui.text = "Damage: DMG HERE";
            postureProUgui.text = "Posture Damage: DMG HERE";
        }

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        damageProUgui.gameObject.SetActive(false);
        postureProUgui.gameObject.SetActive(false);
    }
}