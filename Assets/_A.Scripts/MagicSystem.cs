using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;
using UnityEngine.UI;

public class MagicSystem : MonoBehaviour
{
    public static MagicSystem Instance;
    public static event EventHandler<float> OnFavorChanged;

    [SerializeField] private float critChanceIncrease = 20f;
    [SerializeField] private float divideFavorBy = 6;
    [SerializeField] private float startFavor = 300;
    [SerializeField] private float maxFavor = 600;

    private float critModifier = 0;
    [SerializeField] private float currentFavor = 0;
    [SerializeField] private Image playerBar;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    private void Start()
    {
        playerBar = GameObject.Find("PlayerBar").GetComponent<Image>();
        currentFavor = startFavor;
        OnFavorChanged?.Invoke(this, currentFavor / maxFavor);
        BaseAction.OnAnySpellCast += BaseAbility_OnAnySpellCast;
    }

    public int AddCritChanceFromFavor(bool freindlyUnit)
    {
        int value = (int)Math.Round(playerBar.fillAmount / 0.168f);
        print(value);
        switch (value)
        {
            case 1:
                if (freindlyUnit)
                    return 30;
                break;
            case 2:
                if (freindlyUnit)
                    return 15;
                break;
            case 3:
                break;
            case 4:
                if (!freindlyUnit)
                    return 15;
                break;
            case 5:
                if (!freindlyUnit)
                    return 30;
                break;
        }
        return 0;
    }
    public float GetMaxFavor() { return maxFavor; }
    public float GetCurrentFavor() { return currentFavor; }

    public bool CanEnemySpendFavorToTakeAction(int spellFavorCost)
    {
        return currentFavor + spellFavorCost <= GetMaxFavor();
    }
    public bool CanFriendlySpendFavorToTakeAction(int spellFavorCost)
    {
        return currentFavor - spellFavorCost >= 0;
    }

    //needs rework or use it as it technically works
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

    private void BaseAbility_OnAnySpellCast(object sender, int usedFavor)
    {
        float newFavor;

        if (TurnSystem.Instance.IsPlayerTurn())
            newFavor = currentFavor - usedFavor;
        else
            newFavor = currentFavor + usedFavor;

        newFavor = Mathf.Clamp(newFavor, 0, maxFavor);

        if (newFavor != currentFavor)
        {
            currentFavor = newFavor;
            OnFavorChanged?.Invoke(this, currentFavor / maxFavor);
        }

    }

}