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
    [SerializeField] private ParticleSystem[] _onRecieveDamageVFX;
    [SerializeField] private ParticleSystem[] _friendlyHealVFX;
    [SerializeField] private Renderer[] _unitMeshRendererGroup;
    [SerializeField] private Material _corruptMat;

    [Header("On Recieving Status effect")]
    [SerializeField] private ParticleSystem[] _stunVFX;
    [SerializeField] private Animator _rootVFX;
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
    [SerializeField] private ParticleSystem[] _aBasicAttack;
    [SerializeField] private ParticleSystem[] _aPuncture;
    [SerializeField] private ParticleSystem[] _aMurder;
    [Header("Invisibility")]
    [SerializeField] private Material m_InvisibleMat;
    [SerializeField] private MeshAndMat hairGroup;
    [SerializeField] private MeshAndMat bodyGroup;
    [SerializeField] private MeshAndMat equipmentGroup;
    [SerializeField] private MeshAndMat weaponOneGroup;
    [SerializeField] private MeshAndMat weaponTwoGroup;
    [SerializeField] private MeshAndMat other1Group;
    [SerializeField] private MeshAndMat other2Group;

    private string _inviMatNoiseStrength = "Vector1_1a2d15c3d3f04ff7ac01469d7e8986bb";

    [Header("Raynard Abilities")]
    [SerializeField] private ParticleSystem[] _rEruption;
    [SerializeField] private ParticleSystem[] _rEnsnare;
    [SerializeField] private ParticleSystem[] _ability3R;
    [SerializeField] private ParticleSystem[] _ability4R;
    [SerializeField] private ParticleSystem[] _ability5R;
    [Header("Nanook Abilities")]
    [SerializeField] private ParticleSystem[] _nBasicAttack;
    [SerializeField] private ParticleSystem[] _nCleave;
    [SerializeField] private ParticleSystem[] _nPommleStirke;
    [SerializeField] private ParticleSystem[] _nEldestsDuty;

    private bool _isCorrupted = false;

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
                _rootVFX.SetBool("TurnOn", true);
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

                //if Material exists skip
                if (_isCorrupted) { return; }

                //Add corrupts material
                foreach (Renderer renderer in _unitMeshRendererGroup)
                {
                    //Get the current materials of the renderer
                    Material[] currentlyAssignedMaterials = renderer.materials;
                    //Create a new materials array with space for the additional material
                    Material[] updatedMaterials = new Material[currentlyAssignedMaterials.Length + 1];
                    //Copy the existing materials to the new array
                    for (int i = 0; i < currentlyAssignedMaterials.Length; i++)
                        updatedMaterials[i] = currentlyAssignedMaterials[i];
                    //Add your new material (e.g., _corruptMat) at the end
                    updatedMaterials[updatedMaterials.Length - 1] = _corruptMat;
                    //Assign the updated materials array back to the renderer
                    renderer.materials = updatedMaterials;
                }
                _isCorrupted = true;
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
                _rootVFX.SetBool("TurnOn", false);
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
                //remove corrupt material
                foreach (Renderer renderer in _unitMeshRendererGroup)
                {
                    //Get the current materials of the renderer
                    Material[] currentlyAssignedMaterials = renderer.materials;
                    //Create a new materials array with space for the additional material
                    Material[] updatedMaterials = new Material[currentlyAssignedMaterials.Length - 1];
                    //Copy the existing materials to the new array
                    for (int i = 0; i < currentlyAssignedMaterials.Length - 1; i++)
                        updatedMaterials[i] = currentlyAssignedMaterials[i];
                    //Assign the updated materials array back to the renderer
                    renderer.materials = updatedMaterials;
                }
                _isCorrupted = false;
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

    public void OnEnteredNanookHeal()
    {
        ActivateVFXArray(_friendlyHealVFX);
    }
    public void OnExitedNanookHeal()
    {
        StopVFXArray(_friendlyHealVFX);
    }

    //Called through Animation
    public void PlayTakeDamage()
    {
        ActivateVFXArray(_onRecieveDamageVFX);
    }
    public void PlayNBasicAttack()
    {
        ActivateVFXArray(_nBasicAttack);
    }
    public void PlayPommleStrike()
    {
        ActivateVFXArray(_nPommleStirke);
    }
    public void PlayCleave()
    {
        ActivateVFXArray(_nCleave);
    }
    public void PlayEldestsDuty()
    {
        ActivateVFXArray(_nEldestsDuty);
    }
    public void PlayABasicAttack()
    {
        ActivateVFXArray(_aBasicAttack);
    }
    public void PlayPuncture()
    {
        ActivateVFXArray(_aPuncture);
    }
    public void PlayMurder()
    {
        ActivateVFXArray(_aMurder);
    }
    public void PlayEruption()
    {
        ActivateVFXArray(_rEruption);
    }
    public void PlayEnsnare()
    {
        ActivateVFXArray(_rEnsnare);
    }

    //private Methods
    private void ChangeAmarokInvisibilityMat(bool invisible)
    {
        if (m_InvisibleMat)
            if (invisible)
            {
                m_InvisibleMat.SetFloat(_inviMatNoiseStrength, -5);
                hairGroup.ChangeGroupMat(m_InvisibleMat);
                bodyGroup.ChangeGroupMat(m_InvisibleMat);
                equipmentGroup.ChangeGroupMat(m_InvisibleMat);
                weaponOneGroup.ChangeGroupMat(m_InvisibleMat);
                weaponTwoGroup.ChangeGroupMat(m_InvisibleMat);
                other1Group.ChangeGroupMat(m_InvisibleMat);
                other2Group.ChangeGroupMat(m_InvisibleMat);
                StartCoroutine(turnInviVisible());
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
                vfxArray[i].Play();
    }
    private void StopVFXArray(ParticleSystem[] vfxArray)
    {
        if (vfxArray != null)
            for (int i = 0; i < vfxArray.Length; i++)
                vfxArray[i].Stop();
    }

    private IEnumerator turnInviVisible()
    {
        while (m_InvisibleMat.GetFloat(_inviMatNoiseStrength) <= 15f)
        {
            yield return null;
            m_InvisibleMat.SetFloat(_inviMatNoiseStrength, m_InvisibleMat.GetFloat(_inviMatNoiseStrength) + Time.deltaTime * 5f);
        }
    }

}