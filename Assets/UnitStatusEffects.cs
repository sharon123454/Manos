using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using static UnityEngine.UI.CanvasScaler;

public class UnitStatusEffects : MonoBehaviour
{


    [SerializeField] private int healValue;
    [SerializeField] private int amountOfArmorGain;
    [Space][Space][Space]
    
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
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChanged;
        _unit = GetComponent<Unit>();
        _stats = GetComponent<UnitStats>();
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();

        #region EnemyStatusEffects
        if (_unit.IsEnemy())
        {
            if (!TurnSystem.Instance.IsPlayerTurn())
            {
                for (int i = unitActiveStatusEffects.Count - 1; i >= 0; i--)
                {
                    switch (unitActiveStatusEffects[i])
                    {
                        case StatusEffect.None:
                            break;
                        case StatusEffect.Stun:
                            _unit.ChangeStunStatus(true);
                            stunDuration--;
                            if (stunDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                                _unit.ChangeStunStatus(false);
                            }
                            break;
                        case StatusEffect.IgnoreArmor:
                            ignoreArmorDuration--;
                            if (ignoreArmorDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Root:
                            rootDuration--;
                            if (rootDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.CowardPlague:
                            cowardPlagueDuration--;
                            if (cowardPlagueDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Nullify:
                            nullifyDuration--;
                            if (nullifyDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Heal:
                            healDuration--;
                            _stats.health += healValue;
                            if (healDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.GainArmor:
                            gainArmorDuration--;
                            _stats.Armor += amountOfArmorGain;
                            if (gainArmorDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }

        #endregion

        #region PlayerStatusEffects
        if (!_unit.IsEnemy())
        {
            if (TurnSystem.Instance.IsPlayerTurn())
            {
                for (int i = unitActiveStatusEffects.Count - 1; i >= 0; i--)
                {
                    switch (unitActiveStatusEffects[i])
                    {
                        case StatusEffect.None:
                            break;
                        case StatusEffect.Stun:
                            _unit.ChangeStunStatus(true);
                            stunDuration--;
                            if (stunDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                                _unit.ChangeStunStatus(false);
                            }
                            break;
                        case StatusEffect.IgnoreArmor:
                            ignoreArmorDuration--;
                            if (ignoreArmorDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Root:
                            rootDuration--;
                            if (rootDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.CowardPlague:
                            cowardPlagueDuration--;
                            if (cowardPlagueDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Nullify:
                            nullifyDuration--;
                            if (nullifyDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Heal:
                            healDuration--;
                            _stats.health += healValue;
                            if (healDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.GainArmor:
                            gainArmorDuration--;
                            _stats.Armor += amountOfArmorGain;
                            if (gainArmorDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        default:
                            throw new ArgumentOutOfRangeException();
                    }
                }
            }
        }


        #endregion
    }

    public void AddStatusEffectToUnit(StatusEffect abilityEffect, int duration)
    {
        unitActiveStatusEffects.Add(abilityEffect);
        //if (!unitActiveStatusEffects.Contains(abilityEffect))


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

    public bool ContainsEffect(StatusEffect effect) { if (unitActiveStatusEffects.Contains(effect)) return true; else return false; }
}
