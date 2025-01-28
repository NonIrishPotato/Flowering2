using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventorySystem inventorySystem; // Reference to the InventorySystem
    public Transform newParent;
    public Transform oldParent;

    private Image targetSlot;
    private Image currentSlot;
    private int currentSlotIndex;
    private int targetSlotIndex;


    public void OnDrop(PointerEventData eventData)
    {
        // Get the target slot and its index

        currentSlot = eventData.pointerDrag.GetComponent<Image>();


        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;


        // Ensure the target object has an Image component
        targetSlot = targetObject.GetComponent<Image>();


        Debug.Log(targetSlot);
        if (targetSlot != null)
        {
            Debug.Log("Dropped on image: " + targetSlot.name);
        }
        else
        {
            Debug.Log("Dropped on non-image object: " + targetObject.name);
        }

        for (int i = 0; i < inventorySystem.InventorySlots.Length; i++)
        {
            if (inventorySystem.InventorySlots[i] == targetSlot)
            {
                targetSlotIndex = i;
                i = 0;
                break;
            }
        }
        


        for (int i = 0; i < inventorySystem.InventorySlots.Length; i++)
        {
            if (inventorySystem.InventorySlots[i] == currentSlot)
            {
                currentSlotIndex = i;
                i = 0;
                break;
            }
        }



        Debug.Log("Picked up at " + currentSlot + " " + targetSlotIndex + " " + currentSlotIndex + " placed at " + targetSlot);




        if (inventorySystem.Items[currentSlotIndex] != inventorySystem.Items[targetSlotIndex])
        {
            int tempCurrent = inventorySystem.Items[currentSlotIndex];
            int tempTarget = inventorySystem.Items[targetSlotIndex];
            inventorySystem.Items[currentSlotIndex] = tempTarget;
            inventorySystem.Items[targetSlotIndex] = tempCurrent;

            Sprite tempCurrentSprite = inventorySystem.InventorySlots[currentSlotIndex].sprite;
            Sprite tempTargetSprite = inventorySystem.InventorySlots[targetSlotIndex].sprite;
            inventorySystem.InventorySlots[currentSlotIndex].sprite = tempTargetSprite;
            inventorySystem.InventorySlots[targetSlotIndex].sprite = tempCurrentSprite;
        }

        if (oldParent != null)
        {
            // Change the parent of this GameObject to the new parent
            currentSlot.transform.SetParent(oldParent, true); // Set the second parameter to true if you want to maintain the local position, rotation, and scale
        }
        else
        {
            Debug.LogWarning("No new parent assigned!");
        }


        currentSlotIndex = 0;
        targetSlotIndex = 0;
    }
}
