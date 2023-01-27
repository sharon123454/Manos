using System.Collections.Generic;
using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class MagicSystemUI : MonoBehaviour
{
    [SerializeField] private Image frontBar;
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

    private void MagicSystem_OnFavorChanged(object sender, float favorNormalized )
    {
        favorValue = favorNormalized;
    }

}
