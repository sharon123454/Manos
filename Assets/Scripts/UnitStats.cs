using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
//using UnityEditor.PackageManager;
using TMPro;

public class UnitStats : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;
    private Unit _unit;
    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxPosture = 100;
    [SerializeField] private float Armor = 0;
    [SerializeField] private float evasion = 20;

    private float health;
    private float currentPosture;
    private int armorMultiplayer = 1;
    private int evasionMultiplayer = 1;
    private int _Effectivness = 0;
    private float postureDMGMultiplayer = 1;

    private Effectiveness GetEffectiveness => _unit.GetGridEffectivness();
    private StatusEffect GetCurrentEffect => _unit.GetGridStatusEffect();
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
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
        _unit = GetComponent<Unit>();
    }

    private void Instance_OnTurnChange(object sender, EventArgs e)
    {
        if (!_unit.IsEnemy())
        {
            if (TurnSystem.Instance.IsPlayerTurn())
            {
                switch (GetCurrentEffect)
                {
                    case StatusEffect.None:
                        break;
                    case StatusEffect.Stun:
                        break;
                    case StatusEffect.IgnoreArmor:
                        break;
                    case StatusEffect.Root:
                        break;
                    case StatusEffect.CowardPlague:
                        break;
                    case StatusEffect.Nullify:
                        break;
                    case StatusEffect.Heal:
                        break;
                    case StatusEffect.GainArmor:
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
        }
    }

    private void Update()
    {

    }
    public float GetHealthNormalized() { return health / maxHealth; }
    public float GetPostureNormalized() { return currentPosture / maxPosture; }

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

    //missing posture damage
    public void TryTakeDamage(float rawDamage, float postureDamage, float hitChance)
    {
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        float damageToRecieve = rawDamage - (Armor * armorMultiplayer);
        if (damageToRecieve <= 0)
            return;

        if (currentPosture > 0)
        {
            if (((hitChance - CurrentEffectiveness) - (evasion * evasionMultiplayer)) >= DiceRoll)
            {
                TakeDamage(damageToRecieve, postureDamage);
            }
            else
                return; //attack missed
        }
        else
            TakeDamage(damageToRecieve, postureDamage);

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

    public void TakePostureDamage(float damageToRecieve)
    {
        currentPosture -= damageToRecieve;
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}