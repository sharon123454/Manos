using SceneManager = UnityEngine.SceneManagement.SceneManager;
using UnityEngine;

public class RestartGame : MonoBehaviour
{
    public void PlayAgain()
    {
        SceneManager.LoadScene(1);
    }
    public void _RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}