using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameOverMenu : MonoBehaviour
{
    private void Update()
    {
        Time.timeScale = 1f;
    }
    public void RestartGame(int i)
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(i);
    }   
    public void GoToMainMenu()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(0);
    }

    public void QuitGame()
    {
        Application.Quit();
    }
}
