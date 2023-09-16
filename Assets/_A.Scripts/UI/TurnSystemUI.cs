using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;

public class TurnSystemUI : MonoBehaviour
{
    [SerializeField] private Button endTurnBtn;
    [SerializeField] private GameObject actionsUIGO;
    //[SerializeField] private TextMeshProUGUI turnNumberText;

    private void Start()
    {
        endTurnBtn.onClick.AddListener(() =>
        {
            TurnSystem.Instance.NextTurn();
        });

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        //UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        UpdateTurnText();
        UpdateUIVisibility();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateUIVisibility();
    }

    private void UpdateTurnText()
    {
        //turnNumberText.text = $"TURN {TurnSystem.Instance.GetTurnNumber()}";
    }
    private void UpdateUIVisibility()
    {
        endTurnBtn.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
        actionsUIGO.SetActive(TurnSystem.Instance.IsPlayerTurn());
    }

}