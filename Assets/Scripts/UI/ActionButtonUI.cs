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
    // [SerializeField] TextMeshProUGUI cooldownProUgui;
    [SerializeField] TextMeshProUGUI cooldownVisualProUgui;

    [SerializeField] GameObject actionSelected;
    [SerializeField] GameObject actionOutline;

    [SerializeField] GameObject bonusActionSelected;
    [SerializeField] GameObject bonusActionOutline;


    [SerializeField] GameObject OnCooldown;

    private BaseAbility isBaseAbility;
    private BaseAction baseAction;

    void Start()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        if (baseAction.GetIsBonusAction())
        {
            bonusActionOutline.SetActive(true);
            actionOutline.SetActive(false);
        }
        else
        {
            bonusActionOutline.SetActive(false);
            actionOutline.SetActive(true);
        }
    }

    IEnumerator test()
    {
        yield return new WaitForSeconds(0.05f);

    }
    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            UpdateSelectedVisual();
        }
    }

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

        Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();

        if (baseAction.GetIsBonusAction())
            bonusActionSelected.SetActive(selectedBaseAction == baseAction);
        else
            actionSelected.SetActive(selectedBaseAction == baseAction);

        if (selectedunit.GetStunStatus()
            || baseAction.GetIsBonusAction() && selectedunit.GetUsedBonusActionPoints()
            || !baseAction.GetIsBonusAction() && selectedunit.GetUsedActionPoints()
            /*|| baseAction is BaseAbility && MagicSystem.Instance.GetCurrentFavor() <= 0*/)
        {
            OnCooldown.SetActive(true);
            if (baseAction.GetCooldown() == 0)
                cooldownVisualProUgui.text = "";
            else
                cooldownVisualProUgui.text = baseAction.GetCooldown().ToString();

        }
        else if (baseAction.GetCooldown() > 0)
        {
            OnCooldown.SetActive(true);
            cooldownVisualProUgui.text = baseAction.GetCooldown().ToString();
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
            damageProUgui.gameObject.SetActive(true);
            postureProUgui.gameObject.SetActive(true);
            //   cooldownProUgui.gameObject.SetActive(true);
            if (!isBaseAbility)
            {
                isBaseAbility = (BaseAbility)baseAction;
                damageProUgui.text = $"Damage: {isBaseAbility.GetDamage()}";
                postureProUgui.text = $"Posture Damage: {isBaseAbility.GetPostureDamage()}";
                //cooldownProUgui.text = $"Cooldown : {baseAction.GetCooldown()} Turns";
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.savedAction);
        damageProUgui.gameObject.SetActive(false);
        postureProUgui.gameObject.SetActive(false);
        //cooldownProUgui.gameObject.SetActive(false);
    }
}