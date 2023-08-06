using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Spine.Unity;
using TMPro;

public class BrothersUI : MonoBehaviour
{
    [SerializeField] private StatusEffectsUI statusEffectUI;
    [SerializeField] private GameObject actionGrayedOut;
    [SerializeField] private GameObject bonusActionGrayedOut;
    [SerializeField] private TextMeshProUGUI shieldAmountText;
    [SerializeField] private TextMeshProUGUI healthText, postureText;
    [SerializeField] private Image healthBar, postureBar;
    [SerializeField] private Image actionImage, bonusActionImage;
    [SerializeField] SkeletonGraphic actionSpine, bonusActionSpine;

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

        statusEffectUI.InitStatusUI(specificBro);
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
                            ActionSpineBlinking();
                            BonusActionSpineBlinking();
                        }
                        else if (UnitActionSystem.Instance.GetSelectedAction().GetIsBonusAction())
                        {
                            ActionSpineBlinking();
                            BonusActionSpineStatic();
                        }
                        else
                        {
                            BonusActionSpineBlinking();
                            ActionSpineStatic();
                        }
                    }
                }
                else
                {
                    ActionSpineStatic();
                    BonusActionSpineStatic();
                }


                if (specificBro.GetUsedActionPoints())
                    ActionSpineNone();
                else
                    ActionSpineStatic();

                if (specificBro.GetUsedBonusActionPoints())
                    BonusActionSpineNone();
                else
                    BonusActionSpineStatic();

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
    void ActionSpineBlinking() { actionSpine.AnimationState.SetAnimation(index, "Action Blinking", true); }
    void ActionSpineStatic() { actionSpine.AnimationState.SetAnimation(0, "Action Blinking", true); }
    void ActionSpineNone() { actionSpine.AnimationState.SetAnimation(0, "None", true); }

    public int index;
    void BonusActionSpineBlinking() { bonusActionSpine.AnimationState.SetAnimation(index, "BA Blinking", true); }
    void BonusActionSpineStatic() { bonusActionSpine.AnimationState.SetAnimation(0, "BA Static", true); }
    void BonusActionSpineNone() { bonusActionSpine.AnimationState.SetAnimation(0, "None", true); }

}