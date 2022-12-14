using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private enum State { WaitingForEnemyTurn, TakingTurn, Busy }

    private State state;
    private float timer;

    private void Awake()
    {
        state = State.WaitingForEnemyTurn;
    }

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
            return;

        switch (state)
        {
            case State.WaitingForEnemyTurn:
                break;
            case State.TakingTurn:
                timer -= Time.deltaTime;
                if (timer <= 0)
                {
                    if (TryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }

    private void SetStateTakingTurn()
    {
        timer = 0.5f;
        state = State.TakingTurn;
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e) 
    {
        if (!TurnSystem.Instance.IsPlayerTurn())
        {
            state = State.TakingTurn;
            timer = 2f;
        }
    }

    private bool TryTakeEnemyAIAction(Action onEnemyAIActionComplete)
    {
        foreach (Unit enemyUnit in UnitManager.Instance.GetEnemyUnitList())
        {
            if (TryTakeEnemyAIAction(enemyUnit, onEnemyAIActionComplete))
            {
                return true;
            }
        }
        return false;
    }

    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction _bestEnemyAIAction = null;
        BaseAction _bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            if (enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
                continue; // Enemy can't afford this action

            if (_bestEnemyAIAction == null)// Set first value
            {
                _bestEnemyAIAction = baseAction.GetBestEnemyAIAction();
                _bestBaseAction = baseAction;
            }
            else // Compare other actions value to the first if better value found, replace.
            {
                EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();

                if (testEnemyAIAction != null && testEnemyAIAction.actionValue > _bestEnemyAIAction.actionValue)
                {
                    _bestEnemyAIAction = testEnemyAIAction;
                    _bestBaseAction = baseAction;
                }
            }
        }

        if (_bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(_bestBaseAction))
        {
            _bestBaseAction.TakeAction(_bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        else
        {
            return false;
        }
    }

}