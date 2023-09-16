using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
public struct StatusVisual
{
    public StatusEffect Effect;
    public string EffectInfo;
    public Sprite EffectImage;
}
public class StatusEffectsUI : MonoBehaviour
{
    [SerializeField] private StatusVisual[] _status;
    [SerializeField] private StatusEffectsUISingle statusUIPrefab;

    private struct ActiveStatusEffect
    {
        public StatusEffectsUISingle UIElement;
        public StatusEffect Status;

        public static bool operator true(ActiveStatusEffect status)
        { return status.UIElement != null; }
        public static bool operator false(ActiveStatusEffect status)
        { return status.UIElement == null; }
    }

    private List<ActiveStatusEffect> _activeEffects = new List<ActiveStatusEffect>();
    private Unit myUnit;

    public void InitStatusUI(Unit unit)
    {
        myUnit = unit;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        myUnit.unitStatusEffects.OnStatusApplied += UnitStatusEffects_OnStatusApplied;
        myUnit.unitStatusEffects.OnStatusRemoved += UnitStatusEffects_OnStatusRemoved;
    }

    private void UnitStatusEffects_OnStatusApplied(object sender, StatusEffect effectApplied)
    {
        bool effectIsActive = false;

        foreach (ActiveStatusEffect activeEffect in _activeEffects)
            if (activeEffect.Status == effectApplied)
                effectIsActive = true;

        foreach (StatusVisual effect in _status)
        {
            if (effectApplied == effect.Effect)
            {
                if (!effectIsActive)
                {
                    //create new effect
                    StatusEffectsUISingle newUI = Instantiate(statusUIPrefab, transform);
                    newUI.Init(effect.Effect.ToString(), effect.EffectImage, effect.EffectInfo, myUnit.unitStatusEffects.GetEffectDurationByEffect(effect.Effect));
                    _activeEffects.Add(new ActiveStatusEffect { Status = effect.Effect, UIElement = newUI });
                }
                else
                {
                    //access existing status UI and Update
                    foreach (ActiveStatusEffect activeEffect in _activeEffects)
                    {
                        if (activeEffect.Status == effect.Effect)
                        {
                            activeEffect.UIElement.UpdateStatusEffect(myUnit.unitStatusEffects.GetEffectDurationByEffect(effect.Effect));
                            break;
                        }
                    }
                }
            }
        }
    }
    private void UnitStatusEffects_OnStatusRemoved(object sender, StatusEffect removedEffect)
    {
        ActiveStatusEffect objToDestroy = new ActiveStatusEffect();
        foreach (ActiveStatusEffect effect in _activeEffects)
            if (effect.Status == removedEffect)
                objToDestroy = effect;

        if (objToDestroy)
        {
            _activeEffects.Remove(objToDestroy);
            Destroy(objToDestroy.UIElement.gameObject);
        }
    }
    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        foreach (ActiveStatusEffect activeEffect in _activeEffects)
            activeEffect.UIElement.UpdateStatusEffect(myUnit.unitStatusEffects.GetEffectDurationByEffect(activeEffect.Status));
    }

}