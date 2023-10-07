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

    public Effectiveness GetEffectiveness => _unit.GetGridEffectiveness();
    private int CurrentEffectiveness(Effectiveness effectiveness)
    {
        switch (effectiveness)
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
    public float GetDamageTaken() { return (health - UnitActionSystem.Instance.GetSelectedBaseAbility().GetDamage()) / maxHealth; }

    public float GetPostureNormalized() { return currentPosture / maxPosture; }
    public float GetPostureTaken() { return (health - UnitActionSystem.Instance.GetSelectedBaseAbility().GetPostureDamage()) / maxHealth; }

    public float GetUnitMaxHP() { return maxHealth; }
    public float GetUnitMaxPosture() { return maxPosture; }

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
    public void TryTakeDamage(float rawDamage, float postureDamage, float hitChance, float actionCritChance, List<StatusEffect> currentEffects, List<AbilityProperties> AP, int StatusEffectChance, int effectDuration, Effectiveness effectiveness)
    {
        #region Dice Rolls
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        int critDiceRoll = UnityEngine.Random.Range(0, 101);
        int statusDiceRoll = UnityEngine.Random.Range(0, 101);
        #endregion

        float damageToRecieve = rawDamage;

        #region Handling Ability properties from Action
        if (AP.Contains(AbilityProperties.Heal))
        {
            Heal(damageToRecieve, currentEffects, effectDuration);
            return;
        }
        if (AP.Contains(AbilityProperties.Finisher))
        {
            if (health <= maxHealth / 2)
                damageToRecieve *= 2;
        }
        if (AP.Contains(AbilityProperties.IgnoreArmor))
        {
            damageToRecieve += Armor * armorMultiplayer;
        }
        #endregion

        #region Handling Unit Active Status effects
        if (_unitStatusEffect.ContainsEffect(StatusEffect.Blind))
        {
            hitChance *= 2;
        }
        #endregion

        if (damageToRecieve <= 0)
        {
            SendConsoleMessage?.Invoke(this, $"{name} recieved less then 0 damage - bug?");
            damageToRecieve = 0;
        }

        #region Normal Calculation
        print(_unit.name + " hit chance: " + hitChance);
        print($"hitchance - effectiveness - evasion: {hitChance - CurrentEffectiveness(effectiveness) - (evasion * evasionMultiplayer)}");
        if (currentPosture > 0)
        {
            if (DiceRoll <= ((hitChance - CurrentEffectiveness(effectiveness)) - (evasion * evasionMultiplayer)))
            {
                if (critDiceRoll <= actionCritChance + MagicSystem.Instance.AddCritChanceFromFavor(_unit.IsEnemy()))
                {
                    TakeDamage(damageToRecieve * 2, postureDamage * 2);
                    SendConsoleMessage?.Invoke(this, "Ability CRIT!");
                    OnCriticalHit?.Invoke(this, EventArgs.Empty);
                }
                else
                {
                    TakeDamage(damageToRecieve, postureDamage);
                }

                if (currentEffects.Count > 0 && statusDiceRoll <= StatusEffectChance)
                    foreach (StatusEffect effect in currentEffects)
                        _unitStatusEffect.AddStatusEffectToUnit(effect, effectDuration);
            }
            else
            {
                OnDodge?.Invoke(this, EventArgs.Empty);
                SendConsoleMessage?.Invoke(this, $"Attack Missed {name}");
            }
        }
        #endregion

        #region PostureBrake
        else
        {
            TakeDamage(damageToRecieve, postureDamage);
            if (currentEffects.Count > 0)
                foreach (StatusEffect effect in currentEffects)
                    _unitStatusEffect.AddStatusEffectToUnit(effect, effectDuration);

            SendConsoleMessage?.Invoke(this, "Posture Break Attack!");
        }
        #endregion
    }

    public UnitStatusEffects getUnitStatusEffects() { return _unitStatusEffect; }
    public void Heal(float healValue, List<StatusEffect> abilityEffects, int effectDuration)
    {
        health += healValue;
        if (health > maxHealth)
        {
            health = maxHealth;
        }

        OnHeal?.Invoke(this, EventArgs.Empty);
        SendConsoleMessage?.Invoke(this, $"{name} was healed for {healValue}");

        if (abilityEffects.Count > 0)
        {
            foreach (StatusEffect effect in abilityEffects)
            {
                _unitStatusEffect.AddStatusEffectToUnit(effect, effectDuration);
            }
        }
    }

    private void TakeDamage(float damageToRecieve, float postureDamage)
    {
        if (damageToRecieve - Armor * armorMultiplayer > 0)//Make sure armor is lower than damage
            health -= (damageToRecieve - Armor * armorMultiplayer);
        currentPosture -= postureDamage;
        //currentPosture -= (BaseAbility)UnitActionSystem.Instance.GetSelectedAction().get
        OnDamaged?.Invoke(this, EventArgs.Empty);
        SendConsoleMessage?.Invoke(this, $"{name} was damaged for {Math.Clamp(damageToRecieve - Armor * armorMultiplayer, 0, int.MaxValue)} HP, {postureDamage} Posture.");

        if (_unitStatusEffect.ContainsEffect(StatusEffect.Undying)) { return; }

        if (health < 0) health = 0;
        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}