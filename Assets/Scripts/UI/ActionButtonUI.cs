using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button button;
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TextMeshProUGUI damageProUgui;
    [SerializeField] TextMeshProUGUI postureProUgui;
    [SerializeField] GameObject selectedGameObject;

    private BaseAbility isBaseAbility;
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
        if (baseAction is BaseAbility)
        {
            damageProUgui.gameObject.SetActive(true);
            postureProUgui.gameObject.SetActive(true);

            if (!isBaseAbility)
                isBaseAbility = (BaseAbility)baseAction;

            damageProUgui.text = $"Damage: {isBaseAbility.GetDamage()}";
            postureProUgui.text = $"Posture Damage: {isBaseAbility.GetPostureDamage()}";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        damageProUgui.gameObject.SetActive(false);
        postureProUgui.gameObject.SetActive(false);
    }
}