using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    public Sound[] musicSounds, sfxSounds, sfxSoundsTheSequal;
    public AudioClip enemyScreamSound;
    public AudioSource musicSource, sfxSource, sfxSourceTheSequal;

    public bool isPlaying;
    public bool iHateMyself;

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

    public static IEnumerator FadeOutAudio(AudioSource audioSource)
    {
        //float startVolume = audioSource.volume;
        float timeToFade = 1f;
        float timeElapsed = 0f;

        while (audioSource.volume < timeToFade)
        {
            //audioSource.volume -= startVolume * Time.deltaTime / timeToFade;

            var newVolume = Mathf.Lerp(1, 0, timeElapsed / timeToFade);

            audioSource.volume = newVolume;

            yield return null;
        }

        audioSource.Stop();
    }

    public IEnumerator FadeInAudio(AudioSource audioSource)
    {
        //float startVolume = audioSource.volume;
        float timeToFade = 1.5f;
        float timeElapsed = 0f;
        while (audioSource.volume < timeToFade)
        {
            //audioSource.volume -= startVolume * Time.deltaTime / timeToFade;

            var newVolume = Mathf.Lerp(0, 1, timeElapsed / timeToFade);

            timeElapsed += Time.deltaTime;

            audioSource.volume = newVolume;

            yield return null;
        }
        //audioSource.volume = startVolume;
    }
}
