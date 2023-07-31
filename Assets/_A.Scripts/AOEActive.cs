using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class AOEActive : MonoBehaviour
{
    [SerializeField] private LayerMask affectedLayer;
    [SerializeField] private List<StatusEffect> _statusEffects;
    [SerializeField] private List<ParticleSystem> _particles;
    private float _activeturns;
    private Unit _myUnit;

    private List<Unit> _affectedUnitList;

    void OnEnable()
    {
        TurnSystem.Instance.OnTurnChange += Instance_OnTurnChange;
    }
    private void OnTriggerEnter(Collider other)
    {
        Unit potentialTarget = other.GetComponent<Unit>();

        if (potentialTarget && !_affectedUnitList.Contains(potentialTarget) && potentialTarget.gameObject.layer == affectedLayer)
            _affectedUnitList.Add(potentialTarget);
    }
    private void OnTriggerExit(Collider other)
    {
        Unit leavingTarget = other.GetComponent<Unit>();

        if (leavingTarget && _affectedUnitList.Contains(leavingTarget))
            _affectedUnitList.Remove(leavingTarget);
    }
    private void OnDisable()
    {
        TurnSystem.Instance.OnTurnChange -= Instance_OnTurnChange;
    }

    public void Init(Unit castingUnit, float numberOfTurns, List<StatusEffect> effects)
    {
        _myUnit = castingUnit;
        _activeturns = numberOfTurns;
        _statusEffects = effects;
    }

    private void Instance_OnTurnChange(object sender, System.EventArgs e)
    {
        foreach (Unit unit in _affectedUnitList)
        {
            foreach (StatusEffect status in _statusEffects)
            {
                //activate effect on Unit
            }
        }

        _activeturns--;

        if (_activeturns <= 0)
            Destroy(gameObject);
    }
}