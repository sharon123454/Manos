using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

[RequireComponent(typeof(Button))]
public class ActionButtonUI : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public static event EventHandler<ActionButtonUI> OnAnyActionButtonPressed;

    [SerializeField] private ActionInfo actionInfo;
    [SerializeField] private Sprite _grayImage;
    [SerializeField] private Sprite _selectedImage;
    [SerializeField] private Sprite _unselectedImage;
    [SerializeField] private int _framesToOpenInfo = 5;

    [Header("Ability & Spell slots")]
    [SerializeField] private GameObject _cooldownBg;
    [SerializeField] private Image _cooldownCircleImage;
    [SerializeField] private TextMeshProUGUI _currentCooldownAmountTMP;
    [SerializeField] private bool _isBasicAction;

    [Header("Dev Tools:")]
    [Tooltip("Refreash purposes")]
    [SerializeField] private HorizontalOrVerticalLayoutGroup _layoutGroup;

    private static Coroutine _InfoActivationCoroutine;
    private BaseAction _myAction;
    private Button _myButton;
    private bool _isHovered;
    private Image _myImage;

    private void Awake()
    {
        _myImage = GetComponent<Image>();
        _myButton = GetComponent<Button>();
        _myButton.onClick.AddListener((/*anonymouseFunction*/) =>
        {
            ActionButtonPressed();
        });

        OnAnyActionButtonPressed += ActionButtonUI_OnAnyActionButtonPressed;
    }
    private void OnDestroy()
    {
        OnAnyActionButtonPressed -= ActionButtonUI_OnAnyActionButtonPressed;
    }

    public void PressButton() { _myButton.onClick.Invoke(); }
    public bool GetIsBaseActionButton() { return _isBasicAction; }
    public BaseAction GetAction() { return _myAction; }

    public void SetButtonAction(BaseAction baseAction)
    {
        //caching the current action to the button
        _myAction = baseAction;

        //setting button function by cached action except ability & spell button contaners
        if (!(_isBasicAction && !_myAction))
        {
            if (_myButton && _myAction)
            {
                _myButton.onClick.RemoveAllListeners();
                _myButton.onClick.AddListener(() =>
                {
                    UnitActionSystem.Instance.SetSelectedAction(_myAction);
                    UnitActionSystem.Instance.savedAction = _myAction;
                    ActionButtonPressed();
                });
            }
        }

        if (_myAction)
        {
            //getting ability images
            if (_myImage && _myAction.GetAbilityImage())
            {
                _grayImage = _myAction.GetAbilityGrayImage();
                _selectedImage = _myAction.GetAbilityImage();
                _myImage.sprite = _selectedImage;
            }

            //update the info tab with the current action
            if (actionInfo)
                actionInfo.UpdateInfoData(_myAction);
        }

        UpdateButtonVisual();
    }
    public void UpdateButtonVisual()
    {
        if (_myAction && TurnSystem.Instance.IsPlayerTurn())//if action not null and is player turn
        {
            Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
            if (_grayImage)//if action button has gray image
            {
                switch (_myAction.GetActionCost())//split by the action cost
                {
                    case TypeOfAction.Action:
                        if (selectedUnit.GetUsedAction() || UnitCantUseAction(selectedUnit, _myAction))
                        {
                            _myImage.sprite = _grayImage;
                        }
                        break;
                    case TypeOfAction.BonusAction:
                        if (selectedUnit.GetUsedBonusAction() || UnitCantUseAction(selectedUnit, _myAction))
                        {
                            _myImage.sprite = _grayImage;
                        }
                        break;
                    case TypeOfAction.Both:
                        if (selectedUnit.GetUsedBonusAction() || selectedUnit.GetUsedAction() || UnitCantUseAction(selectedUnit, _myAction))
                        {
                            _myImage.sprite = _grayImage;
                        }
                        break;
                }

                if (_cooldownBg && _cooldownBg.activeSelf) { _myImage.sprite = _grayImage; }//if cooldown is active make gray
            }

            if (_myAction.GetCurrentCooldown() <= 0)
            {
                //setting the cooldown amount to empty
                if (_currentCooldownAmountTMP)
                    _currentCooldownAmountTMP.text = "";
                //setting the circle (cooldown visual) to 0
                if (_cooldownCircleImage)
                    _cooldownCircleImage.fillAmount = 0;
                //turning off the rayblocking background
                if (_cooldownBg)
                    _cooldownBg.SetActive(false);
            }
            else
            {
                //activating the rayblocking background
                if (_cooldownBg)
                    _cooldownBg.SetActive(true);
                //setting the cooldown amount to current cooldown amount
                if (_currentCooldownAmountTMP)
                    _currentCooldownAmountTMP.text = _myAction.GetCurrentCooldown().ToString();
                //setting the circle (cooldown visual) to it's percentage
                if (_cooldownCircleImage)
                    _cooldownCircleImage.fillAmount = (float)_myAction.GetCurrentCooldown() / (float)_myAction.GetAbilityCooldown();
            }
        }
    }
    private bool UnitCantUseAction(Unit selectedUnit, BaseAction action)//tied to UpdateButtonVisual
    {
        return action is BaseAbility && !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(action.GetFavorCost())//if Action is Ability and don't have favor
        || selectedUnit.unitStatusEffects.ContainsEffect(StatusEffect.Root) && action.GetRange() == ActionRange.Move//if Rooted and Action is Movement
        || selectedUnit.unitStatusEffects.ContainsEffect(StatusEffect.Silence) && !action.IsXPropertyInAction(AbilityProperties.Basic)//if Silenced and Action isn't basic
        || selectedUnit.unitStatusEffects.ContainsEffect(StatusEffect.Stun);//if Unit is Stunned
    }

    private void ActionButtonPressed()
    {
        Debug.Log($"Object name: {name} has been pressed");
        OnAnyActionButtonPressed?.Invoke(this, this);
    }

    private IEnumerator ActivateInfoUI()
    {
        for (int i = 0; i < _framesToOpenInfo; i++)
            yield return null;

        if (actionInfo && _isHovered)
        {
            actionInfo.gameObject.SetActive(true);
            for (int i = 0; i < 3; i++)
            {
                yield return null;
                _layoutGroup.enabled = false;
                _layoutGroup.enabled = true;
            }
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        UnitActionSystem.Instance.SetHoveringOnUI(true);
        CursorManager.Instance.SetClickableCursor();

        if (!_myAction) { return; }
        UnitActionSystem.Instance.SetSelectedAction(_myAction);

        _InfoActivationCoroutine = StartCoroutine(ActivateInfoUI());

        //change cursor?
        //if (!OnCooldown.activeInHierarchy && baseAction.GetFavorCost() <= MagicSystem.Instance.GetCurrentFavor())
        //else
        //    CursorManager.Instance.SetBlockableCursor();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        CursorManager.Instance.SetDefaultCursor();
        UnitActionSystem.Instance.SetHoveringOnUI(false);

        if (actionInfo && actionInfo.gameObject.activeSelf)
        {
            actionInfo.gameObject.SetActive(false);
        }

        UnitActionSystem.Instance.SetSelectedAction(UnitActionSystem.Instance.savedAction);
        if (_myAction is MoveAction) { return; }//???
    }

    private void ActionButtonUI_OnAnyActionButtonPressed(object sender, ActionButtonUI pressedButton)
    {
        if (pressedButton)
        {
            if (_selectedImage && pressedButton == this)//pressed on an action button which has a selected Image
            {
                _myImage.sprite = _selectedImage;
            }
            else if (_unselectedImage && pressedButton != this && pressedButton.GetIsBaseActionButton())//basic action button pressed and this isn't it and has the unselected image
            {
                _myImage.sprite = _unselectedImage;
            }
            else if (_myAction)//if non basic action button pressed
            {

            }
        }
    }

}