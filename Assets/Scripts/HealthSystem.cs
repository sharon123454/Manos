using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;
    public event EventHandler OnDamaged;

    [SerializeField] private float maxHealth = 100;
    private float health;

    private void Awake()
    {
        health = maxHealth;
    }

    public float GetHealthNormalized() {return health / maxHealth;}

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0) health = 0;

        OnDamaged?.Invoke(this, EventArgs.Empty);

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}