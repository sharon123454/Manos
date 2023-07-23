using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab, actionButtonContainerTransform;

    private List<ActionButtonUI> actionButtonUIList;

    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>(10);
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        CreateUnitActionButtons(UnitActionSystem.Instance.GetSelectedUnit());
        UpdateActionSystemVisuals();
    }

    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateActionSystemVisuals();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (actionButtonContainerTransform)
            actionButtonContainerTransform.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

        UpdateActionSystemVisuals();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit newlySelectedUnit)
    {
        CreateUnitActionButtons(newlySelectedUnit);
        UpdateActionSystemVisuals();
    }

    private void CreateUnitActionButtons(Unit selectedUnit)
    {
        foreach (Transform buttonTransform in actionButtonContainerTransform)
            Destroy(buttonTransform.gameObject);

        actionButtonUIList.Clear();

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

    private void UpdateActionSystemVisuals()
    {
        foreach (ActionButtonUI actionButtonUI in actionButtonUIList)
            if (actionButtonUI.isActiveAndEnabled)
                actionButtonUI.UpdateButtonVisual();
    }

}