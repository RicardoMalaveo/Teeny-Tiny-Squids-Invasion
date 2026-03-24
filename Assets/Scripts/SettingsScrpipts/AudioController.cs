using System;
using UnityEngine;

public class AudioController : MonoBehaviour
{
    public static AudioController Instance;

    public Sound[] musicSounds, SFXSounds, UISounds;
    public AudioSource musicSource, SFXSource, UISource;
    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Start()
    {
        PlayMusic("MainMenu");
    }

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }

        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }
    public void PlaySFX(string name)
    {
        Sound s = Array.Find(SFXSounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("Sound: " + name + " not found!");
        }
        else
        {
            SFXSource.PlayOneShot(s.clip);
        }

    }
    public void PlayUI(string name)
    {
        Sound s = Array.Find(UISounds, x => x.name == name);
        if (s == null)
        {
            Debug.LogWarning("UI Sound: " + name + " not found!");
        }
        else
        {
            UISource.PlayOneShot(s.clip);
        }
    }
    public void ToggleMusic(bool muteStatus)
    {
        musicSource.mute = !muteStatus;
    }

    public void ToggleSFX(bool muteStatus)
    {
        SFXSource.mute = !muteStatus;
    }
    public void ToggleUI(bool muteStatus)
    {
        UISource.mute = !muteStatus;
    }
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SetSFXVolume(float volume)
    {
        SFXSource.volume = volume;
    }

    public void SetUIVolume(float volume)
    {
        UISource.volume = volume;
    }
}
