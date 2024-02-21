using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystemEventTrigger : MonoBehaviour, IBeginDragHandler, IDropHandler
{
    public InventorySystem inventorySystem; // Reference to the InventorySystem

    void Start()
    {
        // Find the InventorySystem instance in the scene
        // Adjust this line if your InventorySystem is structured differently
        inventorySystem = FindObjectOfType<InventorySystem>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnBeginDrag(eventData, gameObject);
        }
    }

    public void OnDrop(PointerEventData eventData)
    {
        if (inventorySystem != null)
        {
            inventorySystem.OnDrop(eventData, gameObject);
        }
    }

}
