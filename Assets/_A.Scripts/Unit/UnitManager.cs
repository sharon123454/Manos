using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    public static event EventHandler GameLost;
    public static event EventHandler GameWon;

    private List<Unit> unitList;
    private List<Unit> enemyUnitList;
    private List<Unit> friendlyUnitList;
    private bool partyWipped = false, partyWin = false;
    private int friendlyID = 0;

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
        Unit.OnAnyUnitDead += Unit_OnAnyUnitDead;
        Unit.OnAnyUnitSpawned += Unit_OnAnyUnitSpawned;
        ManosInputController.Instance.SwitchSelectedPlayer.performed += InputController_SwitchSelectedPlayer;
    }

    private void InputController_SwitchSelectedPlayer(InputAction.CallbackContext context)
    {
        SwitichToNextUnit();
    }

    public void SwitichToNextUnit()
    {
        friendlyID++;

        if (friendlyID > friendlyUnitList.Count - 1)
            friendlyID = 0;

        UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[friendlyID]);
    }

    public GameObject GetCurrentUnit() { return friendlyUnitList[friendlyID].gameObject; }
    public List<Unit> GetUnitList() { return unitList; }
    public List<Unit> GetEnemyUnitList() { return enemyUnitList; }
    public List<Unit> GetFriendlyUnitList() { return friendlyUnitList; }

    public void SelectFriendlyUnitWithUI(string brotherName)
    {
        foreach (Unit unit in friendlyUnitList)
            if (brotherName == unit.name)
                UnitActionSystem.Instance.SetSelectedUnit(unit);
    }

    private void Unit_OnAnyUnitDead(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Remove(unit);

        if (unit.IsEnemy())
            enemyUnitList.Remove(unit);
        else
            friendlyUnitList.Remove(unit);

        if (friendlyUnitList.Count <= 0)
            partyWipped = true;

        if (enemyUnitList.Count <= 0)
            partyWin = true;

        if (partyWipped)
            GameLost?.Invoke(this, EventArgs.Empty);
        if (partyWin)
            GameWon?.Invoke(this, EventArgs.Empty);
    }

    private void Unit_OnAnyUnitSpawned(object sender, EventArgs e)
    {
        Unit unit = sender as Unit;

        unitList.Add(unit);

        if (unit.IsEnemy())
            enemyUnitList.Add(unit);
        else
        {
            friendlyUnitList.Add(unit);

            if (friendlyUnitList.Count == 1)
                UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[0]);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        int _selectedFriendlyID = 0;

        foreach (Unit friendlyUnit in friendlyUnitList)
        {
            if (friendlyUnit == UnitActionSystem.Instance.GetSelectedUnit())
                friendlyID = _selectedFriendlyID;

            _selectedFriendlyID++;
        }
    }

}