using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class MagicSystem : MonoBehaviour
{
    [SerializeField] private int maxFavor = 300, minFavor = -300;

    private int currentFavor = 0;

    private void Start()
    {
        BaseAbility.OnAnySpellCast += BaseAbility_OnAnySpellCast;
    }

    public void BaseAbility_OnAnySpellCast(object sender, int usedFavor)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            currentFavor += usedFavor;
            if (currentFavor > maxFavor) { currentFavor = maxFavor; }
        }
        else
        {
            currentFavor -= usedFavor;
            if (currentFavor < minFavor) { currentFavor = minFavor; }
        }
        print(currentFavor);
    }

}