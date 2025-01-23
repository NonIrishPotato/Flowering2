using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenCloseInventory : MonoBehaviour
{
    public GameObject inventoryImage; // Reference to the GameObject containing the inventory image

    private bool inventoryOpen = false; // Flag to keep track of inventory state

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the inventory image is initially closed
        if (inventoryImage != null)
        {
            inventoryImage.SetActive(false);
        }

        // Add Event Trigger component if not already attached
        if (GetComponent<EventTrigger>() == null)
        {
            gameObject.AddComponent<EventTrigger>();
        }

        // Add pointer click listener
        EventTrigger trigger = GetComponent<EventTrigger>();
        EventTrigger.Entry entry = new EventTrigger.Entry();
        entry.eventID = EventTriggerType.PointerClick;
        entry.callback.AddListener((data) => { OnPointerClick(); });
        trigger.triggers.Add(entry);
    }

    // Called when the GameObject is clicked
    public void OnPointerClick()
    {
        // Toggle the inventory image visibility when clicked
        if (inventoryImage != null)
        {
            inventoryOpen = !inventoryOpen;
            inventoryImage.SetActive(inventoryOpen);
        }
    }
}
