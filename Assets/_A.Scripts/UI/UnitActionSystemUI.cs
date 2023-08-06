using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using System;
using TMPro;

public class UnitActionSystemUI : MonoBehaviour
{
    [SerializeField] private Transform actionButtonPrefab, abilitesButtonContainerTransform, spellsButtonContainerTransform;
    private List<ActionButtonUI> actionButtonUIList;
    [Space]
    [Header("AbilitiesContainer")]
    [SerializeField] private RectTransform AbilitiesContainerGameObject;
    [SerializeField] private RectTransform SpellsContainerGameObject;
    float AbilitiesContainerInisialX;
    float SpellsContainerInisialX;

    [Space]
    [Header("Abilties")]
    [SerializeField] private Image abilitiesOutLine;
    [SerializeField] private Image spellsOutLine;
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

        SpellsContainerInisialX = SpellsContainerGameObject.anchoredPosition.x;
        spellsOutLineInitialX = spellsOutLine.rectTransform.anchoredPosition.x;
        spellsOutLineInitialWidth = spellsOutLine.rectTransform.sizeDelta.x;
    }

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += UnitActionSystem_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += UnitActionSystem_OnSelectedActionChanged;
        TurnSystem.Instance.OnTurnChange += TurnSystem_OnTurnChange;

        CreateUnitActionButtonsForAbilites();
        UpdateActionSystemVisuals();
    }
    private void Update()
    {
        
    }
    private void UnitActionSystem_OnSelectedActionChanged(object sender, EventArgs e)
    {
        UpdateActionSystemVisuals();
    }

    private void TurnSystem_OnTurnChange(object sender, EventArgs e)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
        {
            abilitiesOutLine.gameObject.SetActive(true);
            spellsOutLine.gameObject.SetActive(true);
        }
        else
        {
            abilitiesOutLine.gameObject.SetActive(false);
            spellsOutLine.gameObject.SetActive(false);
        }
        if (abilitesButtonContainerTransform)
            abilitesButtonContainerTransform.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());
        else
            spellsButtonContainerTransform.gameObject.SetActive(TurnSystem.Instance.IsPlayerTurn());

        UpdateActionSystemVisuals();
    }

    private void UnitActionSystem_OnSelectedUnitChanged(object sender, Unit newlySelectedUnit)
    {

        if (abilitesButtonContainerTransform.gameObject.activeInHierarchy)
            CreateUnitActionButtonsForAbilites();
        else
            CreateUnitActionButtonsForSpells();

        UpdateActionSystemVisuals();

    }

    public void CreateUnitActionButtonsForAbilites()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        AbilitiesContainerGameObject.gameObject.SetActive(true);
        SpellsContainerGameObject.gameObject.SetActive(false);
        foreach (Transform buttonTransform in abilitesButtonContainerTransform)
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
                if (baseAction.isActiveAndEnabled && !baseAction.IsBasicAbility() && baseAction.GetFavorCost() == 0)
                {
                    AbilitiesContainerGameObject.anchoredPosition = new Vector2(abilityContainerinitialX, AbilitiesContainerGameObject.anchoredPosition.y);
                    abilitiesOutLine.rectTransform.anchoredPosition = new Vector2(_initialX, abilitiesOutLine.rectTransform.anchoredPosition.y);
                    abilitiesOutLine.rectTransform.sizeDelta = new Vector2(_initialWidth, abilitiesOutLine.rectTransform.sizeDelta.y);
                    Transform actionButtonTransform = Instantiate(actionButtonPrefab, abilitesButtonContainerTransform);
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
    public void CreateUnitActionButtonsForSpells()
    {
        Unit selectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        SpellsContainerGameObject.gameObject.SetActive(true);
        AbilitiesContainerGameObject.gameObject.SetActive(false);
        foreach (Transform buttonTransform in spellsButtonContainerTransform)
            Destroy(buttonTransform.gameObject);
        float abilityContainerinitialX = SpellsContainerInisialX;
        float _initialX = spellsOutLineInitialX;
        float _initialWidth = spellsOutLineInitialWidth;
        SpellsContainerGameObject.anchoredPosition = new Vector2(abilityContainerinitialX, SpellsContainerGameObject.anchoredPosition.y);
        spellsOutLine.rectTransform.anchoredPosition = new Vector2(_initialX, spellsOutLine.rectTransform.anchoredPosition.y);
        spellsOutLine.rectTransform.sizeDelta = new Vector2(_initialWidth, spellsOutLine.rectTransform.sizeDelta.y);
        actionButtonUIList.Clear();

        if (selectedUnit)
        {
            foreach (BaseAction baseAction in selectedUnit.GetBaseActionArray())
            {
                if (baseAction.isActiveAndEnabled && !baseAction.IsBasicAbility() && baseAction.GetFavorCost() > 0)
                {
                    SpellsContainerGameObject.anchoredPosition = new Vector2(abilityContainerinitialX, SpellsContainerGameObject.anchoredPosition.y);
                    spellsOutLine.rectTransform.anchoredPosition = new Vector2(_initialX, spellsOutLine.rectTransform.anchoredPosition.y);
                    spellsOutLine.rectTransform.sizeDelta = new Vector2(_initialWidth, spellsOutLine.rectTransform.sizeDelta.y);
                    Transform actionButtonTransform = Instantiate(actionButtonPrefab, spellsButtonContainerTransform);
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