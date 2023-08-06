using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;
using TMPro;

public class StatusEffectsUISingle : MonoBehaviour
{
    [SerializeField] private Image imageUGUI;
    [SerializeField] private TextMeshProUGUI valueTMPro;
    [SerializeField] private float timeBetweerIncrease = 0.1f;
    [SerializeField] private float aphlaIncrease = 0.15f;

    private WaitForSeconds waitForFadeIn;
    private StatusEffect myEffect;

    private void Awake()
    {
        waitForFadeIn = new WaitForSeconds(timeBetweerIncrease);
    }

    public void Init(StatusEffect effect, Sprite myStatusImage)
    {
        myEffect = effect;
        imageUGUI.sprite = myStatusImage;
        StartCoroutine(FadeIn());
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

}