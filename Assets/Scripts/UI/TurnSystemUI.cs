using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI turnNumberText;
    [SerializeField] private GameObject enemyTurnVisualGO;
    [SerializeField] private Button endTurnBtn;

    private void Start()
    {
        endTurnBtn.onClick.AddListener(() => 
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        UpdateTurnText(); 
        EndTurnBtnVisibility();
        UpdateEnemyTurnVisual();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateEnemyTurnVisual(); 
        EndTurnBtnVisibility();
    }

    private void UpdateTurnText()
    {
        turnNumberText.text = $"TURN {TurnSystem.Instance.GetTurnNumber()}";
    }

    private void UpdateEnemyTurnVisual()
    {
        enemyTurnVisualGO.SetActive(!TurnSystem.Instance.IsPlayerTurn());
    }

    private void EndTurnBtnVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}