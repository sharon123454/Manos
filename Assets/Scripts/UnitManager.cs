using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }

    private List<Unit> unitList;
    private List<Unit> enemyUnitList;
    private List<Unit> friendlyUnitList;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;

        unitList = new List<Unit>();
        enemyUnitList = new List<Unit>();
        friendlyUnitList = new List<Unit>();
    }

    private void Start()
    {
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
    }

    public List<Unit> GetUnitList() { return unitList; }

    public List<Unit> GetEnemyUnitList() { return enemyUnitList; }

    public List<Unit> GetFriendlyUnitList() { return friendlyUnitList; }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
            enemyUnitList.Add(unit);
        else
            friendlyUnitList.Add(unit);
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
            enemyUnitList.Remove(unit);
        else
            friendlyUnitList.Remove(unit);
    }

}