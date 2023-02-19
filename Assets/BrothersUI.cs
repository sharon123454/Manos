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
    private Unit specificBro;

    void Start()
    {
        StartCoroutine(delayIni());
    }

    IEnumerator delayIni()
    {
        yield return new WaitForSeconds(0.5f);
        unit = UnitManager.Instance.GetFriendlyUnitList();
        specificBro = unit[brotherIndex];
    }
    void Update()
    {

        if (specificBro != null)
        {
            if (specificBro.GetUnitStats().health >= 0)
            {
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