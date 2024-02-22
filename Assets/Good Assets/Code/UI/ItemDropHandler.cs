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


        currentSlotIndex = 0;
        targetSlotIndex = 0;
    }
}
