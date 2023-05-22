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
            { subVFX.Play(); }
    }

    public void PlaySlashAnim()
    {
        _SlashVFX.Play();
    }

}