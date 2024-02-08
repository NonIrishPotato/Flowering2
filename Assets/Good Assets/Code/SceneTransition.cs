using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneTransition : MonoBehaviour
{
    public string sceneToLoad;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            DontDestroyOnLoad(GameManager.Instance.gameObject);
            // Perform scene transition
            SceneManager.LoadScene(sceneToLoad);
        }
    }
}
