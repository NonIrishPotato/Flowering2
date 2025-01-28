using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuManager : MonoBehaviour
{
    public int chooseSceneNumber;

    public void PlayGame()
    {
        SceneManager.LoadScene(chooseSceneNumber);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
