using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBushEnemys : MonoBehaviour
{
    public GameManager gameManager;
    public static GameObject Instance;
    public float infectionSpeed = 1f;

    void Start()
    {
        gameManager = GameManager.Instance;
    }
    void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            if(gameManager.InfectionBar <= 1)
            {
                gameManager.InfectionBar += Time.deltaTime * infectionSpeed;
            }
        }
    }
}
