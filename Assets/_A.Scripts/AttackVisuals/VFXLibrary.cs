using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class VFXLibrary : MonoBehaviour
{
    [SerializeField] private ParticleSystem _SlashVFX;
    [SerializeField] private ParticleSystem[] _HealVFX;
    [SerializeField] private ParticleSystem[] _StunVFX;

    public void PlaySlashAnim()
    {
        if (_SlashVFX)
            _SlashVFX.Play();
    }

    public void OnStatusEffectRecieved(StatusEffect statusEffect, bool isActive)
    {
        switch (statusEffect)
        {
            case StatusEffect.None:
                break;
            case StatusEffect.Stun:
                ActivateVFXArray(_StunVFX);
                break;
            case StatusEffect.Silence:
                break;
            case StatusEffect.Root:
                break;
            case StatusEffect.ArmorBreak:
                break;
            case StatusEffect.GainArmor:
                break;
            case StatusEffect.Haste:
                break;
            case StatusEffect.Blind:
                break;
            case StatusEffect.Undying:
                break;
            case StatusEffect.Regeneration:
                break;
            case StatusEffect.Corruption:
                break;
            case StatusEffect.CowardPlague:
                break;
            case StatusEffect.Nullify:
                break;
            case StatusEffect.ToBeTauntUnused:
                break;
            default:
                break;
        }
    }

    public void PlayHealActionAnim()
    {
        ActivateVFXArray(_HealVFX);
    }
    public void StopHealActionVFX()
    {
        StopVFXArray(_HealVFX);
    }

    private void ActivateVFXArray(ParticleSystem[] vfxArray)
    {
        for (int i = 0; i < vfxArray.Length; i++)
            vfxArray[i].Play();
    }
    private void StopVFXArray(ParticleSystem[] vfxArray)
    {
        for (int i = 0; i < vfxArray.Length; i++)
            vfxArray[i].Stop();
    }

}