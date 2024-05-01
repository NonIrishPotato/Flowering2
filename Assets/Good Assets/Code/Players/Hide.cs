using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameManager manager;

    private static bool isPlayerinSpot = false; // Static variable to track hiding state globally


    private void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        manager = GameManager.Instance;
    }
    // Called when another collider enters the trigger zone
    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerinSpot = true; // Update hiding state globally
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerinSpot = false; // Update hiding state globally
        }
    }

    // Check if the player is hiding
    public static bool IsPlayerinSpot()
    {
        return isPlayerinSpot;
    }

    void Update()
    {
        // Check if the player is holding down the Left Shift key while in a hiding spot
        if (IsPlayerinSpot() && Input.GetKey(KeyCode.LeftShift) && manager.canHide == true)
        {
            // Perform actions for hiding
            manager.IsPlayerHiding = true;
            Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
            // Add any additional actions for when the player is hiding
        }
        else
        {
            // Perform actions for not hiding
            manager.IsPlayerHiding = false;
            Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);

            // Add any additional actions for when the player is not hiding
        }
    }
    //Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), true)
}
