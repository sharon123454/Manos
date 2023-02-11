using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.UI.CanvasScaler;

public class UnitStatusEffects : MonoBehaviour
{
    [SerializeField] private int stunDuration;
    [SerializeField] private int ignoreArmorDuration;
    [SerializeField] private int rootDuration;
    [SerializeField] private int cowardPlagueDuration;
    [SerializeField] private int nullifyDuration;
    [SerializeField] private int healDuration;
    [SerializeField] private int gainArmorDuration;

    private Unit _unit;
    private UnitStats _stats;


    public List<StatusEffect> unitActiveStatusEffects;
    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        _unit = GetComponent<Unit>();
        _stats = GetComponent<UnitStats>();
    }

    private void Instance_OnTurnChange(object sender, EventArgs e)
    {
        GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();
        if (_unit.IsEnemy())
        {
            if (!TurnSystem.Instance.IsPlayerTurn())
            {
                foreach (var VARIABLE in unitActiveStatusEffects)
                {
                    switch (VARIABLE)
                    {
                        case StatusEffect.None:
                            break;
                        case StatusEffect.Stun:
                            stunDuration--;
                            break;
                        case StatusEffect.IgnoreArmor:
                            ignoreArmorDuration--;
                            break;
                        case StatusEffect.Root:
                            rootDuration--;
                            break;
                        case StatusEffect.CowardPlague:
                            cowardPlagueDuration--;
                            break;
                        case StatusEffect.Nullify:
                            nullifyDuration--;
                            break;
                        case StatusEffect.Heal:
                            healDuration--;
                            break;
                        case StatusEffect.GainArmor:
                            gainArmorDuration--;
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

    }

    public void AddStatusEffectToUnit(StatusEffect abilityEffect, int duration)
    {
        unitActiveStatusEffects.Add(abilityEffect);
        switch (abilityEffect)
        {
            case StatusEffect.None:
                break;
            case StatusEffect.Stun:
                stunDuration += duration;
                break;
            case StatusEffect.IgnoreArmor:
                ignoreArmorDuration += duration;
                break;
            case StatusEffect.Root:
                rootDuration += duration;
                break;
            case StatusEffect.CowardPlague:
                cowardPlagueDuration += duration;
                break;
            case StatusEffect.Nullify:
                nullifyDuration += duration;
                break;
            case StatusEffect.Heal:
                healDuration += duration;
                break;
            case StatusEffect.GainArmor:
                gainArmorDuration += duration;
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(abilityEffect), abilityEffect, null);
        }
    }
}
