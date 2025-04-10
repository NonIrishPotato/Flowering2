using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    // Audio players components.
    public AudioSource sfxSource;
    public AudioSource musicSource;
    public AudioClip musicClip;

    // Random pitch adjustment range.
    public float lowPitchRange = .95f;
    public float highPitchRange = 1.05f;

    // Singleton instance.
    public static AudioManager Instance = null;

    // Initialize the singleton instance.
     private void Awake()
    {
        // If there is not already an instance of AudioManager, set it to this.
        if (Instance == null)
        {
            Instance = this;
        }
        // If an instance already exists, destroy this object to enforce the singleton.
        else if (Instance != this)
        {
            Destroy(gameObject);
        }

        // Set AudioManager to DontDestroyOnLoad so that it won't be destroyed when reloading our scene.
        DontDestroyOnLoad(gameObject);

        // Ensure AudioSource components are assigned.
        if (sfxSource == null || musicSource == null)
        {
            Debug.LogError("AudioSource components are not assigned in the AudioManager.");
        }
    }

    private void Start()
    {
            // Play the background music.
        PlayMusic(musicClip);
        musicSource.loop = true;

    }

    // Play a single clip through the sound effects source.
    public void PlaySFK(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null. Cannot play audio.");
            return;
        }

        sfxSource.clip = clip;
        sfxSource.Play();
    }

    // Play a single clip through the music source.
    public void PlayMusic(AudioClip clip)
    {
        if (clip == null)
        {
            Debug.LogError("AudioClip is null. Cannot play music.");
            return;
        }

        musicSource.clip = clip;
        musicSource.Play();
    }

    // Stop the currently playing sound effect.
    public void StopSFX()
    {
        sfxSource.Stop();
    }

    // Stop the currently playing music.
    public void StopMusic()
    {
        musicSource.Stop();
    }

    // Set the volume of the sound effects source.
    public void SetSFXVolume(float volume)
    {
        sfxSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    // Set the volume of the music source.
    public void SetMusicVolume(float volume)
    {
        musicSource.volume = Mathf.Clamp(volume, 0f, 1f);
    }

    // Play a random clip from an array, and randomize the pitch slightly.
    public void RandomSoundEffect(params AudioClip[] clips)
    {
        if (clips.Length == 0)
        {
            Debug.LogError("No AudioClips provided. Cannot play random sound effect.");
            return;
        }

        int randomIndex = Random.Range(0, clips.Length);
        float randomPitch = Random.Range(lowPitchRange, highPitchRange);

        sfxSource.pitch = randomPitch;
        sfxSource.clip = clips[randomIndex];
        sfxSource.Play();
    }
}
