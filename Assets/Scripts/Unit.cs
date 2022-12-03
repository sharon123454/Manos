using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private int actionPointsMax = 2;

    public static event EventHandler OnAnyActionPointsChanged;

    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private int actionPoints;

    private void Awake()
    {
        actionPoints = actionPointsMax;
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            LevelGrid.Instance.UnitMovedGridPosition(this, gridPosition, newGridPosition);
            gridPosition = newGridPosition;
        }
    }

    public BaseAction[] GetBaseActionArray() { return baseActionArray; }

    public GridPosition GetGridPosition() { return gridPosition; }

    public MoveAction GetMoveAction() { return moveAction; }

    public SpinAction GetSpinction() { return spinAction; }

    public int GetActionPoints() { return actionPoints; }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        actionPoints = actionPointsMax;
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

    public bool TrySpenActionPointsToTakeAction(BaseAction baseAction)
    {
        if (CanSpenActionPointsToTakeAction(baseAction))
        {
            SpendActionPoints(baseAction.GetActionPointCost());
            return true;
        }
        else
        {
            return false;
        }
    }

    private bool CanSpenActionPointsToTakeAction(BaseAction baseAction)
    {
        return actionPoints >= baseAction.GetActionPointCost();
    }

    private void SpendActionPoints(int amount) 
    {
        actionPoints -= amount; 
        OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
    }

}