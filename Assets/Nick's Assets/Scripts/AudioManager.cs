using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, sfxSoundsTheSequal;
    public AudioSource musicSource, sfxSource, sfxSourceTheSequal;

    public bool isPlaying;

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

    public void PlayMusic(string name)
    {
        Sound s = Array.Find(musicSounds, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
        }
        else
        {
            musicSource.clip = s.clip;
            musicSource.Play();
        }
    }

    public void PlaySFX(string name)
    {
        Sound s = Array.Find(sfxSounds, x => x.name == name);
        Sound s2 = Array.Find(sfxSoundsTheSequal, x => x.name == name);

        if (s == null)
        {
            Debug.Log("Sound Not Found");
            isPlaying = false;
        }
        else
        {
            sfxSource.PlayOneShot(s.clip);
            isPlaying = true;
        } 
    }

    public void PlaySFXtheSequal(string name)
    {
        Sound s2 = Array.Find(sfxSoundsTheSequal, x => x.name == name);


        if (s2 == null)
        {
            isPlaying = false;
        }
        else
        {
            sfxSourceTheSequal.PlayOneShot(s2.clip);
            isPlaying = true;
        }
    }

    public void ToggleMusic()
    {
        musicSource.mute = !musicSource.mute;
    }

    public void ToggleSFX()
    {
        sfxSource.mute = !sfxSource.mute;
        sfxSourceTheSequal.mute = !sfxSourceTheSequal.mute;
    }

    public void MusicVolume(float volume)
    {
        musicSource.volume = volume;
    }

    public void SFXVolume(float volume)
    {
        sfxSource.volume = volume;
        sfxSourceTheSequal.volume = volume;
    }
}
