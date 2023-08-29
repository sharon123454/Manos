using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionNameTMP;
    [SerializeField] private TextMeshProUGUI _actionCooldownTMP;
    [SerializeField] private TextMeshProUGUI _actionDescriptionTMP;
    [SerializeField] private GameObject _actionCostGroup;
    [SerializeField] private GameObject _bonusActionCostGroup;
    [SerializeField] private GameObject _actionFavorCostGroup;
    [SerializeField] private TextMeshProUGUI _actionFavorTMP;
    [SerializeField] private GameObject _actionTypeGroup;
    [SerializeField] private Image _actionTypeImage;
    [SerializeField] private TextMeshProUGUI _actionTypeTMP;
    [SerializeField] private GameObject _actionAttackGroup;
    [SerializeField] private TextMeshProUGUI _actionAccuracyTMP;
    [SerializeField] private TextMeshProUGUI _actionDamageTMP;
    [SerializeField] private TextMeshProUGUI _actionPostureTMP;
    [SerializeField] private TextMeshProUGUI _actionCritHitChanceTMP;

    public void UpdateInfoData(BaseAction baseAction)
    {
        if (_actionNameTMP)
            _actionNameTMP.text = baseAction.GetActionName().ToUpper();
        if (_actionDescriptionTMP)
            _actionDescriptionTMP.text = baseAction.GetActionDescription();
        if (_actionCooldownTMP)
            _actionCooldownTMP.text = baseAction.GetAbilityCooldown().ToString();

        if (baseAction.ActionUsingBoth())
        {
            if (_bonusActionCostGroup)
                _bonusActionCostGroup.SetActive(true);
            if (_actionCostGroup)
                _actionCostGroup.SetActive(true);
        }
        else if (baseAction.GetIsBonusAction())
        {
            if (_bonusActionCostGroup)
                _bonusActionCostGroup.SetActive(true);
            if (_actionCostGroup)
                _actionCostGroup.SetActive(false);
        }
        else
        {
            if (_bonusActionCostGroup)
                _bonusActionCostGroup.SetActive(false);
            if (_actionCostGroup)
                _actionCostGroup.SetActive(true);
        }

        if (_actionTypeTMP && baseAction.GetRange() == ActionRange.Melee)
        {
            _actionTypeTMP.text = "Melee";
            //_actionTypeImage.sprite = ;
        }
        else if (baseAction.IsBasicAbility())
        {

        }
        else
        {
            if (_actionTypeTMP)
                _actionTypeTMP.text = "Range";
            //_actionTypeImage.sprite = ;
        }

        if (_actionFavorCostGroup)
            _actionFavorCostGroup.SetActive(baseAction.GetFavorCost() > 0);

        //if(baseAction.IsBasicAbility() && baseAction.GetActionName() != "Basic Attack")

        if (_actionAttackGroup)
            if (baseAction is BaseAbility)
            {
                BaseAbility isBaseAbility = (BaseAbility)baseAction;
                _actionAttackGroup.SetActive(true);
                _actionAccuracyTMP.text = $"{isBaseAbility.GetAbilityHitChance()}%";
                _actionCritHitChanceTMP.text = $"{isBaseAbility.GetCritChance()}%";
                _actionPostureTMP.text = $"{isBaseAbility.GetPostureDamage()}";
                _actionDamageTMP.text = $"{isBaseAbility.GetDamage()}";

                if (_actionFavorCostGroup.activeSelf)
                    _actionFavorTMP.text = $"Favor - {isBaseAbility.GetFavorCost()}";
            }
            else
            {
                _actionAttackGroup.SetActive(false);
            }
    }

}