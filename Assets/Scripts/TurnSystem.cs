using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class TurnSystem : MonoBehaviour
{
    public static TurnSystem Instance { get; private set; }
    public event EventHandler OnTurnChange;

    private int turnNumber = 1;
    private bool isPlayerTurn = true;

    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);

        Instance = this;
    }

    public int GetTurnNumber() { return turnNumber; }

    public bool IsPlayerTurn() { return isPlayerTurn; }

    public void NextTurn()
    {
        turnNumber++;
        isPlayerTurn = !isPlayerTurn;

        OnTurnChange?.Invoke(this, EventArgs.Empty);
    }

}