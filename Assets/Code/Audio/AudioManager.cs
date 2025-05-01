using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    public static AudioManager instance;
    public AudioSource audioSource;

    public AudioSource soundFXObject;

    private void Awake()
    {
        if (instance == null)
            instance = this;
    }

    public void PlaySoundFXClip(AudioClip audioClip)
    {
        // Spawn in gameObject
        audioSource = Instantiate(soundFXObject);

        //assign the audioClip
        audioSource.clip = audioClip;

        //play sound 
        audioSource.Play();

        // get length of sound FX clip
        float clipLength = audioSource.clip.length;

        // destroy the clip after it is done playing
        Destroy(audioSource.gameObject, clipLength);

    }

}


