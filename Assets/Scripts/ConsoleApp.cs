using System.Collections.Generic;
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
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            settingsMenu.SetActive(!settingsMenu.activeSelf);
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
        if (textContainer.childCount > 4)
            Destroy(textContainer.GetChild(0).gameObject);

        GameObject newConsoleLine = Instantiate(consoleLinePrefab.gameObject, textContainer);
        TextMeshProUGUI newTextLine = newConsoleLine.GetComponentInChildren<TextMeshProUGUI>();
        newTextLine.text = textToPrint;
    }

}