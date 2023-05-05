using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;
using UnityEngine.EventSystems;

public class UnitWorldUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private Image postureBarImage;
    [SerializeField] private Image actionBarImage;
    [SerializeField] private Image bonusActionBarImage;
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private TextMeshProUGUI armorPointsText;
    [SerializeField] private TextMeshProUGUI hitChanceText;
    [SerializeField] private TextMeshProUGUI healthVisual;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        unitStats.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    void Update()
    {
    }
    private void UpdateActionPointsText()
    {
        if (!unit.GetUsedActionPoints()) actionBarImage.fillAmount = 100;
        else actionBarImage.fillAmount = 0;

        if (!unit.GetUsedBonusActionPoints()) bonusActionBarImage.fillAmount = 100;
        else bonusActionBarImage.fillAmount = 0;
    }

    public void UpdateHealthBar()
    {
        healthBarImage.fillAmount = unitStats.GetHealthNormalized();
        postureBarImage.fillAmount = unitStats.GetPostureNormalized();
        armorPointsText.text = unitStats.GetArmor().ToString();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) { UpdateHealthBar(); }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) { UpdateActionPointsText(); }

    public void OnPointerEnter(PointerEventData eventData)
    {
        //HitChance == Evaition - Ability HitChance
        //Posture = 100 AbilityHitChance + Evaiton = 0

        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        var checkIfBaseAbility = UnitActionSystem.Instance.GetSelectedAction();
        float getHitChance = 0;
        healthVisual.gameObject.SetActive(true);
        healthVisual.text = $"{unit.GetUnitStats().health} / {unit.GetUnitStats().GetUnitMaxHP()}";

        if (unit.IsEnemy() && checkIfBaseAbility is BaseAbility)
        {
            getHitChance = UnitActionSystem.Instance.GetSelectedBaseAbility().GetAbilityHitChance();
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
                hitChanceText.text = $"hitChance = [{100}]%";
            else
            {
                switch (unit.GetGridPosition().ReturnRangeType())
                {
                    case Effectiveness.Effective:
                         hitChanceText.text = $"hitChance = [{Mathf.Clamp(getHitChance - unitStats.GetEvasion(), 0, 100)}]%";
                        break;
                    case Effectiveness.Inaccurate:
                        hitChanceText.text = $"hitChance = [{Mathf.Clamp(getHitChance - unitStats.GetEvasion() - 30, 0, 100)}]%";
                        break;
                    case Effectiveness.Miss:
                        hitChanceText.text = $"hitChance = [0]%";
                        break;
                    default:
                        hitChanceText.text = "Cant find Effectiveness";
                        break;

                }
               
            }

        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hitChanceText.gameObject.SetActive(false);
        healthVisual.gameObject.SetActive(false);
    }
}