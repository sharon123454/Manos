using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

[Serializable]
public struct MeshAndMat
{
    public Renderer[] UnitMeshRendererGroup;
    public Material GroupMat;

    public void ChangeGroupMat(Material mat)
    {
        if (UnitMeshRendererGroup.Length > 0)
            foreach (var renderer in UnitMeshRendererGroup)
                renderer.material = mat;
    }
}

public class VFXLibrary : MonoBehaviour
{
    [Header("Generic")]
    [SerializeField] private ParticleSystem[] _healVFX;
    [SerializeField] private ParticleSystem[] _meleeAttackVFX;
    [Header("On Recieving Status effect")]
    [SerializeField] private ParticleSystem[] _stunVFX;
    [SerializeField] private ParticleSystem[] _rootVFX;
    [SerializeField] private ParticleSystem[] _hasteVFX;
    [SerializeField] private ParticleSystem[] _blindVFX;
    [SerializeField] private ParticleSystem[] _silnceVFX;
    [SerializeField] private ParticleSystem[] _nullifyVFX;
    [SerializeField] private ParticleSystem[] _undyingVFX;
    [SerializeField] private ParticleSystem[] _gainArmorVFX;
    [SerializeField] private ParticleSystem[] _armorBreakVFX;
    [SerializeField] private ParticleSystem[] _corruptionVFX;
    [SerializeField] private ParticleSystem[] _regenerationVFX;
    [SerializeField] private ParticleSystem[] _cowardPlagueVFX;
    [Header("Amarock Abilities")]
    [SerializeField] private ParticleSystem[] _ability1A;
    [SerializeField] private ParticleSystem[] _ability2A;
    [SerializeField] private ParticleSystem[] _ability3A;
    [SerializeField] private ParticleSystem[] _ability4A;
    [SerializeField] private ParticleSystem[] _ability5A;
    [Header("Invisibility")]
    [SerializeField] private Material m_InvisibleMat;
    [SerializeField] private MeshAndMat hairGroup;
    [SerializeField] private MeshAndMat bodyGroup;
    [SerializeField] private MeshAndMat equipmentGroup;
    [SerializeField] private MeshAndMat weaponOneGroup;
    [SerializeField] private MeshAndMat weaponTwoGroup;
    [SerializeField] private MeshAndMat other1Group;
    [SerializeField] private MeshAndMat other2Group;
    [Header("Raynard Abilities")]
    [SerializeField] private ParticleSystem[] _ability1R;
    [SerializeField] private ParticleSystem[] _ability2R;
    [SerializeField] private ParticleSystem[] _ability3R;
    [SerializeField] private ParticleSystem[] _ability4R;
    [SerializeField] private ParticleSystem[] _ability5R;
    [Header("Nanook Abilities")]
    [SerializeField] private ParticleSystem _SlashVFX;
    [SerializeField] private ParticleSystem[] _ability1N;
    [SerializeField] private ParticleSystem[] _ability2N;
    [SerializeField] private ParticleSystem[] _ability3N;
    [SerializeField] private ParticleSystem[] _ability4N;

    public void OnStatusEffectRecieved(StatusEffect statusEffect)
    {
        switch (statusEffect)
        {
            case StatusEffect.Stun:
                ActivateVFXArray(_stunVFX);
                break;
            case StatusEffect.Silence:
                ActivateVFXArray(_silnceVFX);
                break;
            case StatusEffect.Root:
                ActivateVFXArray(_rootVFX);
                break;
            case StatusEffect.ArmorBreak:
                ActivateVFXArray(_armorBreakVFX);
                break;
            case StatusEffect.GainArmor:
                ActivateVFXArray(_gainArmorVFX);
                break;
            case StatusEffect.Haste:
                ActivateVFXArray(_hasteVFX);
                break;
            case StatusEffect.Blind:
                ActivateVFXArray(_blindVFX);
                break;
            case StatusEffect.Undying:
                ActivateVFXArray(_undyingVFX);
                break;
            case StatusEffect.Regeneration:
                ActivateVFXArray(_regenerationVFX);
                break;
            case StatusEffect.Corruption:
                ActivateVFXArray(_corruptionVFX);
                break;
            case StatusEffect.Nullify:
                ActivateVFXArray(_nullifyVFX);
                break;
            case StatusEffect.Invisibility:
                print("invi was recieved");
                ChangeAmarokInvisibilityMat(true);
                break;
            case StatusEffect.CowardPlague:
                ActivateVFXArray(_cowardPlagueVFX);
                break;
            case StatusEffect.ToBeTauntUnused:
            case StatusEffect.Taunt:
                break;
            case StatusEffect.None:
            default:
                break;
        }
    }
    public void OnStatusEffectRemoved(StatusEffect statusEffect)
    {
        switch (statusEffect)
        {
            case StatusEffect.Stun:
                StopVFXArray(_stunVFX);
                break;
            case StatusEffect.Silence:
                StopVFXArray(_silnceVFX);
                break;
            case StatusEffect.Root:
                StopVFXArray(_rootVFX);
                break;
            case StatusEffect.ArmorBreak:
                StopVFXArray(_armorBreakVFX);
                break;
            case StatusEffect.GainArmor:
                StopVFXArray(_gainArmorVFX);
                break;
            case StatusEffect.Haste:
                StopVFXArray(_hasteVFX);
                break;
            case StatusEffect.Blind:
                StopVFXArray(_blindVFX);
                break;
            case StatusEffect.Undying:
                StopVFXArray(_undyingVFX);
                break;
            case StatusEffect.Regeneration:
                StopVFXArray(_regenerationVFX);
                break;
            case StatusEffect.Corruption:
                StopVFXArray(_corruptionVFX);
                break;
            case StatusEffect.Nullify:
                StopVFXArray(_nullifyVFX);
                break;
            case StatusEffect.Invisibility:
                ChangeAmarokInvisibilityMat(false);
                break;
            case StatusEffect.CowardPlague:
                StopVFXArray(_cowardPlagueVFX);
                break;
            case StatusEffect.ToBeTauntUnused:
            case StatusEffect.Taunt:
                break;
            case StatusEffect.None:
            default:
                break;
        }
    }
    public void PlayHealActionAnim()
    {
        ActivateVFXArray(_healVFX);
    }

    //Called through Animation
    public void PlaySlashAnim()
    {
        if (_SlashVFX)
            _SlashVFX.Play();
    }

    private void ChangeAmarokInvisibilityMat(bool invisible)
    {
        if (m_InvisibleMat)
            if (invisible)
            {
                hairGroup.ChangeGroupMat(m_InvisibleMat);
                bodyGroup.ChangeGroupMat(m_InvisibleMat);
                equipmentGroup.ChangeGroupMat(m_InvisibleMat);
                weaponOneGroup.ChangeGroupMat(m_InvisibleMat);
                weaponTwoGroup.ChangeGroupMat(m_InvisibleMat);
                other1Group.ChangeGroupMat(m_InvisibleMat);
                other2Group.ChangeGroupMat(m_InvisibleMat);
            }
            else
            {
                hairGroup.ChangeGroupMat(hairGroup.GroupMat);
                bodyGroup.ChangeGroupMat(bodyGroup.GroupMat);
                equipmentGroup.ChangeGroupMat(equipmentGroup.GroupMat);
                weaponOneGroup.ChangeGroupMat(weaponOneGroup.GroupMat);
                weaponTwoGroup.ChangeGroupMat(weaponTwoGroup.GroupMat);
                other1Group.ChangeGroupMat(other1Group.GroupMat);
                other2Group.ChangeGroupMat(other2Group.GroupMat);
            }
    }
    private void ActivateVFXArray(ParticleSystem[] vfxArray)
    {
        if (vfxArray != null)
            for (int i = 0; i < vfxArray.Length; i++)
            {
                vfxArray[i].gameObject.SetActive(true);
                vfxArray[i].Play();
            }
    }
    private void StopVFXArray(ParticleSystem[] vfxArray)
    {
        if (vfxArray != null)
            for (int i = 0; i < vfxArray.Length; i++)
                vfxArray[i].Stop();
    }

}