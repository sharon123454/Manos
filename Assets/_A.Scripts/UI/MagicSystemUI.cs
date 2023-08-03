using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MagicSystemUI : MonoBehaviour
{
    [SerializeField] private Image frontBar;
    [SerializeField] private Image indicator;
    [SerializeField] private TextMeshProUGUI friendlyFavorText, enemyFavorText;
    [Tooltip("How fast the bar moves on change")]
    [SerializeField] private float favorChangeSpeed = 1f;

    private float favorValue = 0.5f;

    private void Awake()
    {
        MagicSystem.OnFavorChanged += MagicSystem_OnFavorChanged;

    }

    private void Update()
    {
        HandleSmoothFavorChange();
        indicator.rectTransform.localPosition = new Vector3(CalculatePosition(MagicSystem.Instance.GetCurrentFavor() / MagicSystem.Instance.GetMaxFavor()), +16.5f, 0);
        if (UnitActionSystem.Instance.GetSelectedAction() is BaseAbility)
        {
            indicator.gameObject.SetActive(true);
            indicator.fillAmount = MagicSystem.Instance.GetCurrentFavor() / MagicSystem.Instance.GetMaxFavor() - (MagicSystem.Instance.GetCurrentFavor() - UnitActionSystem.Instance.GetSelectedAction().GetFavorCost()) / MagicSystem.Instance.GetMaxFavor();
        }
        else
            indicator.gameObject.SetActive(false);
    }

    private void HandleSmoothFavorChange()
    {
        frontBar.fillAmount = Mathf.Lerp(frontBar.fillAmount, favorValue, Time.deltaTime * favorChangeSpeed);

    }

    private void MagicSystem_OnFavorChanged(object sender, float currentFavor)
    {
        float normalizedFavor = currentFavor / MagicSystem.Instance.GetMaxFavor();
        favorValue = normalizedFavor;
        friendlyFavorText.text = currentFavor.ToString();
        enemyFavorText.text = (MagicSystem.Instance.GetMaxFavor() - currentFavor).ToString();
    }


    public static float CalculatePosition(float fillAmount)
    {
        // Given data points
        float position1 = 0f;   // Position when fill amount is 1
        float fillAmount1 = 1f; // Corresponding fill amount for position1
        float position2 = -760;   // Position when fill amount is 0
        float fillAmount2 = 0f; // Corresponding fill amount for position2

        // Linear interpolation formula
        float position = position1 + (fillAmount - fillAmount1) * (position2 - position1) / (fillAmount2 - fillAmount1);

        return position;
    }
}
