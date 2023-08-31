using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class TurnSystemUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
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

        UpdateTurnText();
        UpdateUIVisibility();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        UpdateTurnText();
        UpdateUIVisibility();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetHoveringOnUI(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        UnitActionSystem.Instance.SetHoveringOnUI(false);
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