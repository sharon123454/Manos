using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using Spine.Unity;
using TMPro;

public class BrothersUI : MonoBehaviour
{
    [SerializeField] private Image portraitImage;
    [SerializeField] private Sprite deadPortrait;
    [SerializeField] private StatusEffectsUI statusEffectUI;
    [SerializeField] private TextMeshProUGUI shieldAmountText;
    [SerializeField] private TextMeshProUGUI healthText, postureText;
    [SerializeField] private Image healthBar, postureBar;
    [SerializeField] SkeletonGraphic actionSpine, bonusActionSpine;

    [Header("Dev Data:")]
    [SerializeField] private float delayInitTime = 0.5f;

    private WaitForSeconds initDelay;
    [SerializeField] private Unit specificBro;

    void Start()
    {
        initDelay = new WaitForSeconds(delayInitTime);
        StartCoroutine(InitBrotherUI());
    }

    void Update()
    {
        if (specificBro != null)
        {
            if (specificBro.GetUnitStats().health >= 0)
            {
                if (UnitActionSystem.Instance.GetSelectedUnit() == specificBro)
                {
                    BaseAction selectedAction = UnitActionSystem.Instance.GetSelectedAction();
                    if (selectedAction)
                    {
                        switch (selectedAction.GetActionCost())
                        {
                            case TypeOfAction.Action:
                                ActionSpineBlinking();
                                BonusActionSpineStatic();
                                break;
                            case TypeOfAction.BonusAction:
                                BonusActionSpineBlinking();
                                ActionSpineStatic();
                                break;
                            case TypeOfAction.Both:
                                BonusActionSpineBlinking();
                                ActionSpineBlinking();
                                break;
                            default:
                                Debug.Log("I'm not supposed to be called");
                                break;
                        }
                    }
                }
                else
                {
                    BonusActionSpineStatic();
                    ActionSpineStatic();
                }
                //------------------------------------------------ elses look wierd
                if (specificBro.GetUsedAction())
                    BonusActionSpineNone();
                else
                    BonusActionSpineStatic();

                if (specificBro.GetUsedBonusAction())
                    ActionSpineNone();
                else
                    ActionSpineStatic();

                healthBar.fillAmount = specificBro.GetHealthNormalized();
                postureBar.fillAmount = specificBro.GetPostureNormalized();
                shieldAmountText.text = $" {specificBro.GetUnitStats().GetArmor()}";


                healthText.text = $"{specificBro.GetUnitStats().health} / {specificBro.GetUnitStats().GetUnitMaxHP()}";
                postureText.text = $"{specificBro.GetUnitStats().GetPosture()} / {specificBro.GetUnitStats().GetUnitMaxPosture()}";
            }
            else
            {
                //test once we have dead images
                //if (deadPortrait)
                    //portraitImage.sprite = deadPortrait;

                //this.enabled = false;
                gameObject.SetActive(false);
            }
        }
    }

    IEnumerator InitBrotherUI()
    {
        yield return initDelay;

        List<Unit> unitLlist = UnitManager.Instance.GetFriendlyUnitList();

        string fixedName = "";
        for (int i = 3; i < name.Length - 8; i++)
            fixedName += name[i];

        foreach (Unit singleUnit in unitLlist)
            if (fixedName == singleUnit.name)
                specificBro = singleUnit;

        if (specificBro)
            statusEffectUI.InitStatusUI(specificBro);
        else
            Debug.Log("Missing Unit reference");
    }

    void BonusActionSpineBlinking() { actionSpine.AnimationState.SetAnimation(index, "Action Blinking", true); }
    void BonusActionSpineStatic() { actionSpine.AnimationState.SetAnimation(0, "Action Blinking", true); }
    void BonusActionSpineNone() { actionSpine.AnimationState.SetAnimation(0, "None", true); }

    public int index;
    void ActionSpineBlinking() { bonusActionSpine.AnimationState.SetAnimation(index, "BA Blinking", true); }
    void ActionSpineStatic() { bonusActionSpine.AnimationState.SetAnimation(0, "BA Static", true); }
    void ActionSpineNone() { bonusActionSpine.AnimationState.SetAnimation(0, "None", true); }

}