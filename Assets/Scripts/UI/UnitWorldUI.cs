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
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI BonusActionPointsText;
    [SerializeField] private TextMeshProUGUI hitChanceText;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        unitStats.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        //actionPointsText.text = $"AP[{unit.GetActionPoints()}]";
        if (unit.GetActionPoints() >= 1)
            actionBarImage.fillAmount = 100;
        else
            actionBarImage.fillAmount = 0;    
        /////////////////////////////////////
        if (unit.GetBonusActionPoints() >= 1)
            bonusActionBarImage.fillAmount = 100;
        else
            bonusActionBarImage.fillAmount = 0;

      //  BonusActionPointsText.text = $"BAP[{unit.GetBonusActionPoints()}]";
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = unitStats.GetHealthNormalized();
        postureBarImage.fillAmount = unitStats.GetPostureNormalized();
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

        if (unit.IsEnemy() && checkIfBaseAbility is BaseAbility)
        {
            switch (checkIfBaseAbility.GetActionName())
            {
                case "Shoot":
                    getHitChance = selectedUnit.GetAction<ShootAction>().GetAbilityHitChance();
                    break;
                case "AoE":
                    getHitChance = selectedUnit.GetAction<AOEAction>().GetAbilityHitChance();
                    break;
                case "Melee":
                    getHitChance = selectedUnit.GetAction<AOEAction>().GetAbilityHitChance();
                    break;
            }
            hitChanceText.gameObject.SetActive(true);
            if (unitStats.GetPosture() <= 0)
                hitChanceText.text = $"hitChance = [{100}]%";
            else
                hitChanceText.text = $"hitChance = [{Mathf.Clamp(getHitChance - unitStats.GetEvasion(), 0, 100)}]%";

        }
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        hitChanceText.gameObject.SetActive(false);
    }
}