using System.Collections.Generic;
using UnityEngine.InputSystem;
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

        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;
        BaseAction.OnAnyActionStarted += BaseAction_OnAnyActionStarted;
        ActionButtonUI.OnAnyActionButtonPressed += ActionButtonUI_OnAnyActionButtonPressed;
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        ManosInputController.Instance.SelectActionWithNumbers.performed += ManosInputController_SetSelectedAction;

        SetBasicActionButtons();
    }
    private void OnDisable()
    {
        TurnSystem.Instance.OnTurnChange -= TurnSystem_OnTurnChange;
        BaseAction.OnAnyActionStarted -= BaseAction_OnAnyActionStarted;
        ActionButtonUI.OnAnyActionButtonPressed -= ActionButtonUI_OnAnyActionButtonPressed;
        UnitActionSystem.Instance.OnSelectedUnitChanged -= UnitActionSystem_OnSelectedUnitChanged;
        ManosInputController.Instance.SelectActionWithNumbers.performed -= ManosInputController_SetSelectedAction;
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
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        if (selectedUnit)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
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

        UpdateActionSystemButtons();
    }
    private void UpdateActionSystemButtons()
    {
        foreach (ActionButtonUI actionButtonUI in _activeActionButtonsUIList)
            if (actionButtonUI.isActiveAndEnabled)
                actionButtonUI.UpdateButtonVisual();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        SetBasicActionButtons();
        _basicActionButtonUIList[1].PressButton();//move button clicked

        if (_actionsButtonContainer)
        {
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void BaseAction_OnAnyActionStarted(object sender, EventArgs e)
    {
        if (!TurnSystem.Instance.IsPlayerTurn()) { return; }

        UpdateActionSystemButtons();
        _basicActionButtonUIList[1].PressButton();//move button clicked

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
            _basicActionButtonUIList[1].PressButton();//move button clicked
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void ActionButtonUI_OnAnyActionButtonPressed(object sender, ActionButtonUI buttonClicked)
    {
        if (!buttonClicked) { return; }

        UpdateActionSystemButtons();
        BaseAction clickedAction = buttonClicked.GetAction();
        if (_actionsButtonContainer && clickedAction && clickedAction.IsBasicAbility())
        {
            _actionsButtonContainer.gameObject.SetActive(false);
        }
    }
    private void ManosInputController_SetSelectedAction(InputAction.CallbackContext inputValue)
    {
        if (UnitActionSystem.Instance.isBusy || !TurnSystem.Instance.IsPlayerTurn()) { return; }

        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();

        int passedInput = (int)inputValue.ReadValue<float>();

        if (passedInput >= _basicActionButtonUIList.Count) { return; }

        BaseAction selectedAction = _activeActionButtonsUIList[passedInput].GetAction();
        int actionCost = (int)selectedAction.GetActionCost();

        if (actionCost == 0 && selectedUnit.GetUsedAction()) { return; }
        else if (actionCost == 1 && selectedUnit.GetUsedBonusAction()) { return; }
        else if (actionCost == 2 && (selectedUnit.GetUsedAction() || selectedUnit.GetUsedBonusAction())) { return; }

        _basicActionButtonUIList[passedInput].PressButton();
    }

}