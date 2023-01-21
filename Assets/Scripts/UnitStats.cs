using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitStats : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;

    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxPosture = 50;
    [SerializeField] private float Armor = 0;
    [SerializeField] private float evasion = 20;

    private float health;
    private float postureAmount;
    private int armorMultiplayer = 1;
    private int evasionMultiplayer = 1;
    private float postureDMGMultiplayer = 1;

    private void Awake()
    {
        postureAmount = maxPosture;
        health = maxHealth;
    }

    public float GetHealthNormalized() { return health / maxHealth; }

    public void Block()
    {
        armorMultiplayer = 2;
        postureDMGMultiplayer = 0.5f;
    }

    public void Dodge()
    {
        evasionMultiplayer = 2;
    }

    public float GetEvasion(){return evasion;}
    public float GetPosture(){return postureAmount; }
    public void ResetUnitStats()
    {
        postureAmount = maxPosture;
        armorMultiplayer = 1;
        evasionMultiplayer = 1;
        postureDMGMultiplayer = 1f;
    }

    //missing posture damage
    public void TryTakeDamage(float rawDamage, float hitChance)
    {
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        float damageToRecieve = rawDamage - (Armor * armorMultiplayer);

        if (damageToRecieve <= 0)
            return;

        if (postureAmount > 0)
        {
            if ((hitChance - (evasion * evasionMultiplayer)) >= DiceRoll)
            {
                TakeDamage(damageToRecieve);
            }
            else
                return; //attack missed
        }
        else
            TakeDamage(damageToRecieve);

    }

    private void TakeDamage(float damageToRecieve)
    {
        health -= damageToRecieve;
        if (health < 0) health = 0;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}