using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class MagicSystemUI : MonoBehaviour
{
    [SerializeField] private Image frontBar;
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
        enemyFavorText.text = (MagicSystem.Instance.GetMaxFavor() -  currentFavor).ToString();
    }

}
