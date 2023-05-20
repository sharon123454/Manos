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
    [SerializeField] private Image actionImage, bonusActionImage;
    [SerializeField] private int brotherIndex;
    private Color actionBarDefualtColor;
    private Color BonusactionBarDefualtColor;

    private Unit specificBro;
    private string thisUnitName;
    void Start()
    {
        actionBarDefualtColor = actionImage.color;
        BonusactionBarDefualtColor = bonusActionImage.color;
        StartCoroutine(delayIni());
    }

    IEnumerator delayIni()
    {
        yield return new WaitForSeconds(0.5f);
        unit = UnitManager.Instance.GetFriendlyUnitList();
        specificBro = unit[brotherIndex];
        thisUnitName = specificBro.name;

    }
    void Update()
    {

        if (specificBro != null)
        {
            if (specificBro.GetUnitStats().health >= 0)
            {
                if (UnitActionSystem.Instance.GetSelectedUnit().name == thisUnitName)
                {
                    if (UnitActionSystem.Instance.GetSelectedAction().GetIsBonusAction())
                    {
                        bonusActionImage.color = Color.green;
                        actionImage.color = actionBarDefualtColor;
                    }
                    else
                    {
                        actionImage.color = Color.green;
                        bonusActionImage.color = BonusactionBarDefualtColor;
                    }
                }
                else
                {
                    actionImage.color = actionBarDefualtColor;
                    bonusActionImage.color = BonusactionBarDefualtColor;
                }


                if (specificBro.GetUsedActionPoints())
                    actionGrayedOut.SetActive(true);
                else
                    actionGrayedOut.SetActive(false);

                if (specificBro.GetUsedBonusActionPoints())
                    bonusActionGrayedOut.SetActive(true);
                else
                    bonusActionGrayedOut.SetActive(false);

                healthBar.fillAmount = unit[brotherIndex].GetHealthNormalized();
                postureBar.fillAmount = unit[brotherIndex].GetPostureNormalized();
                shieldAmountText.text = unit[brotherIndex].GetUnitStats().GetArmor().ToString();
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}