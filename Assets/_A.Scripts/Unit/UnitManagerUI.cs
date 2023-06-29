using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject _LosePanel;
    [SerializeField] private GameObject _WinPanel;

    private void Start()
    {
        UnitManager.GameLost += UnitManager_GameLost;
        UnitManager.GameWon += UnitManager_GameWon;
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

    private void UnitManager_GameLost(object sender, EventArgs e)
    {
        _LosePanel.SetActive(true);
    }
    private void UnitManager_GameWon(object sender, EventArgs e)
    {
        _WinPanel.SetActive(true);
    }

}