using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MagicSystemUI : MonoBehaviour
{
    [SerializeField] private Image playerBar;
    [SerializeField] private Image enemyBar;
    [Header("Favor Stones")]
    [SerializeField] private Image[] activeFavorStones;
    [Tooltip("How fast the bar moves on change")]
    [SerializeField] private float favorChangeSpeed = 2f;

    private float favorValue = 0.5f;

    private void Awake()
    {
        MagicSystem.OnFavorChanged += MagicSystem_OnFavorChanged;
    }

    private void Update()
    {
        BaseAction selectedAbility = UnitActionSystem.Instance.GetSelectedAction();
        if (selectedAbility && selectedAbility is BaseAbility && selectedAbility.GetFavorCost() > 0)//if there is a selected action & it's an ability & costs favor
        {
            //lerp bar in lower percentage of favor cost
            playerBar.fillAmount = Mathf.Lerp(playerBar.fillAmount, favorValue - (selectedAbility.GetFavorCost() / MagicSystem.Instance.GetMaxFavor()), Time.deltaTime * favorChangeSpeed);
        }
        else//normal
        {
            HandleSmoothFavorChange();
        }
    }

    private void HandleSmoothFavorChange()
    {
        playerBar.fillAmount = Mathf.Lerp(playerBar.fillAmount, favorValue, Time.deltaTime * favorChangeSpeed);
        enemyBar.fillAmount = Mathf.Lerp(enemyBar.fillAmount, 1 - favorValue, Time.deltaTime * favorChangeSpeed);
    }

    private void MagicSystem_OnFavorChanged(object sender, float normalizedFavor)
    {
        favorValue = normalizedFavor;
    }

}