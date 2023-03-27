using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using System;

public class UnitManager : MonoBehaviour
{
    public static UnitManager Instance { get; private set; }
    public static event EventHandler GameLost;

    private List<Unit> unitList;
    private List<Unit> enemyUnitList;
    private List<Unit> friendlyUnitList;
    private bool partyWipped = false;
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
        BaseAction.OnAnyActionCompleted += BaseAction_OnAnyActionCompleted;
        ManosInputController.Instance.SwitchSelectedPlayer.performed += InputController_SwitchSelectedPlayer;
    }

    private void InputController_SwitchSelectedPlayer(InputAction.CallbackContext context)
    {
        friendlyID++;

        if (friendlyID > friendlyUnitList.Count - 1)
            friendlyID = 0;

        UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[friendlyID]);
    }

    public List<Unit> GetUnitList() { return unitList; }

    public List<Unit> GetEnemyUnitList() { return enemyUnitList; }

    public List<Unit> GetFriendlyUnitList() { return friendlyUnitList; }

    public void SelectFriendlyUnitWithUI(int brotherID)
    {
        switch (brotherID)
        {
            case 0:
                friendlyID = 0;
                UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[0]);
                break;
            case 1:
                friendlyID = 1;
                UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[1]);
                break;
            case 2:
                friendlyID = 2;
                UnitActionSystem.Instance.SetSelectedUnit(friendlyUnitList[2]);
                break;
            default:
                break;
        }
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

    private void BaseAction_OnAnyActionCompleted(object sender, EventArgs e)
    {
        if (partyWipped)
            GameLost?.Invoke(this, EventArgs.Empty);
    }

}