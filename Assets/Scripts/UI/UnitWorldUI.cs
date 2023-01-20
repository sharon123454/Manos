using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UnitWorldUI : MonoBehaviour
{
    [SerializeField] private Unit unit;
    [SerializeField] private Image healthBarImage;
    [SerializeField] private UnitStats unitStats;
    [SerializeField] private TextMeshProUGUI actionPointsText;
    [SerializeField] private TextMeshProUGUI BonusActionPointsText;

    private void Start()
    {
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;
        unitStats.OnDamaged += HealthSystem_OnDamaged;

        UpdateActionPointsText();
        UpdateHealthBar();
    }

    private void UpdateActionPointsText()
    {
        actionPointsText.text = $"AP[{unit.GetActionPoints()}]";
        BonusActionPointsText.text = $"BAP[{unit.GetBonusActionPoints()}]";
    }

    private void UpdateHealthBar()
    {
        healthBarImage.fillAmount = unitStats.GetHealthNormalized();
    }

    private void HealthSystem_OnDamaged(object sender, EventArgs e) { UpdateHealthBar(); }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) { UpdateActionPointsText(); }

}