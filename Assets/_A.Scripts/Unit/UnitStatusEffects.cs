using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitStatusEffects : MonoBehaviour
{
    [SerializeField] private int healValue;
    [SerializeField] private int amountOfArmorGain;
    [SerializeField] private int curruptionDMG;
    [SerializeField] private int regenerationAmount;
    [Space]
    [SerializeField] private int stunDuration;
    [SerializeField] private int armorBrakeDuration;
    [SerializeField] private int rootDuration;
    [SerializeField] private int cowardPlagueDuration;
    [SerializeField] private int nullifyDuration;
    [SerializeField] private int gainArmorDuration;
    [SerializeField] private int SilenceDuration;
    [SerializeField] private int HasteDuration;
    [SerializeField] private int BlindDuration;
    [SerializeField] private int UndyingDuration;
    [SerializeField] private int RegenerationDuration;
    [SerializeField] private int CorruptionDuration;
    [SerializeField] private int unusedDuration;

    public List<StatusEffect> unitActiveStatusEffects;

    private Unit _unit;
    private UnitStats _stats;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChanged;
        _unit = GetComponent<Unit>();
        _stats = GetComponent<UnitStats>();
    }

    public void ReduceCooldowns()
    {
        stunDuration--;
        armorBrakeDuration--;
        rootDuration--;
        cowardPlagueDuration--;
        nullifyDuration--;
        unusedDuration--;
        gainArmorDuration--;
        SilenceDuration--;
        HasteDuration--;
        BlindDuration--;
        UndyingDuration--;
        RegenerationDuration--;
        CorruptionDuration--;
    }

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        if (ContainsEffect(StatusEffect.Corruption))
        {
            _unit.GetUnitStats().health -= curruptionDMG;
        }
        if (ContainsEffect(StatusEffect.Regeneration))
        {
            _unit.GetUnitStats().health += regenerationAmount;
        }
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
                        case StatusEffect.ArmorBreak:
                            armorBrakeDuration--;
                            if (armorBrakeDuration == 0)
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
                        case StatusEffect.GainArmor:
                            gainArmorDuration--;
                            _stats.Armor += amountOfArmorGain;
                            if (gainArmorDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Silence:
                            SilenceDuration--;
                            if (SilenceDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Haste:
                            HasteDuration--;
                            if (HasteDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Blind:
                            BlindDuration--;
                            if (BlindDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Undying:
                            UndyingDuration--;
                            if (UndyingDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Regeneration:
                            RegenerationDuration--;
                            if (RegenerationDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.Corruption:
                            CorruptionDuration--;
                            if (CorruptionDuration == 0)
                            {
                                unitActiveStatusEffects.Remove(unitActiveStatusEffects[i]);
                            }
                            break;
                        case StatusEffect.ToBeTauntUnused:
                        default:
                            Debug.Log($"{unitActiveStatusEffects[i]} effect isn't Implamented");
                            break;
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
                            break;
                        case StatusEffect.ArmorBreak:
                            break;
                        case StatusEffect.Root:
                            break;
                        case StatusEffect.CowardPlague:
                            break;
                        case StatusEffect.Nullify:
                            break;
                        case StatusEffect.GainArmor:
                            break;
                        case StatusEffect.Silence:
                            break;
                        case StatusEffect.Haste:
                            break;
                        case StatusEffect.Blind:
                            break;
                        case StatusEffect.Undying:
                            break;
                        case StatusEffect.Regeneration:
                            break;
                        case StatusEffect.Corruption:
                            break;
                        case StatusEffect.ToBeTauntUnused:
                        default:
                            break;
                    }

                    DecreaseEffectDuration(unitActiveStatusEffects[i]);
                }
            }
        }
        #endregion
    }

    private void DecreaseEffectDuration(StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.None:
                break;
            case StatusEffect.Stun:
                stunDuration--;
                if (stunDuration <= 0)
                {
                    unitActiveStatusEffects.Remove(effect);
                    _unit.ChangeStunStatus(false);
                }
                break;
            case StatusEffect.Silence:
                SilenceDuration--;
                if (SilenceDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Root:
                rootDuration--;
                if (rootDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.ArmorBreak:
                armorBrakeDuration--;
                if (armorBrakeDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.GainArmor:
                gainArmorDuration--;
                _stats.Armor += amountOfArmorGain;
                if (gainArmorDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Haste:
                HasteDuration--;
                if (HasteDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Blind:
                BlindDuration--;
                if (BlindDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Undying:
                UndyingDuration--;
                if (UndyingDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Regeneration:
                RegenerationDuration--;
                if (RegenerationDuration == 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Corruption:
                CorruptionDuration--;
                if (CorruptionDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.Nullify:
                nullifyDuration--;
                if (nullifyDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.CowardPlague:
                cowardPlagueDuration--;
                if (cowardPlagueDuration <= 0)
                    unitActiveStatusEffects.Remove(effect);
                break;
            case StatusEffect.ToBeTauntUnused:
            default:
                Debug.Log($"{effect} effect isn't Implamented");
                break;
        }
    }

    public void AddStatusEffectToUnit(List<StatusEffect> abilityEffect, int duration)
    {
        foreach (var item in abilityEffect)
        {
            unitActiveStatusEffects.Add(item);
            switch (item)
            {
                case StatusEffect.None:
                    break;
                case StatusEffect.Stun:
                    stunDuration += duration;
                    break;
                case StatusEffect.Silence:
                    SilenceDuration += duration;
                    break;
                case StatusEffect.ArmorBreak:
                    armorBrakeDuration += duration;
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
                case StatusEffect.GainArmor:
                    gainArmorDuration += duration;
                    break;
                case StatusEffect.Haste:
                    HasteDuration += duration;
                    break;
                case StatusEffect.Blind:
                    BlindDuration += duration;
                    break;
                case StatusEffect.Undying:
                    UndyingDuration += duration;
                    break;
                case StatusEffect.Regeneration:
                    RegenerationDuration += duration;
                    break;
                case StatusEffect.Corruption:
                    CorruptionDuration += duration;
                    break;
                case StatusEffect.ToBeTauntUnused:
                default:
                    Debug.Log($"{abilityEffect} effect isn't Implamented");
                    break;
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
                case StatusEffect.Silence:
                    SilenceDuration += duration;
                    break;
                case StatusEffect.ArmorBreak:
                    armorBrakeDuration += duration;
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
                case StatusEffect.GainArmor:
                    gainArmorDuration += duration;
                    break;
                case StatusEffect.Haste:
                    HasteDuration += duration;
                    break;
                case StatusEffect.Blind:
                    BlindDuration += duration;
                    break;
                case StatusEffect.Undying:
                    UndyingDuration += duration;
                    break;
                case StatusEffect.Regeneration:
                    RegenerationDuration += duration;
                    break;
                case StatusEffect.Corruption:
                    CorruptionDuration += duration;
                    break;
                case StatusEffect.ToBeTauntUnused:
                default:
                    Debug.Log($"{abilityEffect} effect isn't Implamented");
                    break;
            }      
    }
    public bool ContainsEffect(StatusEffect effect) { return unitActiveStatusEffects.Contains(effect); }

}