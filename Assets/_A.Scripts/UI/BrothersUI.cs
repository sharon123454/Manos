using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class BrothersUI : MonoBehaviour
{
    [SerializeField] private GameObject actionGrayedOut;
    [SerializeField] private GameObject bonusActionGrayedOut;
    [SerializeField] private TextMeshProUGUI shieldAmountText;
    [SerializeField] private TextMeshProUGUI healthText,postureText;
    [SerializeField] private Image healthBar, postureBar;
    [SerializeField] private Image actionImage, bonusActionImage;

    [Header("Dev Data:")]
    [SerializeField] private float delayInitTime = 0.5f;

    private Color actionBarDefualtColor;
    private Color BonusactionBarDefualtColor;
    private WaitForSeconds initDelay;
    [SerializeField] private Unit specificBro;

    void Start()
    {
        initDelay = new WaitForSeconds(delayInitTime);
        BonusactionBarDefualtColor = bonusActionImage.color;
        actionBarDefualtColor = actionImage.color;
        StartCoroutine(InitBrotherUI());
    }

    IEnumerator InitBrotherUI()
    {
        yield return initDelay;

        List<Unit> unitLlist = UnitManager.Instance.GetFriendlyUnitList();

        string fixedName = "";
        for (int i = 0; i < name.Length - 2; i++)
            fixedName += name[i];

        foreach (Unit singleUnit in unitLlist)
            if (fixedName == singleUnit.name)
                specificBro = singleUnit;
    }

    void Update()
    {
        if (specificBro != null)
        {
            if (specificBro.GetUnitStats().health >= 0)
            {
                if (UnitActionSystem.Instance.GetSelectedUnit() == specificBro)
                {
                    if (UnitActionSystem.Instance.GetSelectedAction() != null)
                    {
                        if (UnitActionSystem.Instance.GetSelectedAction().ActionUsingBoth())
                        {
                            bonusActionImage.color = Color.gray;
                            actionImage.color = Color.gray;
                        }
                        else if (UnitActionSystem.Instance.GetSelectedAction().GetIsBonusAction())
                        {
                            bonusActionImage.color = Color.gray;
                            actionImage.color = actionBarDefualtColor;
                        }
                        else
                        {
                            actionImage.color = Color.gray;
                            bonusActionImage.color = BonusactionBarDefualtColor;
                        }
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

                healthBar.fillAmount = specificBro.GetHealthNormalized();
                postureBar.fillAmount = specificBro.GetPostureNormalized();
                shieldAmountText.text = specificBro.GetUnitStats().GetArmor().ToString();


                healthText.text = $"{specificBro.GetUnitStats().health} / {specificBro.GetUnitStats().GetUnitMaxHP()}";
                postureText.text = $"{specificBro.GetUnitStats().GetPosture()} / {specificBro.GetUnitStats().GetUnitMaxPosture()}";
            }
            else
            {
                gameObject.SetActive(false);
            }
        }
    }
}