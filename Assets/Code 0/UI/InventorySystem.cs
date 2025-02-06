using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class InventorySystem : MonoBehaviour
{
    /*
     * 0 = Empty Slot
     * 1 = Test Berry 1
     * 2 = Test Berry 2
     * 3 = 
     * ...
     */
    private GameManager gameManager;
    public Image[] InventorySlots; //All image place holders for inventory
    public Image[] KeyItemSlots; //All image place holders for Key Items
    public Image[] CraftingSlots;//All image place holders for Crafting Slots
    public Image[] ToolSlots;//All image place holders for Tools
    public int[] Items; //This represents all items in the inventory slots, there is a max of 22
    public int[] KeyItems; //This represents all key items
    public int[] ToolItems; //This represents all Tool Items
    public int[] CraftingItems; //This represents all Crafted Items

    public Sprite emptySlot;
    public Sprite testBerry;
    public Sprite testBerry2;

    private Image draggedItem;
    private int draggedItemIndex;
    private Image targetSlot;
    private int targetSlotIndex;

    private RectTransform draggedItemTransform;


    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;

        //Checks Inventory Slots and fill items appropriately
        UpdateInventorySlots();

        //Checks KeyItemSlots and fills them appropriately
        UpdateKeyItemSlots();

        //Checks Tool slots and fills them appropriately
        UpdateToolSlots();

        //Checks Crafting Slots and fills them appropriately
        UpdateCraftingSlots();

        //Count Berrys
        countBerrys();      
    }

    // Update is called once per frame
    void Update()
    {
        
        //Checks Inventory Slots and fill items appropriately
        UpdateInventorySlots();

        //Checks KeyItemSlots and fills them appropriately
        UpdateKeyItemSlots();

        //Checks Tool slots and fills them appropriately
        UpdateToolSlots();

        //Checks Crafting Slots and fills them appropriately
        UpdateCraftingSlots();


        countBerrys();
    }

    public void countBerrys()
    {
        for (int i = 0; i < gameManager.tempBerrysForMySanity; i++)
        {
            bool placed = false;
            int amountOfBerrysFound = 0;
            for (int z = 0; z < Items.Length; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 1;
                    InventorySlots[z].sprite = testBerry;
                    gameManager.tempBerrysForMySanity -= 1;
                    Debug.Log(gameManager.tempBerrysForMySanity);
                    break;
                }
                if (Items[z] == 1)
                {
                    amountOfBerrysFound += 1;
                    if (amountOfBerrysFound >= gameManager.amountOfBerrys)
                    {
                        break;
                    }
                }

            }
        }
    }

    private void UpdateInventorySlots()
    {
        for (int i = 0; i < InventorySlots.Length; i++)
        {
            if (Items[i] == 0)
            {
                InventorySlots[i].sprite = emptySlot;
            }
            else if (Items[i] == 1)
            {
                InventorySlots[i].sprite = testBerry;
            }
            else if (Items[i] == 2)
            {
                InventorySlots[i].sprite = testBerry2;
            }
        }
    }

    private void UpdateKeyItemSlots()
    {
        for (int i = 0; i < KeyItemSlots.Length; i++)
        {
            if (KeyItems[i] == 0)
            {
                KeyItemSlots[i].sprite = emptySlot;
            }
        }
    }

    public void UpdateToolSlots()
    {
        for (int i = 0; i < ToolSlots.Length; i++)
        {
            if (ToolItems[i] == 0)
            {
                ToolSlots[i].sprite = emptySlot;
            }
        }
    }
    private void UpdateCraftingSlots()
    {
        for (int i = 0; i < CraftingSlots.Length; i++)
        {
            if (CraftingItems[i] == 0)
            {
                CraftingSlots[i].sprite = emptySlot;
            }
        }
    }

}
