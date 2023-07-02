using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource VFXSource;
    public AudioSource MusicSource;
    public AudioSource AmbiantSource;

    [Header("New VFX")]
    public AudioClip VFX;

    [Space]
    [Header("Music")]
    public AudioClip Music1;
    public AudioClip Music2;
    [Space]
    [Header("Main Menu")]
    public AudioClip mainMenu;
    public AudioClip gamePlay;
    [Space]
    [Header("Game Objects")]
    public GameObject muteIcon;
    public GameObject UnmuteIcon;

    public GameObject MusicMuteIcon;
    public GameObject MusicUnmuteIcon;
    // Random pitch adjustment range.
    public float LowPitchRange = .95f;
    public float HighPitchRange = 1.05f;

    int musicVolume;
    int soundVolume;
    int endOfGameSoundVolume;

    private bool toggleMute;
    private bool toggleMusicMute;
    // Singleton Instance.
    #region Singelton
    public static SoundManager Instance { get; private set; }
    private void Awake()
    {
        // If there is an Instance, and it's not me, delete myself.

        if (Instance != null && Instance != this)
        {
            Destroy(this);
        }
        else
        {
            Instance = this;
        }
    }
    #endregion
    // Initialize the singleton Instance.
    private void Start()
    {
        _PlayMusic(gamePlay);
    }


    public void Play(AudioClip clip)
    {
        VFXSource.clip = clip;
        VFXSource.Play();
    }
    // Play a single clip through the music source.
    public void PlayVFX(AudioClip clip)
    {
        MusicSource.clip = clip;
        MusicSource.Play();
    }
    public void _PlayMusic(AudioClip clip)
    {
        AmbiantSource.clip = clip;
        AmbiantSource.Play();
    }
    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(LowPitchRange, HighPitchRange);
        VFXSource.pitch = randomPitch;
        VFXSource.clip = clips[randomIndex];
        VFXSource.Play();
    }


    public void ToggleMute()
    {
        toggleMute = !toggleMute;

        if (!toggleMute)
        {
            PlayerPrefs.SetInt("SoundVolume", 0);
            VFXSource.volume = 1;
            MusicSource.volume = 1;
            UnmuteIcon.SetActive(true);
            muteIcon.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("SoundVolume", 1);
            VFXSource.volume = 0;
            MusicSource.volume = 0;
            UnmuteIcon.SetActive(false);
            muteIcon.SetActive(true);
        }
    }
    public void ToggleMusicMute()
    {
        toggleMusicMute = !toggleMusicMute;

        if (!toggleMusicMute)
        {
            PlayerPrefs.SetInt("MusicVolume", 0);
            AmbiantSource.volume = 0.8f;
            MusicUnmuteIcon.SetActive(true);
            MusicMuteIcon.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("MusicVolume", 1);
            AmbiantSource.volume = 0;
            MusicUnmuteIcon.SetActive(false);
            MusicMuteIcon.SetActive(true);
        }
    }
    public void CheckCurrentState()
    {
        if (!toggleMute)
        {
            PlayerPrefs.SetInt("SoundVolume", 0);
            VFXSource.volume = 1f;
            MusicSource.volume = 1f;
            UnmuteIcon.SetActive(true);
            muteIcon.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("SoundVolume", 1);
            VFXSource.volume = 0;
            MusicSource.volume = 0;
            UnmuteIcon.SetActive(false);
            muteIcon.SetActive(true);
        }

        if (!toggleMusicMute)
        {
            PlayerPrefs.SetInt("MusicVolume", 0);
            AmbiantSource.volume = 0.8f;
            MusicUnmuteIcon.SetActive(true);
            MusicMuteIcon.SetActive(false);
        }
        else
        {
            PlayerPrefs.SetInt("MusicVolume", 1);
            AmbiantSource.volume = 0;
            MusicUnmuteIcon.SetActive(false);
            MusicMuteIcon.SetActive(true);
        }
    }
    public void PlayGamePlayMusic()
    {
        _PlayMusic(gamePlay);
    }
    public void PlayMainMenuMusic()
    {
        _PlayMusic(mainMenu);
    }
}
