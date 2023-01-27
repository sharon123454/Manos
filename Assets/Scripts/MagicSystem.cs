using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class MagicSystem : MonoBehaviour
{
    public static event EventHandler<float> OnFavorChanged;

    [SerializeField] private int startFavor = 300, maxFavor = 600;

    private bool visualMatching = false;
    private int currentFavor = 0;

    private void Awake()
    {
        BaseAbility.OnAnySpellCast += BaseAbility_OnAnySpellCast;
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
    }

    private void Start()
    {
        currentFavor = startFavor;
        UpdateFavorVisual();
    }

    private void BaseAbility_OnAnySpellCast(object sender, int usedFavor)
    {
        int newFavor;

        if (TurnSystem.Instance.IsPlayerTurn())
        {
            newFavor = currentFavor + usedFavor;

            if (newFavor > maxFavor) 
                currentFavor = maxFavor;
        }
        else
        {
            newFavor = currentFavor - usedFavor;

            if (newFavor < 0) 
                currentFavor = 0;
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

    private void UpdateFavorVisual()
    {
        float normalizedFavor = (float)currentFavor / (float)maxFavor;
        OnFavorChanged?.Invoke(this, normalizedFavor);
        visualMatching = true;
    }

}