using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingPlayerSounds : MonoBehaviour
{
    public bool isWalking = false;

    // Update is called once per frame
    void Update()
    {
        if(!PauseMenuScript.isPaused)
        {
            if ((Input.GetKey(KeyCode.A)) || (Input.GetKey(KeyCode.D)))
            {
                isWalking = true;
            }
            else
            {
                isWalking = false;
            }

            if (isWalking && !AudioManager.Instance.sfxSource.isPlaying)
            {
                AudioManager.Instance.PlaySFX("Walk");
                Debug.Log("Player is walking");
            }

            if (Input.GetMouseButtonDown(0))
            {
                AudioManager.Instance.PlaySFX("Slingshot Fire");
            }
        }
    }
}
