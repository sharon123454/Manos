using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] Button button;
    [SerializeField] GameObject actionInfo;
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] TextMeshProUGUI descriptionUgui;
    [SerializeField] TextMeshProUGUI damageProUgui;
    [SerializeField] TextMeshProUGUI postureProUgui;
    [SerializeField] TextMeshProUGUI favorProUgui;
    [SerializeField] TextMeshProUGUI critHitChanceProUgui;
    [SerializeField] TextMeshProUGUI statusHitChanceProUgui;
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
        UpdateSelectedVisual();
    }

    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        UpdateSelectedVisual();
    }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        textMeshPro.text = baseAction.GetActionName().ToUpper();

        button.onClick.AddListener((/*anonymouseFunction*/) =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
            UnitActionSystem.Instance.savedAction = baseAction;
        });
    }

    public void UpdateSelectedVisual()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

            Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();

            if (bonusActionSelected && baseAction.GetIsBonusAction())
                bonusActionSelected.SetActive(selectedBaseAction == baseAction);
            else if (actionSelected)
                actionSelected.SetActive(selectedBaseAction == baseAction);

            if (UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Silence) && !baseAction.GetAbilityPropertie().Contains(AbilityProperties.Basic)
                || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Stun)
                || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Root) && baseAction.GetRange() == AbilityRange.Move
                || baseAction.GetIsBonusAction() && selectedunit.GetUsedBonusActionPoints()
                || !baseAction.GetIsBonusAction() && selectedunit.GetUsedActionPoints()
                || !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost())
                /*|| baseAction is BaseAbility && MagicSystem.Instance.GetCurrentFavor() <= 0*/)
            {
                if (OnCooldown)
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
                if (OnCooldown)
                    OnCooldown.SetActive(false);
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.IsHoveringOnUI(true);
        if (baseAction is MoveAction) { return; }

        UnitActionSystem.Instance.SetSelectedAction(baseAction);
        if (baseAction is BaseAbility)
        {
            actionInfo.SetActive(true);
            //   cooldownProUgui.gameObject.SetActive(true);
            if (!isBaseAbility)
            {
                isBaseAbility = (BaseAbility)baseAction;
                damageProUgui.text = $"Damage: {isBaseAbility.GetDamage()}";
                postureProUgui.text = $"Posture Damage: {isBaseAbility.GetPostureDamage()}";
                favorProUgui.text = $"Favor Cost: {isBaseAbility.GetFavorCost()}";
                descriptionUgui.text = $"{isBaseAbility.GetActionDescription()}";
                critHitChanceProUgui.text = $"Crit Chance: {isBaseAbility.GetCritChance()}%";
                statusHitChanceProUgui.text = $"Status Chance: {isBaseAbility.GetStatusChance()}%";
                //cooldownProUgui.text = $"Cooldown : {baseAction.GetCooldown()} Turns";
            }
        }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.IsHoveringOnUI(false);
        if (baseAction is MoveAction) { return; }

        UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.savedAction);
        actionInfo.SetActive(false);
        //cooldownProUgui.gameObject.SetActive(false);
    }

}