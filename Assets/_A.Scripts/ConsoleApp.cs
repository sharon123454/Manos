using System.Collections.Generic;
using UnityEngine.InputSystem;
using System.Collections;
using UnityEngine;
using TMPro;

public class ConsoleApp : MonoBehaviour
{
    public static ConsoleApp Instance;

    [SerializeField] private GameObject settingsMenu;
    [SerializeField] private Transform consoleLinePrefab;
    [SerializeField] private Transform textContainer;

    private void Start()
    {
        Unit.SendConsoleMessage += EventPrint;
        UnitStats.SendConsoleMessage += EventPrint;
        //UnitStatusEffects.SendConsoleMessage += EventPrint;
        ManosInputController.Instance.OpenSettings.performed += InputController_Pause;
    }

    private void OnDisable()
    {
        ManosInputController.Instance.OpenSettings.performed -= InputController_Pause;
    }

    public void CloseGame()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void EventPrint(object sender, string name)
    {
        if (textContainer != null)
        {
            if (textContainer.childCount > 4)
                Destroy(textContainer.GetChild(0).gameObject);

            GameObject newConsoleLine = Instantiate(consoleLinePrefab.gameObject, textContainer);
            TextMeshProUGUI newTextLine = newConsoleLine.GetComponentInChildren<TextMeshProUGUI>();
            newTextLine.text = name;
        }
    }

    private void InputController_Pause(InputAction.CallbackContext context)
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

}