using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class HealthSystem : MonoBehaviour
{
    public event EventHandler OnDeath;

    [SerializeField] private float health = 100;

    public void TakeDamage(float damage)
    {
        health -= damage;

        if (health < 0) health = 0;

        if (health == 0) Die();
    }

    private void Die()
    {
        OnDeath?.Invoke(this, EventArgs.Empty);
    }

}