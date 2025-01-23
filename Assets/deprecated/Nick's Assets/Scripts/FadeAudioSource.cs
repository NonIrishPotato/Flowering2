using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public static class FadeAudioSource
{
    public static IEnumerator StartFade(bool fadeIn, AudioSource audioSource, float duration, float targetVolume)
    {
        if(!fadeIn)
        {
            double lengthOfSource = (double)audioSource.clip.samples / audioSource.clip.frequency;
            yield return new WaitForSecondsRealtime((float)(lengthOfSource - duration));
        }


        float currentTime = 0;
        float start = audioSource.volume;
        while (currentTime < duration)
        {
            currentTime += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(start, targetVolume, currentTime / duration);
            yield return null;
        }
        yield break;
    }

    /*public static IEnumerator FadeOutAudio(AudioSource audioSource, float FadeTime)
    {
        float startVolume = audioSource.volume;

        while (audioSource.volume > 0)
        {
            audioSource.volume -= startVolume * Time.deltaTime / FadeTime;

            yield return null;
        }

        audioSource.Stop();
        audioSource.volume = startVolume;
    }*/
}