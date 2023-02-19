using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;


public class BaseHeal : BaseAction
{
    [SerializeField] protected AbilityRange range;

    public static event EventHandler<int> OnAnySpellCast;

    [SerializeField] protected bool isSpell = true;

    [Range(1f, 600f)] [SerializeField] protected float healValue = 10;


    public float GetHealValue() { return healValue; }
    public AbilityRange GetRange() { return range; }

    public override void TakeAction(GridPosition gridPosition, Action actionComplete)
    {
        OnAnySpellCast?.Invoke(this, GetFavorCost());
    }

    public override EnemyAIAction GetEnemyAIAction(GridPosition gridPosition)
    {
        throw new NotImplementedException();
    }

    public override List<GridPosition> GetValidActionGridPositionList()
    {
        throw new NotImplementedException();
    }

    public override string GetActionName() { return "Heal"; }

    protected virtual void CastSpell()
    {
        OnAnySpellCast?.Invoke(this, GetFavorCost());
    }

}