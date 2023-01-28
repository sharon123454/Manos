using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MagicSystem : MonoBehaviour
{
    public static MagicSystem Instance;
    public static event EventHandler<float> OnFavorChanged;

    [SerializeField] private float critChanceIncrease = 20f;
    [SerializeField] private float divideFavorBy = 6;
    [SerializeField] private float startFavor = 300;
    [SerializeField] private float maxFavor = 600;

    private bool visualMatching = false;
    private float critModifier = 0;
    private float currentFavor = 0;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
        BaseAbility.OnAnySpellCast += BaseAbility_OnAnySpellCast;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void Start()
    {
        currentFavor = startFavor;
        UpdateFavorVisual();
    }

    /// <summary>
    /// hard coded trash, help
    /// </summary>
    /// <returns>X% Percentage to increase Crit Chance by</returns>
    public float GetCritModifier()
    {
        critModifier = 0;

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            if (currentFavor == 600)
                critModifier = critChanceIncrease * 3;
            else if (currentFavor >= 500)
                critModifier = critChanceIncrease * 2;
            else if (currentFavor >= 400)
                critModifier = critChanceIncrease;
        }
        else
        {
            if (currentFavor == 0)
                critModifier = critChanceIncrease * 3;
            else if (currentFavor <= 100)
                critModifier = critChanceIncrease * 2;
            else if (currentFavor <= 200)
                critModifier = critChanceIncrease;
        }

        return critModifier;
    }

    private void UpdateFavorVisual()
    {
        float normalizedFavor = (float)currentFavor / (float)maxFavor;
        OnFavorChanged?.Invoke(this, normalizedFavor);
        visualMatching = true;
        GetCritModifier();
    }

    private void BaseAbility_OnAnySpellCast(object sender, int usedFavor)
    {
        float newFavor;

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            newFavor = currentFavor - usedFavor;

            if (newFavor < 0)
            {
                newFavor = 0;
                currentFavor = 0;
            }
        }
        else
        {
            newFavor = currentFavor + usedFavor;

            if (newFavor > maxFavor)
            {
                newFavor = maxFavor;
                currentFavor = maxFavor;
            }
        }

        if (newFavor != currentFavor)
        {
            currentFavor = newFavor;
            visualMatching = false;
        }
    }

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        if (!visualMatching)
            UpdateFavorVisual();
    }

}