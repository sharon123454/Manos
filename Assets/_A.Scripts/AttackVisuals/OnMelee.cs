using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class OnMelee : MonoBehaviour
{
    [SerializeField] private ParticleSystem _SlashVFX;
    [SerializeField] private ParticleSystem[] _HealVFX;

    public void PlayHealAnim()
    {
        foreach (ParticleSystem subVFX in _HealVFX)
            if (subVFX)
            { subVFX.Play(); }
    }
    public void StopHealVFX()
    {
        foreach (ParticleSystem subVFX in _HealVFX)
            if (subVFX)
            { subVFX.Stop(); }
    }

    public void PlaySlashAnim()
    {
        if (_SlashVFX)
            _SlashVFX.Play();
    }

}