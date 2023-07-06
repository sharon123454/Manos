using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitStats : MonoBehaviour
{
    public static event EventHandler<string> SendConsoleMessage;

    public event EventHandler OnDeath;
    public event EventHandler OnDodge;
    public event EventHandler OnHeal;
    public event EventHandler OnDamaged;
    public event EventHandler OnCriticalHit;
    private Unit _unit;
    private UnitStatusEffects _unitStatusEffect;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxPosture = 100;
    [SerializeField] private float evasion = 20;

    [HideInInspector] public float health;
    public float Armor = 0;
    private float currentPosture;
    private int armorMultiplayer = 1;
    private int evasionMultiplayer = 1;
    private int _Effectivness = 0;
    private float postureDMGMultiplayer = 1;

    private Effectiveness GetEffectiveness => _unit.GetGridEffectiveness();
    private int CurrentEffectiveness
    {
        get
        {
            switch (GetEffectiveness)
            {
                case Effectiveness.Effective:
                    _Effectivness = 0;
                    break;
                case Effectiveness.Inaccurate:
                    _Effectivness = 30;
                    break;
                case Effectiveness.Miss:
                    _Effectivness = 200;
                    break;
                default:
                    _Effectivness = 50;
                    break;
            }
            return _Effectivness;
        }
    }

    private void Awake()
    {
        currentPosture = maxPosture;
        health = maxHealth;
    }

    private void Start()
    {
        _unit = GetComponent<Unit>();
        _unitStatusEffect = GetComponent<UnitStatusEffects>();
    }

    private void Update()
    {
        //   print(GetEffectiveness + " "+ _unit.name);
    }

    public float GetHealthNormalized() { return health / maxHealth; }

    public float GetPostureNormalized() { return currentPosture / maxPosture; }

    public void ReduceStatusEffectCooldowns()
    {
        _unitStatusEffect.ReduceCooldowns();
    }

    public void ReduceAbilityCooldowns()
    {

    }

    public float GetUnitMaxHP() { return maxHealth; }
    public void Block()
    {
        armorMultiplayer = 2;
        postureDMGMultiplayer = 0.5f;
    }

    public void Dodge()
    {
        evasionMultiplayer = 2;
    }

    public float GetEvasion() { return evasion; }
    public float GetPosture() { return currentPosture; }
    public float GetArmor() { return Armor; }
    public void ResetUnitStats()
    {
        if (currentPosture <= 0)
            currentPosture = maxPosture;
        else
            currentPosture += maxPosture * 0.2f;

        armorMultiplayer = 1;
        evasionMultiplayer = 1;
        postureDMGMultiplayer = 1f;
    }

    public void RemovePosture(float postureDamage)
    {
        currentPosture -= postureDamage;
    }
    public void TryTakeDamage(float rawDamage, float postureDamage, float hitChance, float abilityCritChance, List<StatusEffect> currentEffects, List<AbilityProperties> AP, int chanceToTakeStatusEffect, int effectDuration)
    {
        #region Dice Rools
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        int critDiceRoll = UnityEngine.Random.Range(0, 101);
        #endregion

        #region Damage To Recieve Types
        float damageToRecieve;

        if (AP.Contains(AbilityProperties.Heal))
        {
            Heal(rawDamage);
            return;
        }
        if (_unitStatusEffect.ContainsEffect(StatusEffect.Blind))
        {
            hitChance /= 2;
        }
        if (AP.Contains(AbilityProperties.IgnoreArmor))
        {
            damageToRecieve = rawDamage;
            if (AP.Contains(AbilityProperties.Finisher))
                if (health <= maxHealth / 2) damageToRecieve *= 2;
            SendConsoleMessage?.Invoke(this, "Armor Ignored!");
        }

        if (_unitStatusEffect.ContainsEffect(StatusEffect.ArmorBreak))
        {
            damageToRecieve = rawDamage;
            if (AP.Contains(AbilityProperties.Finisher))
                if (health <= maxHealth / 2) damageToRecieve *= 2;
        }
        else
        {
            damageToRecieve = rawDamage - (Armor * armorMultiplayer);
            if (AP.Contains(AbilityProperties.Finisher))
                if (health <= maxHealth / 2) damageToRecieve *= 2;
        }

        if (damageToRecieve <= 0)
            return;
        #endregion

        #region Normal Calculation
        if (currentPosture > 0)
        {
            if (((hitChance - CurrentEffectiveness) - (evasion * evasionMultiplayer)) >= DiceRoll)
            {
                if (critDiceRoll <= abilityCritChance)
                {
                    TakeDamage(damageToRecieve * 2, postureDamage * 2);
                    SendConsoleMessage?.Invoke(this, "Ability CRIT!");
                    OnCriticalHit?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    TakeDamage(damageToRecieve, postureDamage);
                    SendConsoleMessage?.Invoke(this, "Ability HIT!");
                }

                if (currentEffects.Count > 0 && UnityEngine.Random.Range(0, 99) <= chanceToTakeStatusEffect)
                {
                    _unitStatusEffect.AddStatusEffectToUnit(currentEffects, effectDuration);
                }
            }
            else
            {
                SendConsoleMessage?.Invoke(this, "Attack Missed");
                OnDodge?.Invoke(this, EventArgs.Empty);
            }
            return;


        }
        #endregion

        #region PostureBrake
        else
        {
            TakeDamage(damageToRecieve, postureDamage);
            if(currentEffects.Count > 0)
            _unitStatusEffect.AddStatusEffectToUnit(currentEffects, effectDuration);

            SendConsoleMessage?.Invoke(this, "Posture Break Attack!");
        }
        #endregion
    }

    public UnitStatusEffects getUnitStatusEffects() { return _unitStatusEffect; }
    public void Heal(float healValue)
    {
        health += healValue;
        if (health > maxHealth)
        {
            health = maxHealth;
        }
        OnHeal?.Invoke(this, EventArgs.Empty);
    }
    public void TryToTakeStatusEffect()
    {
        // _unitStatusEffect.unitActiveStatusEffects.Add(_unit.SetGridStatusEffect(_abilityEffect));

    }

    private void TakeDamage(float damageToRecieve, float postureDamage)
    {
        health -= damageToRecieve;
        currentPosture -= postureDamage;
        //currentPosture -= (BaseAbility)UnitActionSystem.Instance.GetSelectedAction().get
        if (_unitStatusEffect.ContainsEffect(StatusEffect.Undying))
        {
            InvokeHPChange();
            return;
        }
        if (health < 0) health = 0;

        InvokeHPChange();

        if (health == 0) Die();
    }

    public void InvokeHPChange()
    {
        OnDamaged?.Invoke(this, EventArgs.Empty);
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}