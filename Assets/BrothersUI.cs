using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BrothersUI : MonoBehaviour
{
    [SerializeField] private List<Unit> unit;
    [SerializeField] private GameObject actionGrayedOut;
    [SerializeField] private GameObject bonusActionGrayedOut;
    [SerializeField] private TextMeshProUGUI shieldAmountText;
    [SerializeField] private Image healthBar, postureBar;
    [SerializeField] private int brotherIndex;

    void Start()
    {
        unit = UnitManager.Instance.GetFriendlyUnitList();
    }

    void Update()
    {
        if (unit[brotherIndex] != null)
        {
            if (unit[brotherIndex].GetUsedActionPoints())
                actionGrayedOut.SetActive(true);
            else
                actionGrayedOut.SetActive(false);

            if (unit[brotherIndex].GetUsedBonusActionPoints())
                bonusActionGrayedOut.SetActive(true);
            else
                bonusActionGrayedOut.SetActive(false);

            healthBar.fillAmount = unit[brotherIndex].GetHealthNormalized();
            postureBar.fillAmount = unit[brotherIndex].GetPostureNormalized();
            shieldAmountText.text = unit[brotherIndex].GetUnitStats().GetArmor().ToString();
        }
    }
}