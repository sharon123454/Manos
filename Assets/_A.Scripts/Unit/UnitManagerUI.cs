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
        UnitManager.GameLost += UnitManager_GameLost;
        UnitManager.GameWon += UnitManager_GameWon;
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
        SetNewMainUnit(UnitActionSystem.Instance.GetSelectedUnit().GetUnitUI());

    }

    private void Instance_OnSelectedUnitChanged(object sender, Unit e)
    {
        if (!e.IsEnemy())
            SetNewMainUnit(e.GetUnitUI());
    }

    /// <summary>
    /// Temporary
    /// </summary>
    /// <returns></returns>
    public void OnYesButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnBrotherUIPressed(string brotherName)
    {
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