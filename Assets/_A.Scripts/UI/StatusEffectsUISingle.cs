using System.Collections.Generic;
using UnityEngine.EventSystems;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StatusEffectsUISingle : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private Image imageUGUI;
    [SerializeField] private TextMeshProUGUI valueTMPro;
    [SerializeField] private GameObject statusInfoGroup;
    [SerializeField] private TextMeshProUGUI statusNameTMPro;
    [SerializeField] private TextMeshProUGUI statusInfoTMPro;
    [SerializeField] private TextMeshProUGUI turnsLeftTMPro;
    [SerializeField] private float timeBetweerIncrease = 0.1f;
    [SerializeField] private float aphlaIncrease = 0.15f;
    [SerializeField] private int _framesToOpenInfo = 5;

    private static Coroutine _InfoActivationCoroutine;
    private WaitForSeconds waitForFadeIn;
    private bool _isHovered;

    private void Awake()
    {
        waitForFadeIn = new WaitForSeconds(timeBetweerIncrease);
    }

    public void Init(string StatusName, Sprite myStatusImage, string StatusInfo, int statusDuration)
    {
        imageUGUI.sprite = myStatusImage;
        statusNameTMPro.text = StatusName;
        statusInfoTMPro.text = StatusInfo;
        UpdateStatusEffect(statusDuration);
        StartCoroutine(FadeIn());
    }
    public void UpdateStatusEffect(int statusDuration)
    {
        valueTMPro.text = statusDuration.ToString();
        turnsLeftTMPro.text = $"Turns left:{statusDuration}";
    }

    private IEnumerator ActivateInfo()
    {
        for (int i = 0; i < _framesToOpenInfo; i++)
            yield return null;

        if (statusInfoGroup && _isHovered)
            statusInfoGroup.SetActive(true);
    }
    private IEnumerator FadeIn()
    {
        float currentAlphaColor = imageUGUI.color.a;
        while (imageUGUI.color.a <= 1)
        {
            yield return waitForFadeIn;
            currentAlphaColor = Mathf.Clamp(currentAlphaColor + aphlaIncrease, 0, 1);
            imageUGUI.color = new Color(1, 1, 1, currentAlphaColor);
        }
        imageUGUI.color = Color.white;
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isHovered = true;
        UnitActionSystem.Instance.SetHoveringOnUI(true);

        _InfoActivationCoroutine = StartCoroutine(ActivateInfo());
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isHovered = false;
        UnitActionSystem.Instance.SetHoveringOnUI(false);

        if (statusInfoGroup && statusInfoGroup.activeSelf)
        {
            statusInfoGroup.SetActive(false);
        }
    }

}