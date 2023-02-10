using System.Collections;
using System.Collections.Generic;
using AmazingAssets.Beast.ExampleScripts;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class RestartGame : MonoBehaviour
{

    public void _RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
