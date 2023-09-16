using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitStatusEffects : MonoBehaviour
{
    public static event EventHandler<string> SendConsoleMessage;

    public event EventHandler<StatusEffect> OnStatusApplied;
    public event EventHandler<StatusEffect> OnStatusRemoved;

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
    [SerializeField] private int InvisibilityDuration;
    [SerializeField] private int TauntDuration;
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

    private void TurnSystem_OnTurnChanged(object sender, EventArgs e)
    {
        GetComponentInChildren<UnitWorldUI>().UpdateHealthBar();

        #region StatusEffects activations for all type Units at the end of their turn
        if (_unit.IsEnemy() && TurnSystem.Instance.IsPlayerTurn() ||
        !_unit.IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
        {
            for (int i = unitActiveStatusEffects.Count - 1; i >= 0; i--)
            {
                switch (unitActiveStatusEffects[i])
                {
                    case StatusEffect.None:
                        break;
                    case StatusEffect.Stun:
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
                        _unit.GetUnitStats().health += regenerationAmount;
                        break;
                    case StatusEffect.Corruption:
                        _unit.GetUnitStats().health -= curruptionDMG;
                        break;
                    case StatusEffect.Invisibility:
                        break;
                    case StatusEffect.Taunt:
                        break;
                    case StatusEffect.ToBeTauntUnused:
                    default:
                        Debug.Log($"{unitActiveStatusEffects[i]} effect isn't Implamented");
                        break;
                }

                DecreaseEffectCooldown(unitActiveStatusEffects[i]);
            }
        }
        #endregion

    }

    public int GetEffectDurationByEffect(StatusEffect effect)
    {
        switch (effect)
        {
            case StatusEffect.Stun:
                return stunDuration;
            case StatusEffect.Silence:
                return SilenceDuration;
            case StatusEffect.Root:
                return rootDuration;
            case StatusEffect.ArmorBreak:
                return armorBrakeDuration;
            case StatusEffect.GainArmor:
                return gainArmorDuration;
            case StatusEffect.Haste:
                return HasteDuration;
            case StatusEffect.Blind:
                return BlindDuration;
            case StatusEffect.Undying:
                return UndyingDuration;
            case StatusEffect.Regeneration:
                return RegenerationDuration;
            case StatusEffect.Corruption:
                return CorruptionDuration;
            case StatusEffect.CowardPlague:
                return cowardPlagueDuration;
            case StatusEffect.Nullify:
                return nullifyDuration;
            case StatusEffect.Invisibility:
                return InvisibilityDuration;
            case StatusEffect.Taunt:
                return TauntDuration;
            case StatusEffect.ToBeTauntUnused:
                return unusedDuration;
            case StatusEffect.None:
            default:
                Debug.Log($"effect: {effect}, is none or not implamented");
                return 0;
        }
    }
    public bool ContainsEffect(StatusEffect effect) { return unitActiveStatusEffects.Contains(effect); }
    public void AddStatusEffectToUnit(StatusEffect statusEffect, int duration)
    {
        switch (statusEffect)
        {
            case StatusEffect.Stun:
                if (!unitActiveStatusEffects.Contains(statusEffect)) //if effect active don't execute
                { _unit.SetStunStatus(true); } //function on adding effect

                stunDuration += duration;
                break;
            case StatusEffect.Silence:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                SilenceDuration += duration;
                break;
            case StatusEffect.ArmorBreak:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                armorBrakeDuration += duration;
                break;
            case StatusEffect.Root:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                rootDuration += duration;
                break;
            case StatusEffect.CowardPlague:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                cowardPlagueDuration += duration;
                break;
            case StatusEffect.Nullify:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                nullifyDuration += duration;
                break;
            case StatusEffect.GainArmor:
                if (!unitActiveStatusEffects.Contains(statusEffect))
                {
                    unitActiveStatusEffects.Add(statusEffect);
                    _stats.Armor += amountOfArmorGain;
                }

                gainArmorDuration += duration;
                break;
            case StatusEffect.Haste:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                HasteDuration += duration;
                break;
            case StatusEffect.Blind:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                BlindDuration += duration;
                break;
            case StatusEffect.Undying:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                UndyingDuration += duration;
                break;
            case StatusEffect.Regeneration:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                RegenerationDuration += duration;
                break;
            case StatusEffect.Corruption:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                CorruptionDuration += duration;
                break;
            case StatusEffect.Invisibility:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                InvisibilityDuration += duration;
                break;
            case StatusEffect.Taunt:
                if (!unitActiveStatusEffects.Contains(statusEffect)) { unitActiveStatusEffects.Add(statusEffect); }

                TauntDuration += duration;
                break;
            case StatusEffect.ToBeTauntUnused:
            case StatusEffect.None:
            default:
                Debug.Log($"{statusEffect} effect isn't Implamented");
                break;
        }

        OnStatusApplied?.Invoke(this, statusEffect);
        SendConsoleMessage?.Invoke(this, $"{statusEffect} was applied for {duration} turns.");
    }

    private void DecreaseEffectCooldown(StatusEffect effect)
    {
        List<StatusEffect> effectToRemoveList = new List<StatusEffect>();

        switch (effect)
        {
            case StatusEffect.None:
                break;
            case StatusEffect.Stun:
                if (stunDuration > 0)
                    stunDuration--;

                if (stunDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                    _unit.SetStunStatus(false);
                }
                break;
            case StatusEffect.Silence:
                if (SilenceDuration > 0)
                    SilenceDuration--;

                if (SilenceDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Root:
                if (rootDuration > 0)
                    rootDuration--;

                if (rootDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.ArmorBreak:
                if (armorBrakeDuration > 0)
                    armorBrakeDuration--;

                if (armorBrakeDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.GainArmor:
                if (gainArmorDuration > 0)
                    gainArmorDuration--;

                if (gainArmorDuration <= 0)
                {
                    _stats.Armor -= amountOfArmorGain;
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Haste:
                if (HasteDuration > 0)
                    HasteDuration--;

                if (HasteDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Blind:
                if (BlindDuration > 0)
                    BlindDuration--;

                if (BlindDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Undying:
                if (UndyingDuration > 0)
                    UndyingDuration--;

                if (UndyingDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Regeneration:
                if (RegenerationDuration > 0)
                    RegenerationDuration--;

                if (RegenerationDuration == 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Corruption:
                if (CorruptionDuration > 0)
                    CorruptionDuration--;

                if (CorruptionDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Nullify:
                if (nullifyDuration > 0)
                    nullifyDuration--;

                if (nullifyDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.CowardPlague:
                if (cowardPlagueDuration > 0)
                    cowardPlagueDuration--;

                if (cowardPlagueDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Invisibility:
                if (InvisibilityDuration > 0)
                    InvisibilityDuration--;

                if (InvisibilityDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.Taunt:
                if (TauntDuration > 0)
                    TauntDuration--;

                if (TauntDuration <= 0)
                {
                    effectToRemoveList.Add(effect);
                }
                break;
            case StatusEffect.ToBeTauntUnused:
            default:
                Debug.Log($"{effect} effect isn't Implamented");
                break;
        }

        if (effectToRemoveList.Count > 0)
            foreach (StatusEffect _effect in effectToRemoveList)
            {
                unitActiveStatusEffects.Remove(_effect);
                OnStatusRemoved?.Invoke(this, _effect);
            }
    }

}