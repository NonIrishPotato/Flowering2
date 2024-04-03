using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectedItemInventory : MonoBehaviour
{

    public GameObject[] selection;
    private int currentIndex = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            // Calculate the new index based on the scroll direction
            int newIndex = currentIndex + (scroll > 0 ? 1 : -1);

            // Ensure the index stays within bounds
            if (newIndex < 0)
            {
                newIndex = selection.Length - 1;
            }
            else if (newIndex >= selection.Length)
            {
                newIndex = 0;
            }

            // Activate the new item slot and deactivate the current one
            selection[currentIndex].SetActive(false);
            selection[newIndex].SetActive(true);

            // Update the current index
            currentIndex = newIndex;
        }
        
    }
}
