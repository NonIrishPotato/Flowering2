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
     * 3 = Cure Berries
     * 4 = Necterblooms
     * 5 - Bloated fungus
     * 6 = Molted Cicada Skins
     * 7 = Rock
     * 8 = Smoke Bomb
     * 9 = Healing Honey
     * ...
     */
    private GameManager gameManager;
    public Image[] InventorySlots; //All image place holders for inventory and crafting slots
    public Image[] KeyItemSlots; //All image place holders for Key Items
    public Image[] ToolSlots;//All image place holders for Tools
    public Image CraftedItemPlaceHolder;
    public int[] Items; //This represents all items in the inventory slots, there is a max of 22
    public int[] KeyItems; //This represents all key items
    public int[] ToolItems; //This represents all Tool Items
    public int[] CraftingSlots;
    public int CrafterItemPlaceholderType = 0;

    public Sprite emptySlot;
    public Sprite testBerry;
    public Sprite testBerry2;
    public Sprite CureBerry;
    public Sprite Necterblooms;
    public Sprite BloatedFungus;
    public Sprite MoltedCicadaSkins;
    public Sprite Rock;
    public Sprite SmokeBomb;
    public Sprite HealingHoney;

    private Image draggedItem;
    private int draggedItemIndex;
    private Image targetSlot;
    private int targetSlotIndex;


    private int amountOfTestBerrys = 0;

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

        //Updates What the Player can craft
        UpdateCraftingPlaceholder();

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

        for (int i = 0; i < 22; i++)
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

        //For Crafting
        for (int i = 22; i < InventorySlots.Length; i++)
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

    private void UpdateToolSlots()
    {
        for (int i = 0; i < ToolSlots.Length; i++)
        {
            if (ToolItems[i] == 0)
            {
                ToolSlots[i].sprite = emptySlot;
            }
        }
    }

    private void UpdateCraftingPlaceholder()
    {
        

            //Getting the Crafting items in the boxes

                CraftingSlots[0] = Items[22];

            //Debug.Log("Slot 1 = " + CraftingSlots[0]);

                CraftingSlots[1] = Items[23];
            //Debug.Log("Slot 2 = " + CraftingSlots[1]);

                CraftingSlots[2] = Items[24];

            //Debug.Log("Slot 3 = " + CraftingSlots[2]);
  
                CraftingSlots[3] = Items[25];
       
            // Debug.Log("Slot 4 = " + CraftingSlots[3]);
  
                CraftingSlots[4] = Items[26];
      
            //Debug.Log("Slot 5 = " + CraftingSlots[4]);

                CraftingSlots[5] = Items[27];
            //Debug.Log("Slot 6 = " + CraftingSlots[5]);
       


        if (CraftingSlots[0] == 1)
        {
            amountOfTestBerrys += 1;
        }
        if (CraftingSlots[1] == 1)
        {
            amountOfTestBerrys += 1;
        }
        if (CraftingSlots[2] == 1)
        {
            amountOfTestBerrys += 1;
        }
        if (CraftingSlots[3] == 1)
        {
            amountOfTestBerrys += 1;
        }
        if (CraftingSlots[4] == 1)
        {
            amountOfTestBerrys += 1;
        }
        if (CraftingSlots[5] == 1)
        {
            amountOfTestBerrys += 1;
        }


        //Crafting Recipies (Opt to change)
        if (amountOfTestBerrys == 2)
        {
            CrafterItemPlaceholderType = 2;
        }
        else
        {
            CrafterItemPlaceholderType = 0;
        }




        if (CrafterItemPlaceholderType == 0)
        {
            CraftedItemPlaceHolder.sprite = emptySlot;
        }
        if(CrafterItemPlaceholderType == 2)
        {
            CraftedItemPlaceHolder.sprite = testBerry2;
        }
        if (CrafterItemPlaceholderType == 8)
        {
            CraftedItemPlaceHolder.sprite = SmokeBomb;
        }
        if (CrafterItemPlaceholderType == 9)
        {
            CraftedItemPlaceHolder.sprite = HealingHoney;
        }

        amountOfTestBerrys = 0;
    }

    public void CraftButtonPress()
    {
        if(CrafterItemPlaceholderType != 0)
        {
            CraftingSlots[0] = 0;
            CraftingSlots[1] = 0;
            CraftingSlots[2] = 0;
            CraftingSlots[3] = 0;
            CraftingSlots[4] = 0;
            CraftingSlots[5] = 0;
            Items[22] = 0;
            Items[23] = 0;
            Items[24] = 0;
            Items[25] = 0;
            Items[26] = 0;
            Items[27] = 0;

            if (CrafterItemPlaceholderType == 2)
            {
                CrafterItemPlaceholderType = 0;
                for(int i = 0; i < InventorySlots.Length - 6; i++)
                {
                    if (Items[i] == 0)
                    {
                        Items[i] = 2;
                        break;
                    }
                }
            }
        }


    }
}
