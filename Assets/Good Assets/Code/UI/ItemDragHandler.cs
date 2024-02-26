using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ItemDragHandler : MonoBehaviour, IDragHandler, IEndDragHandler
{
    private Image currentSlot;
    public Transform newParent;
    private Vector3 vec;
    void Start()
    {
        vec = transform.localPosition;
    }

    public void OnDrag(PointerEventData eventData)
    {
        currentSlot = eventData.pointerDrag.GetComponent<Image>();
        if (newParent != null)
        {
            // Change the parent of this GameObject to the new parent
            currentSlot.transform.SetParent(newParent, true); // Set the second parameter to true if you want to maintain the local position, rotation, and scale
        }
        else
        {
            Debug.LogWarning("No new parent assigned!");
        }
        transform.position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        transform.localPosition = vec;
    }
}
