using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hide : MonoBehaviour
{
    public Rigidbody2D rb;
    public GameManager manager;
    public bool isCrate = false; // True if this is a crate, false if this is a bush

    private static bool isPlayerinSpot = false; // Static variable to track hiding state globally
    public static Hide currentHidingSpot; // Static reference to the current hiding spot

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
            currentHidingSpot = this; // Set the current hiding spot
        }
    }

    // Called when another collider exits the trigger zone
    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerinSpot = false; // Update hiding state globally
            currentHidingSpot = null; // Clear the current hiding spot
        }
    }

    // Check if the player is hiding
    public static bool IsPlayerinSpot()
    {
        return isPlayerinSpot;
    }

    void Update()
    {
        if (isCrate)
        {
            // Crate functionality
            if (IsPlayerinSpot() && Input.GetKeyDown(KeyCode.E) && manager.canHide == true && manager.IsPlayerHiding == false)
            {
                manager.IsPlayerHiding = true;
                manager.PlayerFrozen = true;
                Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
            }
            else if (IsPlayerinSpot() && Input.GetKeyDown(KeyCode.E) && manager.IsPlayerHiding == true)
            {
                manager.IsPlayerHiding = false;
                manager.PlayerFrozen = false;
                Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
            }
        }
        else
        {
            // Bush functionality
            if (IsPlayerinSpot() && Input.GetKey(KeyCode.LeftControl) && manager.canHide == true)
            {
                manager.IsPlayerHiding = true;
                Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
            }
            else if (isPlayerinSpot == false)
            {
                manager.IsPlayerHiding = false;
                Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
            }
        }
    }
}
