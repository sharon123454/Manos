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
    [Space]
    [Header("AbilitiesContainer")]
    [SerializeField] private RectTransform AbilitiesContainerGameObject;
    float AbilitiesContainerInisialX;

    [Space]
    [Header("Abilties")]
    [SerializeField] private Image abilitiesOutLine;
    float abilitiesOutLineInitialX;
    float abilitiesOutLineInitialWidth;

    [Space]
    [Header("Spells")]
    float spellsOutLineInitialX;
    float spellsOutLineInitialWidth;
    private void Awake()
    {
        actionButtonUIList = new List<ActionButtonUI>(10);
        AbilitiesContainerInisialX = AbilitiesContainerGameObject.anchoredPosition.x;
        abilitiesOutLineInitialX = abilitiesOutLine.rectTransform.anchoredPosition.x;
        abilitiesOutLineInitialWidth = abilitiesOutLine.rectTransform.sizeDelta.x;
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
        float abilityContainerinitialX = AbilitiesContainerInisialX;
        float _initialX = abilitiesOutLineInitialX;
        float _initialWidth = abilitiesOutLineInitialWidth;
        AbilitiesContainerGameObject.anchoredPosition = new Vector2(abilityContainerinitialX, AbilitiesContainerGameObject.anchoredPosition.y);
        abilitiesOutLine.rectTransform.anchoredPosition = new Vector2(_initialX, abilitiesOutLine.rectTransform.anchoredPosition.y);
        abilitiesOutLine.rectTransform.sizeDelta = new Vector2(_initialWidth, abilitiesOutLine.rectTransform.sizeDelta.y);
        actionButtonUIList.Clear();

        if (selectedUnit)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                if (baseAction.isActiveAndEnabled && !baseAction.IsBasicAbility())
                {
                    AbilitiesContainerGameObject.anchoredPosition = new Vector2(abilityContainerinitialX, AbilitiesContainerGameObject.anchoredPosition.y);
                    abilitiesOutLine.rectTransform.anchoredPosition = new Vector2(_initialX, abilitiesOutLine.rectTransform.anchoredPosition.y);
                    abilitiesOutLine.rectTransform.sizeDelta = new Vector2(_initialWidth, abilitiesOutLine.rectTransform.sizeDelta.y);
                    Transform actionButtonTransform = Instantiate(actionButtonPrefab, actionButtonContainerTransform);
                    ActionButtonUI actionbuttonUI = actionButtonTransform.GetComponent<ActionButtonUI>();
                    actionbuttonUI.SetBaseAction(baseAction);
                    actionButtonUIList.Add(actionbuttonUI);
                    _initialX += 33f;
                    _initialWidth += 33 * 2;
                    abilityContainerinitialX -= 33f;
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