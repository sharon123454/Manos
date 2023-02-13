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
    [SerializeField] TextMeshProUGUI cooldownProUgui;
    [SerializeField] GameObject selectedGameObject;
    [SerializeField] GameObject OnCooldown;

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
                UnitActionSystem.Instance.savedAction = baseAction;
            });
    }

    public void UpdateSelectedVisual()
    {
        BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();
        selectedGameObject.SetActive(selectedBaseAction == baseAction);
        if (baseAction.GetCooldown() > 0)
        {
            OnCooldown.SetActive(true);
        }
        else
        {
            OnCooldown.SetActive(false);
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetSelectedAction(baseAction);
        if (baseAction is BaseAbility)
        {
            //if (isBaseAbility.GetCooldown() > 0)
            //{
            //    return;
            //}
            damageProUgui.gameObject.SetActive(true);
            postureProUgui.gameObject.SetActive(true);
            cooldownProUgui.gameObject.SetActive(true);
            if (!isBaseAbility)
            {
                isBaseAbility = (BaseAbility)baseAction;
                damageProUgui.text = $"Damage: {isBaseAbility.GetDamage()}";
                postureProUgui.text = $"Posture Damage: {isBaseAbility.GetPostureDamage()}";
                cooldownProUgui.text = $"Cooldown : {baseAction.GetCooldown()} Turns";
            }
        }
        else
        {
            //if (baseAction.GetCooldown() > 0)
            //{
            //    return;
            //}
            cooldownProUgui.gameObject.SetActive(true);
            cooldownProUgui.text = $"Cooldown : {baseAction.GetCooldown()} Turns";
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.savedAction);
        damageProUgui.gameObject.SetActive(false);
        postureProUgui.gameObject.SetActive(false);
        cooldownProUgui.gameObject.SetActive(false);
    }
}