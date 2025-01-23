using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FlowerBushEnemys : MonoBehaviour
{
    public GameManager gameManager;
    public static GameObject Instance;
    public float damageInterval = 2f; // Time interval between each damage tick
    private float lastDamageTime; // Time when the last damage was dealt

    void Start()
    {
        gameManager = GameManager.Instance;
    }
        void OnTriggerStay2D(Collider2D other)
    {
        // Check if the colliding object is the player
        if (other.CompareTag("Player"))
        {
            // Check if enough time has passed since the last damage
            if (Time.time - lastDamageTime > damageInterval)
            {
                // Deal damage to the player
                gameManager.currentHealth--;
                // Update the last damage time
                lastDamageTime = Time.time;
                Debug.Log("Player Health is: " + gameManager.currentHealth);
            }
        }
    }
}
