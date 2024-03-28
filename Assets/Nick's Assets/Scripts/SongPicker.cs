using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SongPicker : MonoBehaviour
{
    public string songName;

    //Place this onto the GameManager as a component
    //In the component, type in the Song Name you wish to play for each new scene
    //Look at the Audio Manager to make sure you typed the correct song name
    void Start()
    {
        AudioManager.Instance.musicSource.Stop(); //This will stop the song from the last level
        AudioManager.Instance.PlayMusic(songName);
    }
}
