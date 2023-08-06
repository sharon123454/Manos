using System.Collections.Generic;
using System.Collections;
using UnityEngine;

public class BasicActionSystemUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> basicSelectedImageSprites;
    [SerializeField] private List<GameObject> BasicUnSelectedImages;
    [SerializeField] private List<ActionButtonUI> actionbuttons;

    [SerializeField] private GameObject AttackUsed;
    [SerializeField] private GameObject MoveUsed;
    [SerializeField] private GameObject BlockUsed;
    [SerializeField] private GameObject DasheUsed;
    [SerializeField] private GameObject DodgeUsed;
    private void Start()
    {
        StartCoroutine(DelayStart());
        RefreshBasicAbilities();
        UnitActionSystem.Instance.OnActionStarted += Instance_OnActionStarted;
    }

    private void Instance_OnActionStarted(object sender, System.EventArgs e)
    {

    }

    private void Instance_OnSelectedActionChanged(object sender, System.EventArgs e)
    {

        //CheckIfUsedBasicAbility(actionbuttons[0], AttackUsed);
        //CheckIfUsedBasicAbility(actionbuttons[1], MoveUsed);
        //CheckIfUsedBasicAbility(actionbuttons[2], BlockUsed);
        //CheckIfUsedBasicAbility(actionbuttons[3], DasheUsed);
        //CheckIfUsedBasicAbility(actionbuttons[4], DodgeUsed);

    }
    public GameObject savedDisabledIcon;
    public void DisableObject(GameObject gameObject) { if(savedDisabledIcon) savedDisabledIcon.SetActive(true); savedDisabledIcon = gameObject; gameObject.SetActive(false); }
    private void CheckIfUsedBasicAbility(ActionButtonUI action, GameObject DisableHud)
    {
        if (DisableHud)
            DisableHud.SetActive(action.GetBaseAction().GetIfUsedAction());
    }
    private void UnselectBasicCanvas()
    {

        foreach (var item in BasicUnSelectedImages)
        {
            item.SetActive(true);
        }
        foreach (var item in basicSelectedImageSprites)
        {
            item.SetActive(false);
        }
    }
    private void Instance_OnSelectedUnitChanged(object sender, Unit e)
    {
        RefreshBasicAbilities();
        UnselectBasicCanvas();
      //  Unit SelectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (e.GetUsedActionPoints())
        {
            AttackUsed.SetActive(true);
            BlockUsed.SetActive(true);
            DasheUsed.SetActive(true);
            DodgeUsed.SetActive(true);
        }
        else
        {
            AttackUsed.SetActive(false);
            BlockUsed.SetActive(false);
            DasheUsed.SetActive(false);
            DodgeUsed.SetActive(false);
        }

        if (e.GetUsedBonusActionPoints())
            MoveUsed.SetActive(true);
        else
            MoveUsed.SetActive(false);
    }

    public void SelectBasicAbility(GameObject currentAbility)
    {
        foreach (var sprite in basicSelectedImageSprites)
        {
            sprite.SetActive(false);
        }
        currentAbility.SetActive(true);
    }

    public void RefreshBasicAbilities()
    {
        foreach (BaseAction baseAction in UnitActionSystem.Instance.GetSelectedUnit().GetBaseActionArray())
        {
            if (baseAction.isActiveAndEnabled && baseAction.IsBasicAbility())
            {
                if (baseAction.GetActionName() == "Basic Attack")
                {
                    actionbuttons[0].SetBaseAction(baseAction);
                }
                if (baseAction.GetActionName() == "Move")
                {
                    actionbuttons[1].SetBaseAction(baseAction);
                }
                if (baseAction.GetActionName() == "Block")
                {
                    actionbuttons[2].SetBaseAction(baseAction);
                }
                if (baseAction.GetActionName() == "Dash")
                {
                    actionbuttons[3].SetBaseAction(baseAction);
                }
                if (baseAction.GetActionName() == "Dodge")
                {
                    actionbuttons[4].SetBaseAction(baseAction);
                }
            }
        }
        Unit SelectedUnit = UnitActionSystem.Instance.GetSelectedUnit();
        if (SelectedUnit.GetUsedActionPoints())
        {
            AttackUsed.SetActive(true);
            BlockUsed.SetActive(true);
            DasheUsed.SetActive(true);
            DodgeUsed.SetActive(true);
        }
        else
        {
            AttackUsed.SetActive(false);
            BlockUsed.SetActive(false);
            DasheUsed.SetActive(false);
            DodgeUsed.SetActive(false);
        }

        if (SelectedUnit.GetUsedBonusActionPoints())
            MoveUsed.SetActive(true);
        else
            MoveUsed.SetActive(false);
    }

    private IEnumerator DelayStart()
    {
        yield return new WaitForSeconds(2f);
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
        UnitActionSystem.Instance.OnSelectedActionChanged += Instance_OnSelectedActionChanged;
    }

}