using UnityEngine;
using System.Collections.Generic;
using System;

public class EnemyAI : MonoBehaviour
{
    private List<Unit> enemyUnits; // You'll need to populate this list with the enemy units

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
                    if (OnTryTakeEnemyAIAction(SetStateTakingTurn))
                    {
                        state = State.Busy;
                    }
                    else
                    {
                        // No more enemies have actions they can take, end enemy turn
                        StopAllCoroutines();
                        TurnSystem.Instance.NextTurn();
                    }
                }
                break;
            case State.Busy:
                break;
        }
    }
    private bool OnTryTakeEnemyAIAction(Action onEnemyAIActionComplete)
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
    private bool TryTakeEnemyAIAction(Unit enemyUnit, Action onEnemyAIActionComplete)
    {
        EnemyAIAction bestEnemyAIAction = null;
        BaseAction bestBaseAction = null;

        foreach (BaseAction baseAction in enemyUnit.GetBaseActionArray())
        {
            // Check if the BaseAction script is enabled
            if (!baseAction.enabled)
                continue; // Skip this action if the script is not enabled

            if (enemyUnit.CanSpendActionPointsToTakeAction(baseAction))
                continue; // Enemy can't afford this action

            EnemyAIAction testEnemyAIAction = baseAction.GetBestEnemyAIAction();
            if (testEnemyAIAction == null)
                continue; // Skip if no valid action

            int randomFactorForTest = UnityEngine.Random.Range(0, 100);
            int randomFactorForBest = bestEnemyAIAction == null ? 0 : UnityEngine.Random.Range(0, 100);

            if (bestEnemyAIAction == null || testEnemyAIAction.actionValue + randomFactorForTest > bestEnemyAIAction.actionValue + randomFactorForBest)
            {
                bestEnemyAIAction = testEnemyAIAction; // Assign the test action if it's better
                bestBaseAction = baseAction; // Assign the corresponding base action
            }
        }

        if (bestEnemyAIAction != null && enemyUnit.TrySpendActionPointsToTakeAction(bestBaseAction))
        {
            bestBaseAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        // If no other action was executed and the enemy unit has action points left, execute MoveAction
        MoveAction moveAction = enemyUnit.GetComponent<MoveAction>();
        if (moveAction != null && moveAction.enabled && enemyUnit.TrySpendActionPointsToTakeAction(moveAction)) // Assuming this method exists
        {
            print("INSODE MOOOOOOOOOVE");
            moveAction.TakeAction(bestEnemyAIAction.gridPosition, onEnemyAIActionComplete);
            return true;
        }
        return false;
    }



    private void OnEnemyAIActionComplete()
    {
        // Logic to handle the completion of an enemy AI action

        // Example: Play a sound effect
        // AudioManager.PlaySound("actionComplete");

        // Example: Update the game state
        // GameStateManager.UpdateState();

        // Example: Start the next enemy's turn or end the enemy turn phase
        TurnSystem.Instance.NextTurn();
    }
    private void Shuffle<T>(IList<T> list)
    {
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = UnityEngine.Random.Range(0, n + 1);
            T value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
    }

    // You can add more methods and logic as needed, such as different AI personalities, tactical decision-making, collaboration between units, etc.
}