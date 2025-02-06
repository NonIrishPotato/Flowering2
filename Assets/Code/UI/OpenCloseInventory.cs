using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class OpenCloseInventory : MonoBehaviour
{
    public GameObject inventoryImage; // Reference to the GameObject containing the inventory image
    public GameObject healthUI; // Reference to the Health UI element
    public GameObject bagUI; // Reference to the Bag UI element
    public GameObject selectionSlotsUI; // Reference to the Selection Slots UI element
    public GameObject infectionUI; // Reference to the Infection UI element

    private bool inventoryOpen = false; // Flag to keep track of inventory state

    // Start is called before the first frame update
    void Start()
    {

        // Ensure the inventory image is initially closed
       

        // Ensure UI elements are enabled initially
        if (healthUI != null) healthUI.SetActive(true);
        if (bagUI != null) bagUI.SetActive(true);
        if (selectionSlotsUI != null) selectionSlotsUI.SetActive(true);
        if (infectionUI != null) infectionUI.SetActive(true);

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
        ToggleInventory();
    }

    // Update is called once per frame
    void Update()
    {
        // Check for Tab key press
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            ToggleInventory();
        }
    }

    // Toggles the inventory image visibility and disables/enables other UI elements
    private void ToggleInventory()
    {
        if (inventoryImage != null)
        {
            inventoryOpen = !inventoryOpen;
            inventoryImage.SetActive(inventoryOpen);

            // Disable or enable the canvas items (UI elements) based on the inventory state
            if (healthUI != null) healthUI.SetActive(!inventoryOpen);
            if (bagUI != null) bagUI.SetActive(!inventoryOpen);
            if (selectionSlotsUI != null) selectionSlotsUI.SetActive(!inventoryOpen);
            if (infectionUI != null) infectionUI.SetActive(!inventoryOpen);
        }
    }
}
