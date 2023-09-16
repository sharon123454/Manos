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
        UnitActionSystem.Instance.OnBusyChanged += UnitActionSystem_OnBusyChanged;

        UpdateTurnText();
        UpdateUIVisibility(TurnSystem.Instance.IsPlayerTurn());
    }

    private void UnitActionSystem_OnBusyChanged(object sender, bool isBusy)
    {
        UpdateUIVisibility(!isBusy);
    }
    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateUIVisibility(TurnSystem.Instance.IsPlayerTurn());
    }

    private void UpdateTurnText()
    {
        //turnNumberText.text = $"TURN {TurnSystem.Instance.GetTurnNumber()}";
    }
    private void UpdateUIVisibility(bool isVisible)
    {
        endTurnBtn.gameObject.SetActive(isVisible);
        actionsUIGO.SetActive(isVisible);
    }

}