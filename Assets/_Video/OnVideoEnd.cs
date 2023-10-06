using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine.UI;
using UnityEngine;

public class OnVideoEnd : MonoBehaviour
{
    [SerializeField] private GameObject mainMenue;
    [SerializeField] private GameObject creditsWindow;
    [SerializeField] private Button startButton;
    [SerializeField] private Button creditButton;
    [SerializeField] private Button quitButton;

    public InputAction skip;

    private VideoPlayer _videoPlayer;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        skip.performed += Skip_performed;
        startButton.onClick.AddListener(() =>
        {
            OnStartButtonClicked();
        });
        creditButton.onClick.AddListener(() =>
        {
            OnCreditButtonClicked();
        });
        quitButton.onClick.AddListener(() =>
        {
            OnQuitButtonClicked();
        });
    }
    private void OnEnable()
    {
        skip.Enable();
    }
    private void OnDisable()
    {
        skip.Disable();
        quitButton.onClick.RemoveAllListeners();
        startButton.onClick.RemoveAllListeners();
        creditButton.onClick.RemoveAllListeners();
    }

    private void OnStartButtonClicked()
    {
        SceneManager.LoadScene(1);
    }
    private void OnCreditButtonClicked()
    {
        if (creditsWindow)
            creditsWindow.SetActive(true);
    }
    private void OnQuitButtonClicked()
    {
#if UNITY_STANDALONE
        Application.Quit();
#endif
#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }

    private void Skip_performed(InputAction.CallbackContext obj)
    {
        _videoPlayer.time = _videoPlayer.length - 2;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        mainMenue.SetActive(true);
    }

}