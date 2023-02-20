using System.Collections;
using System.Collections.Generic;
using AmazingAssets.Beast.ExampleScripts;
using UnityEngine;
using SceneManager = UnityEngine.SceneManagement.SceneManager;

public class RestartGame : MonoBehaviour
{

    public void _RestartGame()
    {
        string scene = SceneManager.GetActiveScene().name;
        SceneManager.LoadScene(scene);
    }

    private void SceneManager_sceneLoaded(UnityEngine.SceneManagement.Scene arg0, UnityEngine.SceneManagement.LoadSceneMode arg1)
    {
        throw new System.NotImplementedException();
    }
}
