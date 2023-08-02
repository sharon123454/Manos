using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AOEActive : MonoBehaviour
{
    [SerializeField] private UnitType affectingType;
    [SerializeField] private LayerMask affectedLayer;
    [SerializeField] private List<ParticleSystem> _particles;

    private List<StatusEffect> _statusEffects;
    private List<Unit> _affectedUnitList;
    private float _activeturns;

    void OnEnable()
    {
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
    private void OnDisable()
    {
        for (int i = 0; i < _particles.Count; i++)
            _particles[i].Stop();

        TurnSystem.Instance.OnTurnChange -= Instance_OnTurnChange;
    }

    public void Init(Unit castingUnit, float numberOfTurns, List<StatusEffect> effects, float AOESize)
    {
        transform.parent = castingUnit.transform;
        transform.localPosition = Vector3.zero;
        _statusEffects = new List<StatusEffect>();
        transform.localScale *= AOESize;
        _activeturns = numberOfTurns;
        _statusEffects = effects;
        ActivateVFX();
    }
    private void ActivateVFX()
    {
        if (_particles.Count > 0)
            for (int i = 0; i < _particles.Count; i++)
                _particles[i].Play();
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
        _activeturns--;
        
        if (_activeturns <= 0)
            Destroy(gameObject);

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
    }

}