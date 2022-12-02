using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class ActionbuttonUI : MonoBehaviour
{
    [SerializeField] TextMeshProUGUI textMeshPro;
    [SerializeField] Button button;

    public void SetBaseAction(BaseAction baseAction)
    {
        textMeshPro.text = baseAction.GetActionName().ToUpper();
        button.onClick.AddListener(
            (/*anonymouseFunction*/) => 
            {
                UnitActionSystem.Instance.SetSelectedAction(baseAction);
            });
    }

}