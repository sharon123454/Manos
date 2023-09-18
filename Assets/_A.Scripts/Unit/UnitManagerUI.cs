using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _LosePanel;
    [SerializeField] private GameObject _WinPanel;
    [Space]
    [SerializeField] GameObject SmallGroup;
    [SerializeField] GameObject SelectedMember;

    private void Start()
    {
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
        UnitManager.GameLost += UnitManager_GameLost;
        UnitManager.GameWon += UnitManager_GameWon;
    }

    private void Instance_OnSelectedUnitChanged(object sender, Unit selectedUnit)
    {
        if (TurnSystem.Instance.IsPlayerTurn())
            SetNewMainUnit(selectedUnit.GetUnitUI());
    }

    /// <summary>
    /// Pressed through buttons (OnYesButtonClick Temporary)
    /// </summary>
    /// <returns></returns>
    public void OnYesButtonClick()
    {
        SceneManager.LoadScene(0);
    }
    public void OnBrotherUIPressed(string brotherName)
    {
        if (UnitActionSystem.Instance.isBusy || !TurnSystem.Instance.IsPlayerTurn()) { return; }
        UnitManager.Instance.SelectFriendlyUnitWithUI(brotherName);
    }

    public void SetNewMainUnit(GameObject NewMain)
    {
        Transform OldMain;

        OldMain = SelectedMember.transform.GetChild(0);
        OldMain.SetParent(SmallGroup.transform);
        OldMain.localScale = Vector3.one;

        NewMain.transform.SetParent(SelectedMember.transform);
        NewMain.transform.localScale = Vector3.one;
        NewMain.GetComponent<RectTransform>().localPosition = Vector3.zero;
    }

    private void UnitManager_GameLost(object sender, EventArgs e)
    {
        _LosePanel.SetActive(true);
    }
    private void UnitManager_GameWon(object sender, EventArgs e)
    {
        _WinPanel.SetActive(true);
    }

}