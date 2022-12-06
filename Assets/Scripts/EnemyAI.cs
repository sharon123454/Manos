using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class EnemyAI : MonoBehaviour
{
    private float timer;

    private void Start()
    {
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
    }

    private void Update()
    {
        if (TurnSystem.Instance.IsPlayerTurn())
            return;

        timer -= Time.deltaTime;
        if (timer <= 0)
            TurnSystem.Instance.NextTurn();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e) { timer = 2f; }

}