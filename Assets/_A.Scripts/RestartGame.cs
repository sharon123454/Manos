using SceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void _RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}