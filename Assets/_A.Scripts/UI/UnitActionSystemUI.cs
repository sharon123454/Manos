using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] Transform actionButtonPrefab, actionButtonContainerTransform;
    //[SerializeField] TextMeshProUGUI actionPointsText;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>();
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        UnitActionSystem.Instance.OnActionStarted += UnitActionSystem_OnActionStarted;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange; //this event might trigger before point reset so next line coveres small bug chance
        Unit.OnAnyActionPointsChanged += Unit_OnAnyActionPointsChanged;

        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void UnitActionSystem_OnActionStarted(object sender, EventArgs e) { UpdateActionPoints(); }

    private void Unit_OnAnyActionPointsChanged(object sender, EventArgs e) { UpdateActionPoints(); }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            actionButtonContainerTransform.gameObject.SetActive(true);
            UpdateActionPoints();
        }
        else
        {
            actionButtonContainerTransform.gameObject.SetActive(false);
        }
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, EventArgs e)
    {
        CreateUnitActionButton();
        UpdateSelectedVisual();
        UpdateActionPoints();
    }

    private void CreateUnitActionButton()
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
            Destroy(buttonTransform.gameObject);

        actionButtonUIList.Clear();

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                if (baseAction.isActiveAndEnabled)
                {
                    Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                    ActionButtonUI actionbuttonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                    actionbuttonUI.SetBaseAction(baseAction);

                    actionButtonUIList.Add(actionbuttonUI);
                }
            }
        }
    }

    private void UpdateSelectedVisual()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
            actionButtonUI.UpdateSelectedVisual();
    }

    private void UpdateActionPoints()
    {
        var selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit)
        {
            //if (UnitActionSystem.Instance.selectedAction.GetIsBonusAction())
            //    actionPointsText.text = $"Bonus Action";
            ////{selectedUnit.GetUsedBonusActionPoints()}
            //else
            //    actionPointsText.text = $"Action";
            // {selectedUnit.GetUsedActionPoints()}
        }
    }

}