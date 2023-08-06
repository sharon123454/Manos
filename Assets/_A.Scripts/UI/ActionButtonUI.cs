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
    [SerializeField] TextMeshProUGUI abilityNameUgui;
    [SerializeField] TextMeshProUGUI descriptionUgui;
    [SerializeField] TextMeshProUGUI damageProUgui;
    [SerializeField] TextMeshProUGUI postureProUgui;
    [SerializeField] TextMeshProUGUI favorProUgui;
    [SerializeField] TextMeshProUGUI critHitChanceProUgui;
    [SerializeField] TextMeshProUGUI statusHitChanceProUgui;
    // [SerializeField] TextMeshProUGUI cooldownProUgui;
    [SerializeField] TextMeshProUGUI cooldownVisualProUgui;
    [SerializeField] Slider cooldownSlider;

    [SerializeField] GameObject actionSelected;
    [SerializeField] GameObject actionOutline;

    [SerializeField] GameObject bonusActionSelected;
    [SerializeField] GameObject bonusActionOutline;
    [SerializeField] GameObject rangedAction;
    [SerializeField] GameObject meleeAction;
    [SerializeField] GameObject favorAction;

    [SerializeField] GameObject OnCooldown;
    [SerializeField] Image abilityImage;

    private BaseAbility isBaseAbility;
    private BaseAction baseAction;
    [SerializeField] bool canUseAction = false;
    void Start()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        // UnitActionSystem.Instance.OnSelectedActionChanged += Instance_OnSelectedActionChanged;
        //if (baseAction.GetIsBonusAction())
        //{
        //    bonusActionOutline.SetActive(true);
        //    actionOutline.SetActive(false);
        //}
        //else
        //{
        //    bonusActionOutline.SetActive(false);
        //    actionOutline.SetActive(true);
        //}

        StartCoroutine(DelayStart());
    }
    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        if (!baseAction.GetUnit().IsEnemy() && baseAction != null)
        {
            UpdateButtonVisual();
        }
    }

    public BaseAction GetBaseAction() { return baseAction; }

    public void SetBaseAction(BaseAction baseAction)
    {
        this.baseAction = baseAction;
        abilityNameUgui.text = baseAction.GetActionName().ToUpper();
        if (baseAction.GetAbilityImage() != null)
            abilityImage.sprite = baseAction.GetAbilityImage();

        button.onClick.AddListener((/*anonymouseFunction*/) =>
        {
            UnitActionSystem.Instance.SetSelectedAction(baseAction);
            UnitActionSystem.Instance.savedAction = baseAction;
        });
        if (baseAction.GetFavorCost() > 0)
        {
            favorAction.SetActive(true);
        }
        if (baseAction.GetIsBonusAction())
            bonusActionSelected.SetActive(true);
        else
            actionSelected.SetActive(true);
        if (baseAction.ActionUsingBoth())
        {
            bonusActionSelected.SetActive(true);
            actionSelected.SetActive(true);
        }
        if (baseAction.GetRange() == ActionRange.Melee)
        {
            rangedAction.SetActive(false);
            meleeAction.SetActive(true);
        }
        else
        {
            rangedAction.SetActive(true);
            meleeAction.SetActive(false);
        }
    }

    public void UpdateButtonVisual()
    {
        if (baseAction && !baseAction.GetUnit().IsEnemy())
        {
            BaseAction selectedBaseAction = UnitActionSystem.Instance.GetSelectedAction();

            Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
            if (TurnSystem.Instance.IsPlayerTurn())
            {

                if (bonusActionSelected && baseAction.GetIsBonusAction())
                    bonusActionSelected.SetActive(selectedBaseAction == baseAction);
                else if (actionSelected)
                    actionSelected.SetActive(selectedBaseAction == baseAction);

                if (UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Silence) && !baseAction.GetAbilityPropertie().Contains(AbilityProperties.Basic)
                    || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Stun)
                    || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Root) && baseAction.GetRange() == ActionRange.Move
                    || baseAction.GetIsBonusAction() && selectedunit.GetUsedBonusActionPoints()
                    || !baseAction.GetIsBonusAction() && selectedunit.GetUsedActionPoints()
                    || !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(baseAction.GetFavorCost())
                    /*|| baseAction is BaseAbility && MagicSystem.Instance.GetCurrentFavor() <= 0*/)
                {
                    if (baseAction.GetCurrentCooldown() == 0)
                    {
                        if (cooldownVisualProUgui)
                            cooldownVisualProUgui.text = "";
                        if (OnCooldown)
                            OnCooldown.SetActive(false);
                        if (!baseAction.IsBasicAbility())
                            abilityImage.color = Color.white;
                    }
                    else
                    {
                        if (OnCooldown)
                            OnCooldown.SetActive(true);
                        if (cooldownVisualProUgui)
                            cooldownVisualProUgui.text = baseAction.GetCurrentCooldown().ToString();
                        cooldownSlider.maxValue = baseAction.GetAbilityCooldown();
                        cooldownSlider.value = baseAction.GetCurrentCooldown();
                        if (!baseAction.IsBasicAbility())
                            abilityImage.color = Color.gray;
                    }
                }

                if (baseAction.GetCurrentCooldown() == 0)
                {
                    if (!baseAction.IsBasicAbility())
                        abilityImage.color = Color.white;
                    if (cooldownVisualProUgui)
                        cooldownVisualProUgui.text = "";
                    if (OnCooldown)
                        OnCooldown.SetActive(false);
                }
                else
                {
                    if (OnCooldown)
                        OnCooldown.SetActive(true);
                    if (cooldownVisualProUgui)
                        cooldownVisualProUgui.text = baseAction.GetCurrentCooldown().ToString();
                    cooldownSlider.maxValue = baseAction.GetAbilityCooldown();
                    cooldownSlider.value = baseAction.GetCurrentCooldown();
                    if (!baseAction.IsBasicAbility())
                        abilityImage.color = Color.gray;
                }

            }
            if (!baseAction.IsBasicAbility())
            {
                if (baseAction.GetUnit().CanSpendActionPointsToTakeAction(baseAction))
                {
                    if (!baseAction.IsBasicAbility())
                        abilityImage.color = Color.gray;
                }
                // else { abilityImage.color = Color.white; }
            }
        }
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.IsHoveringOnUI(true);
        UnitActionSystem.Instance.SetSelectedAction(baseAction);
        if (baseAction.GetActionName() != "Basic Attack")
        {
            return;
        }
        actionInfo.SetActive(true);
        //if (!OnCooldown.activeInHierarchy && baseAction.GetFavorCost() <= MagicSystem.Instance.GetCurrentFavor())
        //else
        //    CursorManager.Instance.SetBlockableCursor();
        CursorManager.Instance.SetClickableCursor();

        if (baseAction is BaseAbility)
        {
            //   cooldownProUgui.gameObject.SetActive(true);
            if (!isBaseAbility)
            {
                isBaseAbility = (BaseAbility)baseAction;
                favorProUgui.gameObject.SetActive(true);
                damageProUgui.gameObject.SetActive(true);
                postureProUgui.gameObject.SetActive(true);
                critHitChanceProUgui.gameObject.SetActive(true);
                statusHitChanceProUgui.gameObject.SetActive(true);
                damageProUgui.text = $"{isBaseAbility.GetDamage()}";
                postureProUgui.text = $"{isBaseAbility.GetPostureDamage()}";
                favorProUgui.text = $"FAVOR - {isBaseAbility.GetFavorCost()}";
                descriptionUgui.text = $"{isBaseAbility.GetActionDescription()}";
                critHitChanceProUgui.text = $"{isBaseAbility.GetCritChance()}%";
                statusHitChanceProUgui.text = $"{isBaseAbility.GetStatusChance()}%";
                //cooldownProUgui.text = $"Cooldown : {baseAction.GetCooldown()} Turns";
            }
        }
        else
        {
            descriptionUgui.text = $"{baseAction.GetActionDescription()}";
            favorProUgui.gameObject.SetActive(false);
            damageProUgui.gameObject.SetActive(false);
            postureProUgui.gameObject.SetActive(false);
            critHitChanceProUgui.gameObject.SetActive(false);
            statusHitChanceProUgui.gameObject.SetActive(false);
        }

        if (baseAction.GetCurrentCooldown() == 0)
            cooldownVisualProUgui.text = "";
        else
        {
            OnCooldown.SetActive(true);
            cooldownVisualProUgui.text = baseAction.GetCurrentCooldown().ToString();
            cooldownSlider.maxValue = baseAction.GetAbilityCooldown();
            cooldownSlider.value = baseAction.GetCurrentCooldown();
        }

        if (baseAction is MoveAction) { return; }
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        CursorManager.Instance.SetDefaultCursor();
        UnitActionSystem.Instance.IsHoveringOnUI(false);
        actionInfo.SetActive(false);
        //cooldownProUgui.gameObject.SetActive(false);

        UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.savedAction);
        if (baseAction is MoveAction) { return; }

    }

    IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(1);
        UpdateButtonVisual();
    }

}