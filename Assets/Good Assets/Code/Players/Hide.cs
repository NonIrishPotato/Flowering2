using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public GameManager gameManager;
    private bool isPlayerInSpot = false;


    private void Start()
    {
        gameManager = GameManager.Instance;
    }
    // Called when another collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInSpot = true;
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInSpot = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Check if the player is holding down the Left Shift key while in the hiding spot
        if (isPlayerInSpot && Input.GetKey(KeyCode.LeftShift))
        {
            // Perform actions for hiding
            gameManager.IsPlayerHiding = true;
            // Add any additional actions for when the player is hiding
        }
        else
        {
            // Perform actions for not hiding
            gameManager.IsPlayerHiding = false;
            // Add any additional actions for when the player is not hiding
        }
    }
}
