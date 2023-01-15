using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using Adobe.Substance;
using UnityEditor.PackageManager;
using Random = System.Random;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;


    [SerializeField] private float maxHealth = 100;
    [SerializeField] private float maxPosture = 100;
    [SerializeField] private float Armor = 100;
    [SerializeField] private float evasion = 100;
    private float health;
    private float postureAmount;

    private void Awake()
    {
        health = maxHealth;
    }

    public float GetHealthNormalized() { return health / maxHealth; }

    public void TakeDamage(float damage, float hitChance)
    {
        int DiceRoll = UnityEngine.Random.Range(0, 101);
        if (postureAmount <= 0)
        {
            health = (health + Armor) - damage;
            OnDamaged?.Invoke(this, EventArgs.Empty);

            if (health == 0) Die();
        }
        else
        {
            if ((hitChance - evasion) >= DiceRoll)
            {
                if (health < 0) health = 0;

                OnDamaged?.Invoke(this, EventArgs.Empty);

                if (health == 0) Die();
            }
            else
            {
                return;
            }
        }


    }
    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }
}