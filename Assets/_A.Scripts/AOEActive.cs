using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AOEActive : MonoBehaviour
{
    [SerializeField] private UnitType affectingType;
    [SerializeField] private Vector3 _affectOffset = new Vector3(0, 0.5f, 0);
    [SerializeField] private List<ParticleSystem> _onEnterParticles;//connect Nanook_Heal_Friendly on entered units
    [SerializeField] private List<ParticleSystem> _particlesToTurnOff;

    private List<StatusEffect> _statusEffects;
    private List<Unit> _affectedUnitList;
    private Transform _myParent;
    private float _activeturns;
    private bool _isActive;
    private Unit _unit;

    void Start()
    {
        _myParent = transform.parent;
        _affectedUnitList = new List<Unit>();
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
    }
    private void OnTriggerEnter(Collider other)
    {
        SubscribeToAOE(other);
    }
    private void OnTriggerExit(Collider other)
    {
        UnSubscribeToAOE(other);
    }

    public void Init(Unit castingUnit, Vector3 actionActivationPos, bool followUnit, float numberOfTurns, List<StatusEffect> effects, float AOESize)
    {
        _unit = castingUnit;
        if (!followUnit)
        {
            transform.parent = transform.root;
            transform.localPosition = actionActivationPos + _affectOffset;
        }
        _statusEffects = new List<StatusEffect>();
        transform.localScale *= AOESize;
        _activeturns = numberOfTurns;
        _statusEffects = effects;
        _isActive = true;
    }

    private void SubscribeToAOE(Collider other)
    {
        Unit potentialTarget = other.GetComponent<Unit>();

        if (potentialTarget)
        {
            if (!_affectedUnitList.Contains(potentialTarget))
            {
                switch (affectingType)
                {
                    case UnitType.Player:
                        if (!potentialTarget.IsEnemy())
                            _affectedUnitList.Add(potentialTarget);
                        break;
                    case UnitType.Enemy:
                        if (potentialTarget.IsEnemy())
                            _affectedUnitList.Add(potentialTarget);
                        break;
                    case UnitType.Both:
                        _affectedUnitList.Add(potentialTarget);
                        break;
                    default:
                        break;
                }
            }
        }
    }
    private void UnSubscribeToAOE(Collider other)
    {
        Unit leavingTarget = other.GetComponent<Unit>();

        if (leavingTarget && _affectedUnitList.Contains(leavingTarget))
            _affectedUnitList.Remove(leavingTarget);
    }

    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        if (_isActive)
        {
            if (_unit.IsEnemy() && TurnSystem.Instance.IsPlayerTurn() ||
            !_unit.IsEnemy() && !TurnSystem.Instance.IsPlayerTurn())
            {
                _activeturns--;

                if (_affectedUnitList.Count > 0)
                {
                    foreach (Unit unit in _affectedUnitList)
                    {
                        foreach (StatusEffect status in _statusEffects)
                        {
                            unit.unitStatusEffects.AddStatusEffectToUnit(status, (int)_activeturns);
                        }
                    }
                }

                if (_activeturns <= 0)
                {
                    _isActive = false;
                    transform.parent = _myParent;
                    transform.localPosition = Vector3.zero + _affectOffset;

                    for (int i = 0; i < _particlesToTurnOff.Count; i++)
                        _particlesToTurnOff[i].Stop();
                }
            }
        }
    }

}