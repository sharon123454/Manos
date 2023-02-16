using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.Collections;
using UnityEngine;
using System;

public class UnitManagerUI : MonoBehaviour
{
    [SerializeField] private GameObject LosePanel;

    private void Start()
    {
        UnitManager.GameLost += UnitManager_GameLost;
    }

    /// <summary>
    /// Temporary
    /// </summary>
    /// <returns></returns>
    public void OnYesButtonClick()
    {
        SceneManager.LoadScene(0);
    }

    public void OnBrotherUIPressed(int brotherID)
    {
        UnitManager.Instance.TrySelectFriendlyUnit(brotherID);
    }

    private void UnitManager_GameLost(object sender, EventArgs e)
    {
        LosePanel.SetActive(true);
    }

}