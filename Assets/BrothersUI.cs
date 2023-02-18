using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrothersUI : MonoBehaviour
{
    [SerializeField] private List<Unit> unit;
    [SerializeField] private GameObject actionGrayedOut;
    [SerializeField] private GameObject bonusActionGrayedOut;
    [SerializeField] private int brotherIndex;

    void Start()
    {
        unit = UnitManager.Instance.GetFriendlyUnitList();
    }
    void Update()
    {
        
        print(unit[brotherIndex].name);
        if (unit[brotherIndex].GetUsedActionPoints())
            actionGrayedOut.SetActive(true);
        else
            actionGrayedOut.SetActive(false);

        if (unit[brotherIndex].GetUsedBonusActionPoints())
            bonusActionGrayedOut.SetActive(true);
        else
            bonusActionGrayedOut.SetActive(false);
    }
}
