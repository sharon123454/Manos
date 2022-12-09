using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class Unit : MonoBehaviour
{
    [SerializeField] private int actionPointsMax = 2;
    [SerializeField] private bool isEnemy;

    public static event EventHandler OnAnyActionPointsChanged;

    private BaseAction[] baseActionArray;
    private GridPosition gridPosition;
    private HealthSystem healthSystem;
    private MoveAction moveAction;
    private SpinAction spinAction;
    private int actionPoints;

    private void Awake()
    {
        actionPoints = actionPointsMax;
        moveAction = GetComponent<MoveAction>();
        spinAction = GetComponent<SpinAction>();
        healthSystem = GetComponent<HealthSystem>();
        baseActionArray = GetComponents<BaseAction>();
    }

    private void Start()
    {
        gridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        LevelGrid.Instance.AddUnitAtGridPosition(gridPosition, this);

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        healthSystem.OnDeath += HealthSystem_OnDeath;
    }

    private void Update()
    {
        GridPosition newGridPosition = LevelGrid.Instance.GetGridPosition(transform.position);
        if (newGridPosition != gridPosition)
        {
            GridPosition oldGridPosition = gridPosition;
            gridPosition = newGridPosition;

            LevelGrid.Instance.UnitMovedGridPosition(this, oldGridPosition, newGridPosition);
        }
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

    public BaseAction[] GetBaseActionArray() { return baseActionArray; }

    public Vector3 GetWorldPosition() { return transform.position; }

    public GridPosition GetGridPosition() { return gridPosition; }

    public MoveAction GetMoveAction() { return moveAction; }

    public SpinAction GetSpinction() { return spinAction; }

    public int GetActionPoints() { return actionPoints; }

    public bool IsEnemy(){ return isEnemy; }

    public void Damage(float damage)
    {
        healthSystem.TakeDamage(damage);
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (IsEnemy() && !TurnSystem.Instance.IsPlayerTurn() ||
            !IsEnemy() && TurnSystem.Instance.IsPlayerTurn())
        {
            actionPoints = actionPointsMax;
            OnAnyActionPointsChanged?.Invoke(this, EventArgs.Empty);
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

    private void HealthSystem_OnDeath(object sender, EventArgs e)
    {
        LevelGrid.Instance.RemoveUnitAtGridPosition(gridPosition, this);
        Destroy(gameObject);
    }

}