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
    [SerializeField] private Sprite _selectedImage;
    [SerializeField] private Sprite _unselectedImage;
    [SerializeField] private int _framesToOpenInfo = 5;

    [Header("Ability & Spell slots")]
    [SerializeField] private Image _abilityButtonUIImage;
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

    public void ActionButtonPressed()
    {
        Debug.Log($"Object name: {name} has been pressed");
        OnAnyActionButtonPressed?.Invoke(this, this);
    }
    public void PressButton() { _myButton.onClick.Invoke(); }
    public bool GetIsBaseActionButton() { return _isBasicAction; }
    public BaseAction GetAction() { return _myAction; }
    //will need rework for when we get ability image in grayscale VvvvvV
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
            //setting ability image if it has an image
            if (_abilityButtonUIImage && _myAction.GetAbilityImage())
            {
                _selectedImage = _myAction.GetAbilityImage();
                _abilityButtonUIImage.sprite = _selectedImage;
                //if (!_cooldownBg.activeSelf) { } <----------------------------------when we get grayscaled UI 
            }

            //update the info tab with the current action
            if (actionInfo)
                actionInfo.UpdateInfoData(_myAction);
        }
    }
    public void UpdateButtonCoolDown()
    {
        if (_myAction && TurnSystem.Instance.IsPlayerTurn())
        {
            //Unit selectedunit = UnitActionSystem.Instance.GetSelectedUnit();
            //if (UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Silence) && !_myAction.GetAbilityPropertie().Contains(AbilityProperties.Basic)
            //    || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Stun)
            //    || UnitActionSystem.Instance.GetSelectedUnit().unitStatusEffects.ContainsEffect(StatusEffect.Root) && _myAction.GetRange() == ActionRange.Move
            //    || _myAction.GetIsBonusAction() && selectedunit.GetUsedBonusActionPoints()
            //    || !_myAction.GetIsBonusAction() && selectedunit.GetUsedActionPoints()
            //    || !MagicSystem.Instance.CanFriendlySpendFavorToTakeAction(_myAction.GetFavorCost())
            //    || !_myAction.IsBasicAbility()
            //    /*|| baseAction is BaseAbility && MagicSystem.Instance.GetCurrentFavor() <= 0*/)
            //{ }

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

    //public static void UnselectButtons()//- needs to see if needed
    //{
    //    OnAnyActionButtonPressed?.Invoke(null, null);
    //}

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
        //related to UnselectButtons, for now it always goes to Move
        //if (!pressedButton)
        //{
        //    if (_unselectedImage)
        //        _myImage.sprite = _unselectedImage;
        //    return;
        //}

        if (pressedButton)
        {
            if (_selectedImage && pressedButton == this)
            {
                _myImage.sprite = _selectedImage;
            }
            else if (_unselectedImage && pressedButton != this && pressedButton.GetIsBaseActionButton())
            {
                _myImage.sprite = _unselectedImage;
            }
            else if (_myAction)
            {

            }
        }
    }

}