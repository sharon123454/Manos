using System.Collections.Generic;
using UnityEngine;
using System;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private RectTransform actionButtonPrefab;
    [SerializeField] private RectTransform _actionsButtonContainer;//Container where the ActionButtonUI created
    [SerializeField] private RectTransform _abilityButtonsContainer;// -_actionsButtonContainer- moved to here when ability button pressed
    [SerializeField] private RectTransform _spellButtonsContainer;// -_actionsButtonContainer- moved to here when spell button pressed
    [SerializeField] private GameObject _subMenueFramePrefab;//Frame should be at the bottom of the hirarchy, so instantiated last (is this good?)
    [Tooltip("0-Basic Attack, 1-Move, 2-Dash, 3-Block, 4-Dodge")]
    [SerializeField] private List<ActionButtonUI> _basicActionButtonUIList;//Reference for the static basic abilities

    private List<ActionButtonUI> _activeActionButtonsUIList = new List<ActionButtonUI>(20);

    private void Start()
    {
        foreach (ActionButtonUI basicActionButton in _basicActionButtonUIList)
            _activeActionButtonsUIList.Add(basicActionButton);

        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        ActionButtonUI.OnAnyActionButtonPressed += ActionButtonUI_OnAnyActionButtonPressed;

        SetBasicActionButtons();
    }

    //Called through Buttons -forAbilities- set thought inspector; says if ability or spell pressed
    public void CreateUnitActionButtons(bool forAbilities)
    {
        //getting the current unit reference
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        //clearing the Button Data list
        _activeActionButtonsUIList.Clear();

        //clearing the Game Objects container parent
        foreach (Transform buttonTransform in _actionsButtonContainer)
            Destroy(buttonTransform.gameObject);

        //set container in parent
        if (forAbilities)
            _actionsButtonContainer.SetParent(_abilityButtonsContainer);
        else
            _actionsButtonContainer.SetParent(_spellButtonsContainer);

        //re-setting position
        _actionsButtonContainer.localPosition = new Vector3(0, 142.5f, 0);

        //activating GameObject
        _actionsButtonContainer.gameObject.SetActive(true);

        //re-adding basic actions to Updating list
        foreach (ActionButtonUI basicActionButton in _basicActionButtonUIList)
            _activeActionButtonsUIList.Add(basicActionButton);

        if (selectedUnit)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                if (baseAction.isActiveAndEnabled && !baseAction.IsBasicAbility())
                {
                    if (forAbilities && baseAction.GetFavorCost() == 0)
                    {
                        RectTransform newAbilityButton = Instantiate(actionButtonPrefab, _actionsButtonContainer);
                        ActionButtonUI abilityUI = newAbilityButton.GetComponentInChildren<ActionButtonUI>();
                        abilityUI.SetButtonAction(baseAction);
                        _activeActionButtonsUIList.Add(abilityUI);
                    }
                    else if (!forAbilities && baseAction.GetFavorCost() > 0)
                    {
                        RectTransform newSpellButton = Instantiate(actionButtonPrefab, _actionsButtonContainer);
                        ActionButtonUI spellUI = newSpellButton.GetComponentInChildren<ActionButtonUI>();
                        spellUI.SetButtonAction(baseAction);
                        _activeActionButtonsUIList.Add(spellUI);
                    }
                }
            }
        }

        Instantiate(_subMenueFramePrefab, _actionsButtonContainer);
        UpdateActionSystemButtons();
    }

    private void SetBasicActionButtons()
    {
        Unit SelectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (SelectedUnit)
        {
            foreach (BaseAction baseAction in SelectedUnit.GetBaseActionArray())
            {
                if (baseAction.enabled && baseAction.IsBasicAbility())
                {
                    if (baseAction.GetActionName() == "Basic Attack")
                    {
                        _basicActionButtonUIList[0].SetButtonAction(baseAction);
                    }
                    if (baseAction.GetActionName() == "Move")
                    {
                        _basicActionButtonUIList[1].SetButtonAction(baseAction);
                    }
                    if (baseAction.GetActionName() == "Dash")
                    {
                        _basicActionButtonUIList[2].SetButtonAction(baseAction);
                    }
                    if (baseAction.GetActionName() == "Block")
                    {
                        _basicActionButtonUIList[3].SetButtonAction(baseAction);
                    }
                    if (baseAction.GetActionName() == "Dodge")
                    {
                        _basicActionButtonUIList[4].SetButtonAction(baseAction);
                    }
                }
            }
        }
    }
    private void UpdateActionSystemButtons()
    {
        foreach (ActionButtonUI actionButtonUI in _activeActionButtonsUIList)
            if (actionButtonUI.isActiveAndEnabled)
                actionButtonUI.UpdateButtonCoolDown();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        SetBasicActionButtons();
        ActionButtonUI.UnselectButtons();

        if (_actionsButtonContainer)
        {
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        UpdateActionSystemButtons();
        ActionButtonUI.UnselectButtons();

        if (_actionsButtonContainer)
        {
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit newlySelectedUnit)
    {
        SetBasicActionButtons();

        if (_actionsButtonContainer)
        {
            ActionButtonUI.UnselectButtons();
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void ActionButtonUI_OnAnyActionButtonPressed(object sender, ActionButtonUI buttonClicked)
    {
        if (!buttonClicked) { return; }

        BaseAction clickedAction = buttonClicked.GetAction();
        if (_actionsButtonContainer && clickedAction && clickedAction.IsBasicAbility())
        {
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }

}