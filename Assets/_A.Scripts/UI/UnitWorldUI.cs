using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UnitWorldUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image healthTakenBarImage;
    [SerializeField] private Image postureBarImage;
    [SerializeField] private Image postureTakenBarImage;
    [SerializeField] private Image actionBarImage;
    [SerializeField] private Image bonusActionBarImage;
    [SerializeField] private Image critImage;
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private TextMeshProUGUI armorPointsText;
    [SerializeField] private TextMeshProUGUI hitChanceText;
    [SerializeField] private TextMeshProUGUI healthText;
    [SerializeField] private GameObject VisualParent;

    private Color actionBarDefualtColor;
    private Color BonusactionBarDefualtColor;
    private string thisUnitName;

    private void Start()
    {
        actionBarDefualtColor = actionBarImage.color;
        BonusactionBarDefualtColor = bonusActionBarImage.color;
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        unitStats.OnDamaged += HealthSystem_OnDamaged;
        unitStats.OnCriticalHit += UnitStats_OnCriticalHit;
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += Instance_OnSelectedActionChanged;
        AOEManager.OnAnyUnitEnteredAOE += AOEManager_OnAnyUnitEnteredAOE;
        AOEManager.OnAnyUnitExitedAOE += AOEManager_OnAnyUnitExitedAOE;
        thisUnitName = unit.name;
        UpdateActionPointsText();
        UpdateHealthBar();
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = unitStats.GetHealthNormalized();
        postureBarImage.fillAmount = unitStats.GetPostureNormalized();
        armorPointsText.text = unitStats.GetArmor().ToString();
        healthText.text = $"{unit.GetUnitStats().health} / {unit.GetUnitStats().GetUnitMaxHP()}";
    }

    private void UpdateActionPointsText()
    {
        if (!unit.GetUsedActionPoints()) actionBarImage.fillAmount = 100;
        else actionBarImage.fillAmount = 0;

        if (!unit.GetUsedBonusActionPoints()) bonusActionBarImage.fillAmount = 100;
        else bonusActionBarImage.fillAmount = 0;
    }
    private void ActivateWorldUI()
    {
        var checkIfBaseAbility = UnitActionSystem.Instance.GetSelectedAction();
        VisualParent.SetActive(true);
        healthText.text = $"{unit.GetUnitStats().health} / {unit.GetUnitStats().GetUnitMaxHP()}";

        if (unit.IsEnemy() && checkIfBaseAbility is BaseAbility)
        {
            if (checkIfBaseAbility.GetIfUsedAction() && checkIfBaseAbility.GetActionName() != "Move")
            {
                CursorManager.Instance.SetBlockableCursor();
                hitChanceText.text = $"hitChance = [{0}]%";
                return;
            }

            float getHitChance = UnitActionSystem.Instance.GetSelectedBaseAbility().GetAbilityHitChance();

            #region Old Switch
            //switch (checkIfBaseAbility.GetActionName())
            //{
            //    case "Shoot":
            //        getHitChance = selectedUnit.GetAction<ShootAction>().GetAbilityHitChance();
            //        break;   
            //    case "Basic":
            //        getHitChance = selectedUnit.GetAction<ShootAction>().GetAbilityHitChance();
            //        break;
            //    case "AoE":
            //        getHitChance = selectedUnit.GetAction<AOEAction>().GetAbilityHitChance();
            //        break;
            //    case "Melee":
            //        getHitChance = selectedUnit.GetAction<MeleeAction>().GetAbilityHitChance();
            //        break;
            //    case "Volley":
            //        getHitChance = selectedUnit.GetAction<ArrowVolleyAction>().GetAbilityHitChance();
            //        break;
            //    case "Stun":
            //        getHitChance = selectedUnit.GetAction<StunBolt>().GetAbilityHitChance();
            //        break;
            //    case "Murder":
            //        getHitChance = selectedUnit.GetAction<MurderAction>().GetAbilityHitChance();
            //        break;
            //    case "Root":
            //        getHitChance = selectedUnit.GetAction<BrakeALegAction>().GetAbilityHitChance();
            //        break;
            //    case "Puncture":
            //        getHitChance = selectedUnit.GetAction<PunctureAction>().GetAbilityHitChance();
            //        break;
            //    case "PommelStrike":
            //        getHitChance = selectedUnit.GetAction<PommelStrike>().GetAbilityHitChance();
            //        break;
            //}
            #endregion
            hitChanceText.gameObject.SetActive(true);
            if (unitStats.GetPosture() <= 0)
            {
                hitChanceText.text = $"hitChance = [{100}]%";
                CursorManager.Instance.SetOptimalCursor();
            }
            else
            {

                switch (unit.GetGridPosition().GetEffectiveRange())
                {
                    case Effectiveness.Effective:
                        hitChanceText.text = $"hitChance = [{Mathf.Clamp(getHitChance - unitStats.GetEvasion(), 0, 100)}]%";
                        CursorManager.Instance.SetOptimalCursor();
                        break;
                    case Effectiveness.Inaccurate:
                        hitChanceText.text = $"hitChance = [{Mathf.Clamp(getHitChance - unitStats.GetEvasion() - 30, 0, 100)}]%";
                        CursorManager.Instance.SetInaccurateCursor();
                        break;
                    case Effectiveness.Miss:
                        hitChanceText.text = $"hitChance = [0]%";
                        CursorManager.Instance.SetBlockableCursor();
                        break;
                    default:
                        hitChanceText.text = "Cant find Effectiveness";
                        break;
                }

            }
            if (getHitChance - unitStats.GetEvasion() > 0)
            {
                healthTakenBarImage.gameObject.SetActive(true);
                healthTakenBarImage.rectTransform.localPosition = new Vector3(unitStats.GetHealthNormalized() - 1, 0, 0);
                healthTakenBarImage.fillAmount = Math.Abs(unitStats.GetDamageTaken() - 1) + unitStats.GetHealthNormalized() - 1;

                postureTakenBarImage.rectTransform.localPosition = new Vector3(unitStats.GetPostureNormalized() - 1, 0, 0);
                postureTakenBarImage.gameObject.SetActive(true);
                postureTakenBarImage.fillAmount = Math.Abs(unitStats.GetPostureTaken() - 1) + unitStats.GetHealthNormalized() - 1;
            }
        }
        else
        {
            hitChanceText.text = "";
        }
    }
    private void DeActivateWorldUI()
    {
        CursorManager.Instance.SetDefaultCursor();
        if (UnitActionSystem.Instance.GetSelectedUnit().name != thisUnitName)
        {
            VisualParent.SetActive(false);
        }
        postureTakenBarImage.gameObject.SetActive(false);
        healthTakenBarImage.gameObject.SetActive(false);
    }

    IEnumerator ShowCriticalHitVisual()
    {
        critImage.gameObject.SetActive(true);
        yield return new WaitForSeconds(2);
        critImage.gameObject.SetActive(false);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (UnitActionSystem.Instance.GetSelectedBaseAbility() &&
            UnitActionSystem.Instance.GetSelectedBaseAbility().GetAbilityPropertie().Contains(AbilityProperties.AreaOfEffect))
            return;

        ActivateWorldUI();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (UnitActionSystem.Instance.GetSelectedBaseAbility() &&
            UnitActionSystem.Instance.GetSelectedBaseAbility().GetAbilityPropertie().Contains(AbilityProperties.AreaOfEffect))
            return;

        DeActivateWorldUI();
    }

    private void AOEManager_OnAnyUnitEnteredAOE(object sender, Unit enteredUnit)
    {
        if (unit == enteredUnit)
            ActivateWorldUI();
    }
    private void AOEManager_OnAnyUnitExitedAOE(object sender, Unit exitedUnit)
    {
        if (unit == exitedUnit)
            DeActivateWorldUI();
    }

    private void Instance_OnSelectedActionChanged(object sender, EventArgs e)
    {
        if (UnitActionSystem.Instance.GetSelectedUnit().name == thisUnitName)
        {
            if (UnitActionSystem.Instance.GetSelectedAction() != null)
            {
                if (UnitActionSystem.Instance.GetSelectedAction().ActionUsingBoth())
                {
                    bonusActionBarImage.color = Color.green;
                    actionBarImage.color = Color.green;
                }
                else if (UnitActionSystem.Instance.GetSelectedAction().GetIsBonusAction())
                {
                    bonusActionBarImage.color = Color.green;
                    actionBarImage.color = actionBarDefualtColor;
                }
                else
                {
                    actionBarImage.color = Color.green;
                    bonusActionBarImage.color = BonusactionBarDefualtColor;
                }
            }
        }
        else
        {
            actionBarImage.color = actionBarDefualtColor;
            bonusActionBarImage.color = BonusactionBarDefualtColor;
        }
    }
    private void Instance_OnSelectedUnitChanged(object sender, Unit newlySelectedUnit)
    {
        if (newlySelectedUnit.name == thisUnitName)
            VisualParent.SetActive(true);
        else
            VisualParent.SetActive(false);
    }

    private void UnitStats_OnCriticalHit(object sender, EventArgs e)
    {
        StartCoroutine(ShowCriticalHitVisual());
    }
    private void HealthSystem_OnDamaged(object sender, EventArgs e) { UpdateHealthBar(); }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) { UpdateActionPointsText(); }

}