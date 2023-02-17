using System.Collections.Generic;
using System.Collections;
using AmazingAssets.Beast.ExampleScripts;
using UnityEngine;
using TMPro;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class ConsoleApp : MonoBehaviour
{
    public static ConsoleApp Instance;
    
    [SerializeField] private Transform consoleLinePrefab;
    [SerializeField] private Transform textContainer;
    [SerializeField] private TMP_InputField inputField;

    private void Start()
    {
        Unit.SendConsoleMessage += EventPrint;
        UnitStats.SendConsoleMessage += EventPrint;
    }


    public void EventPrint(object sender, string name)
    {
        CreateLine(name);
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