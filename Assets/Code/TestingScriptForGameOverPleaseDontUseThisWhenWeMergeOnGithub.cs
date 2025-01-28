using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestingScriptForGameOverPleaseDontUseThisWhenWeMergeOnGithub : MonoBehaviour
{
    public int health;
    public int food;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            GetHurt();
        }

        if (health <= 0)
        {
            FindObjectOfType<PauseMenuScript>().GameOver();
        }

        if (Input.GetKeyDown(KeyCode.E))
        {
            GetFood();
        }

        if (food >= 4)
        {
            FindObjectOfType<PauseMenuScript>().YouWin();
        }
    }

    void GetHurt()
    {
        health--;
        Debug.Log("Player got hit!");
        Debug.Log("Health: " + health);
    }

    void GetFood()
    {
        food++;
        Debug.Log("Player obtained food!");
        Debug.Log("Food: " + food);
    }
}
