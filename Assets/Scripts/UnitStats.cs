using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
//using UnityEditor.PackageManager;
using TMPro;
using Random = System.Random;

public class UnitStats : MonoBehaviour
{
    public static event EventHandler<string> SendConsoleMessage;

    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;
    private Unit _unit;
    private UnitStatusEffects _unitStatusEffect;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxPosture = 100;
    [SerializeField] private float evasion = 20;

    public float health;
    public float Armor = 0;
    private float currentPosture;
    private int armorMultiplayer = 1;
    private int evasionMultiplayer = 1;
    private int _Effectivness = 0;
    private float postureDMGMultiplayer = 1;

    private Effectiveness GetEffectiveness => _unit.GetGridEffectivness();
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
                    _Effectivness = 100;
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
        //   print(_unit.GetGridStatusEffect() + ""+ _unit.name);
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
        currentPosture = maxPosture;
        armorMultiplayer = 1;
        evasionMultiplayer = 1;
        postureDMGMultiplayer = 1f;
    }

    public void TryTakeDamage(float rawDamage, float postureDamage, float hitChance, float abilityCritChance, StatusEffect currentEffect, int chanceToTakeStatusEffect, int effectDuration)
    {
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        int critDiceRoll = UnityEngine.Random.Range(0, 101);
        float damageToRecieve = rawDamage - (Armor * armorMultiplayer);
        if (damageToRecieve <= 0)
            return;

        if (currentPosture > 0)
        {
            if (((hitChance - CurrentEffectiveness) - (evasion * evasionMultiplayer)) >= DiceRoll)
            {
                if (critDiceRoll <= abilityCritChance)
                {
                    TakeDamage(damageToRecieve * 2, postureDamage * 2);
                    SendConsoleMessage?.Invoke(this, "Ability CRIT!");
                }
                else
                {
                    TakeDamage(damageToRecieve, postureDamage);
                    SendConsoleMessage?.Invoke(this, "Ability HIT!");
                }

                if (UnityEngine.Random.Range(0, 99) <= chanceToTakeStatusEffect)
                {
                    _unitStatusEffect.AddStatusEffectToUnit(currentEffect, effectDuration);
                    SendConsoleMessage?.Invoke(this, this.name + " Recived " + currentEffect);
                }
            }
            else
                SendConsoleMessage?.Invoke(this, "Attack Missed");
            return;
        }
        else
        {
            TakeDamage(damageToRecieve, postureDamage);
            _unitStatusEffect.AddStatusEffectToUnit(currentEffect, effectDuration);
        }
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
        if (health < 0) health = 0;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}