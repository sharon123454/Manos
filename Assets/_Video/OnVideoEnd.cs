using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using UnityEngine.Video;
using UnityEngine;

public class OnVideoEnd : MonoBehaviour
{
    private VideoPlayer _videoPlayer;
    public InputAction skip;

    private void Awake()
    {
        _videoPlayer = GetComponent<VideoPlayer>();
        _videoPlayer.loopPointReached += VideoPlayer_loopPointReached;
        skip.performed += Skip_performed;
    }
    private void OnEnable()
    {
        skip.Enable();
    }
    private void OnDisable()
    {
        skip.Disable();
    }

    private void Skip_performed(InputAction.CallbackContext obj)
    {
        print("lol0");
        _videoPlayer.time = _videoPlayer.length - 2;
    }

    private void VideoPlayer_loopPointReached(VideoPlayer source)
    {
        SceneManager.LoadScene(1);
    }

}