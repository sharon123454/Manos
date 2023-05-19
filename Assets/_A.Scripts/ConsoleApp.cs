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
        ManosInputController.Instance.OpenSettings.performed += InputController_Pause;
        UnityEngine.SceneManagement.SceneManager.sceneLoaded += SceneManager_sceneLoaded;
    }

    private void OnDisable()
    {
        ManosInputController.Instance.OpenSettings.performed -= InputController_Pause;
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        Unit.SendConsoleMessage += EventPrint;
        UnitStats.SendConsoleMessage += EventPrint;
    }

    public void EventPrint(object sender, string name)
    {
        CreateLine(name);
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

    private void CreateLine(string textToPrint)
    {
        if (textContainer != null)
        {
            if (textContainer.childCount > 4)
                Destroy(textContainer.GetChild(0).gameObject);

            GameObject newConsoleLine = Instantiate(consoleLinePrefab.gameObject, textContainer);
            TextMeshProUGUI newTextLine = newConsoleLine.GetComponentInChildren<TextMeshProUGUI>();
            newTextLine.text = textToPrint;
        }

    }

    private void InputController_Pause(InputAction.CallbackContext context)
    {
        settingsMenu.SetActive(!settingsMenu.activeSelf);
    }

}