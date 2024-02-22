using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDropHandler : MonoBehaviour, IDropHandler
{
    public InventorySystem inventorySystem; // Reference to the InventorySystem

    private Image targetSlot;
    private Image currentSlot;
    private int currentSlotIndex;
    private int targetSlotIndex;

    public void OnDrop(PointerEventData eventData)
    {
        // Get the target slot and its index
        GameObject targetObject = eventData.pointerCurrentRaycast.gameObject;

        // Ensure the target object has an Image component
        targetSlot = targetObject.GetComponent<Image>();
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

        currentSlot = eventData.pointerDrag.GetComponent<Image>();
        


        for (int i = 0; i < inventorySystem.InventorySlots.Length; i++)
        {
            if (inventorySystem.InventorySlots[i] == currentSlot)
            {
                currentSlotIndex = i;
                i = 0;
                break;
            }
        }

        Debug.Log("Dropped " + currentSlot + " " + targetSlotIndex + " " + currentSlotIndex);



        if (currentSlotIndex < targetSlotIndex)
        {
            // Swap items between slots
            int tempItem = inventorySystem.Items[currentSlotIndex];
            inventorySystem.Items[currentSlotIndex] = inventorySystem.Items[targetSlotIndex];
            inventorySystem.Items[targetSlotIndex] = tempItem;

            // Swap sprites between slots
            Sprite tempSprite = inventorySystem.InventorySlots[currentSlotIndex].sprite;
            inventorySystem.InventorySlots[currentSlotIndex].sprite = inventorySystem.InventorySlots[targetSlotIndex].sprite;
            inventorySystem.InventorySlots[targetSlotIndex].sprite = tempSprite;
        }
        if (currentSlotIndex >= targetSlotIndex)
        {
            // Swap items between slots
            int tempItem = inventorySystem.Items[targetSlotIndex];
            inventorySystem.Items[targetSlotIndex] = inventorySystem.Items[currentSlotIndex];
            inventorySystem.Items[currentSlotIndex] = tempItem;

            // Swap sprites between slots
            Sprite tempSprite = inventorySystem.InventorySlots[targetSlotIndex].sprite;
            inventorySystem.InventorySlots[targetSlotIndex].sprite = inventorySystem.InventorySlots[currentSlotIndex].sprite;
            inventorySystem.InventorySlots[currentSlotIndex].sprite = tempSprite;
        }

        currentSlotIndex = 0;
        targetSlotIndex = 0;
    }
}
