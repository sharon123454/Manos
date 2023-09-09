using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionInfo : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI _actionNameTMP;
    [SerializeField] private TextMeshProUGUI _actionDescriptionTMP;
    [Header("Action prameters")]
    [SerializeField] private GameObject _actionCoooldownGroup;
    [SerializeField] private TextMeshProUGUI _actionCooldownTMP;
    [SerializeField] private GameObject _actionCostGroup;
    [SerializeField] private GameObject _bonusActionCostGroup;
    [SerializeField] private GameObject _actionFavorCostGroup;
    [SerializeField] private TextMeshProUGUI _actionFavorTMP;
    [SerializeField] private GameObject _actionTypeGroup;
    [SerializeField] private TextMeshProUGUI _actionTypeTMP;
    [SerializeField] private Image _actionTypeImage;
    [SerializeField] private Sprite _actionMeleeImage;
    [SerializeField] private Sprite _actionRangeImage;
    [SerializeField] private GameObject _actionAttackGroup;
    [SerializeField] private TextMeshProUGUI _actionAccuracyTMP;
    [SerializeField] private TextMeshProUGUI _actionDamageTMP;
    [SerializeField] private TextMeshProUGUI _actionPostureTMP;
    [SerializeField] private TextMeshProUGUI _actionCritHitChanceTMP;

    //need more work - displays everything to UI
    public void UpdateInfoData(BaseAction baseAction)
    {
        //Set fresh data
        if (_actionNameTMP)
            _actionNameTMP.text = baseAction.GetActionName();
        if (_actionDescriptionTMP)
            _actionDescriptionTMP.text = baseAction.GetActionDescription();

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

        if (_actionTypeGroup)
        {
            _actionTypeGroup.SetActive(true);
            if (_actionTypeTMP && baseAction.GetRange() == ActionRange.Melee)
            {
                _actionTypeTMP.text = "Melee";
                _actionTypeImage.sprite = _actionMeleeImage;
            }
            else if (baseAction.IsBasicAbility() && _actionNameTMP.text != "Basic Attack")
            {
                _actionTypeGroup.SetActive(false);
            }
            else
            {
                if (_actionTypeTMP)
                {
                    _actionTypeTMP.text = "Range";
                    _actionTypeImage.sprite = _actionRangeImage;
                }
            }
        }

        if (_actionFavorCostGroup)
            _actionFavorCostGroup.SetActive(baseAction.GetFavorCost() > 0);

        //if(baseAction.IsBasicAbility() && baseAction.GetActionName() != "Basic Attack")

        if (_actionAttackGroup)
            if (baseAction is BaseAbility)
            {
                BaseAbility isBaseAbility = (BaseAbility)baseAction;
                _actionAttackGroup.SetActive(true);
                _actionCoooldownGroup.SetActive(true);
                _actionCooldownTMP.text = isBaseAbility.GetAbilityCooldown().ToString();
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
                _actionCoooldownGroup.SetActive(false);
            }
    }

}