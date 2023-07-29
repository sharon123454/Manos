using System.Collections;
using System.Collections.Generic;
using System.Security.Policy;
using UnityEngine;
using UnityEngine.UI;

public class BasicActionSystemUI : MonoBehaviour
{
    [SerializeField] private List<GameObject> basicImageSprites;
    [SerializeField] private List<ActionButtonUI> actionbuttons;

    private void Start()
    {
        RefreshBasicAbilities();
        UnitActionSystem.Instance.OnSelectedUnitChanged += Instance_OnSelectedUnitChanged;
    }

    private void Instance_OnSelectedUnitChanged(object sender, Unit e)
    {
        RefreshBasicAbilities();
    }

    public void SelectBasicAbility(GameObject currentAbility)
    {
        foreach (var sprite in basicImageSprites)
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
    }
}
