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
     * 6 = Root Clippings
     * 7 = Rock
     * 8 = Smoke Bomb
     * 9 = Healing Honey
     * 10 = Preventative
     * ...
     */

    public static InventorySystem Instance;
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
    public Sprite RootClipings;
    public Sprite Rock;
    public Sprite SmokeBomb;
    public Sprite HealingHoney;
    public Sprite Preventative;

    public Image[] SelectedItemSlots;


    private int amountOfBloatedFungus = 0;
    private int amountOfNectarBlooms = 0;
    private int amountOfRootClippings = 0;

    private RectTransform draggedItemTransform;

    private void Awake()
    {
        // Singleton pattern to ensure only one instance of GameManager exists
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject); // Don't destroy GameManager when loading new scenes
        }
        else
        {
            Destroy(gameObject); // Destroy duplicate GameManager instances
        }
    }

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

        KeyItems = gameManager.keyArray;
        gameManager.keyArray = KeyItems;

        Items = gameManager.intArray;
        gameManager.intArray = Items;
    }

    public void countBerrys()
    {
        
        for (int i = 0; i < gameManager.cureBerrys; i++)
        {
            bool placed = false;
            int amountOfCureBerrysFound = 0;

            for (int z = 0; z < KeyItems.Length; z++)
            {
                if (KeyItems[z] == 0 && placed == false && gameManager.amountOfBerrys <= 7)
                {
                    placed = true;
                    KeyItems[z] = 3;
                    KeyItemSlots[z].sprite = CureBerry;
                    gameManager.cureBerrys -= 1;
                }
                if (KeyItems[z] == 3)
                {
                    amountOfCureBerrysFound += 1;
                    gameManager.amountOfBerrys = amountOfCureBerrysFound;
                }

            }
        }
        for (int i = 0; i < gameManager.necterblooms; i++)
        {
            bool placed = false;
            int amountOfNecterbloomsFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 4;
                    InventorySlots[z].sprite = Necterblooms;
                    gameManager.necterblooms -= 1;
                }
                if (Items[z] == 4)
                {
                    amountOfNecterbloomsFound += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.bloatedFungus; i++)
        {
            bool placed = false;
            int amountOfBloatedFungusFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 5;
                    InventorySlots[z].sprite = BloatedFungus;
                    gameManager.bloatedFungus -= 1;
                }
                if (Items[z] == 5)
                {
                    amountOfBloatedFungusFound += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.rootClippings; i++)
        {
            bool placed = false;
            int amountOfRootClippings = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 6;
                    InventorySlots[z].sprite = RootClipings;
                    gameManager.rootClippings -= 1;
                }
                if (Items[z] == 6)
                {
                    amountOfRootClippings += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.rocks; i++)
        {
            bool placed = false;
            int amountOfRocksFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 7;
                    InventorySlots[z].sprite = Rock;
                    gameManager.rocks -= 1;
                }
                if (Items[z] == 7)
                {
                    amountOfRocksFound += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.smokeBombs; i++)
        {
            bool placed = false;
            int amountOfSmokeBombsFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 8;
                    InventorySlots[z].sprite = SmokeBomb;
                    gameManager.smokeBombs -= 1;
                }
                if (Items[z] == 8)
                {
                    amountOfSmokeBombsFound += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.healingHoney; i++)
        {
            bool placed = false;
            int amountOfHealingHoneyFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 9;
                    InventorySlots[z].sprite = HealingHoney;
                    gameManager.healingHoney -= 1;
                }
                if (Items[z] == 9)
                {
                    amountOfHealingHoneyFound += 1;
                }

            }
        }
        for (int i = 0; i < gameManager.preventative; i++)
        {
            bool placed = false;
            int amountOfPreventativeFound = 0;

            for (int z = 0; z < Items.Length - 10; z++)
            {
                if (Items[z] == 0 && placed == false)
                {
                    placed = true;
                    Items[z] = 10;
                    InventorySlots[z].sprite = Preventative;
                    gameManager.preventative -= 1;
                }
                if (Items[z] == 10)
                {
                    amountOfPreventativeFound += 1;
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
            else if (Items[i] == 3)
            {
                InventorySlots[i].sprite = CureBerry;
            }
            else if (Items[i] == 4)
            {
                InventorySlots[i].sprite = Necterblooms;
            }
            else if (Items[i] == 5)
            {
                InventorySlots[i].sprite = BloatedFungus;
            }
            else if (Items[i] == 6)
            {
                InventorySlots[i].sprite = RootClipings;
            }
            else if (Items[i] == 7)
            {
                InventorySlots[i].sprite = Rock;
            }
            else if (Items[i] == 8)
            {
                InventorySlots[i].sprite = SmokeBomb;
            }
            else if (Items[i] == 9)
            {
                InventorySlots[i].sprite = HealingHoney;
            }
            else if (Items[i] == 10)
            {
                InventorySlots[i].sprite = Preventative;
            }
        }

        //For Crafting
        for (int i = 22; i < InventorySlots.Length -4; i++)
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
            else if (Items[i] == 3)
            {
                InventorySlots[i].sprite = CureBerry;
            }
            else if (Items[i] == 4)
            {
                InventorySlots[i].sprite = Necterblooms;
            }
            else if (Items[i] == 5)
            {
                InventorySlots[i].sprite = BloatedFungus;
            }
            else if (Items[i] == 6)
            {
                InventorySlots[i].sprite = RootClipings;
            }
            else if (Items[i] == 7)
            {
                InventorySlots[i].sprite = Rock;
            }
            else if (Items[i] == 8)
            {
                InventorySlots[i].sprite = SmokeBomb;
            }
            else if (Items[i] == 9)
            {
                InventorySlots[i].sprite = HealingHoney;
            }
            else if (Items[i] == 10)
            {
                InventorySlots[i].sprite = Preventative;
            }
        }

        //For Tool Slots
        for (int i = 28; i < InventorySlots.Length; i++)
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
            else if (Items[i] == 3)
            {
                InventorySlots[i].sprite = CureBerry;
            }
            else if (Items[i] == 4)
            {
                InventorySlots[i].sprite = Necterblooms;
            }
            else if (Items[i] == 5)
            {
                InventorySlots[i].sprite = BloatedFungus;
            }
            else if (Items[i] == 6)
            {
                InventorySlots[i].sprite = RootClipings;
            }
            else if (Items[i] == 7)
            {
                InventorySlots[i].sprite = Rock;
            }
            else if (Items[i] == 8)
            {
                InventorySlots[i].sprite = SmokeBomb;
            }
            else if (Items[i] == 9)
            {
                InventorySlots[i].sprite = HealingHoney;
            }
            else if (Items[i] == 10)
            {
                InventorySlots[i].sprite = Preventative;
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
            SelectedItemSlots[i].sprite = ToolSlots[i].sprite;
        }

        ToolItems[0] = Items[28];

        ToolItems[1] = Items[29];

        ToolItems[2] = Items[30];

        ToolItems[3] = Items[31];

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

     
      

        //Counts NectarBlooms
        if (CraftingSlots[0] == 4)
        {
            amountOfNectarBlooms += 1;
        }
        if (CraftingSlots[1] == 4)
        {
            amountOfNectarBlooms += 1;
        }
        if (CraftingSlots[2] == 4)
        {
            amountOfNectarBlooms += 1;
        }
        if (CraftingSlots[3] == 4)
        {
            amountOfNectarBlooms += 1;
        }
        if (CraftingSlots[4] == 4)
        {
            amountOfNectarBlooms += 1;
        }
        if (CraftingSlots[5] == 4)
        {
            amountOfNectarBlooms += 1;
        }

        //Counts Bloated Fungus
        if (CraftingSlots[0] == 5)
        {
            amountOfBloatedFungus += 1;
        }
        if (CraftingSlots[1] == 5)
        {
            amountOfBloatedFungus += 1;
        }
        if (CraftingSlots[2] == 5)
        {
            amountOfBloatedFungus += 1;
        }
        if (CraftingSlots[3] == 5)
        {
            amountOfBloatedFungus += 1;
        }
        if (CraftingSlots[4] == 5)
        {
            amountOfBloatedFungus += 1;
        }
        if (CraftingSlots[5] == 5)
        {
            amountOfBloatedFungus += 1;
        }

        //Counts Root Clippings
        if (CraftingSlots[0] == 6)
        {
            amountOfRootClippings += 1;
        }
        if (CraftingSlots[1] == 6)
        {
            amountOfRootClippings += 1;
        }
        if (CraftingSlots[2] == 6)
        {
            amountOfRootClippings += 1;
        }
        if (CraftingSlots[3] == 6)
        {
            amountOfRootClippings += 1;
        }
        if (CraftingSlots[4] == 6)
        {
            amountOfRootClippings += 1;
        }
        if (CraftingSlots[5] == 6)
        {
            amountOfRootClippings += 1;
        }


        //Crafting Recipies (Opt to change)

        if(amountOfBloatedFungus == 2)
        {
            CrafterItemPlaceholderType = 8;
        }
        else if (amountOfNectarBlooms == 2)
        {
            CrafterItemPlaceholderType = 9;
        }
        else if (amountOfNectarBlooms == 1 && amountOfRootClippings == 1)
        {
            CrafterItemPlaceholderType = 10;
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
        if (CrafterItemPlaceholderType == 10)
        {
            CraftedItemPlaceHolder.sprite = Preventative;
        }

        amountOfNectarBlooms = 0;
        amountOfBloatedFungus = 0;
        amountOfRootClippings = 0;
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

            //Different crafting recipy results

            //Smoke Bomb made from 2 Bloated fungus
            // Preventative (Made from 1 nectarbloom and a root clipping
            //Healing Honey (Made with 2 nectarblooms)
            if (CrafterItemPlaceholderType == 2)
            {
                CrafterItemPlaceholderType = 0;
                for(int i = 0; i < InventorySlots.Length - 10; i++) //-10 to accounnt for tool and crafting slots. Don't want to check those.
                {
                    if (Items[i] == 0)
                    {
                        Items[i] = 2;
                        break;
                    }
                }
            }

            if (CrafterItemPlaceholderType == 8)
            {
                CrafterItemPlaceholderType = 0;
                for (int i = 0; i < InventorySlots.Length - 10; i++) //-10 to accounnt for tool and crafting slots. Don't want to check those.
                {
                    if (Items[i] == 0)
                    {
                        Items[i] = 8;
                        break;
                    }
                }
            }

            if (CrafterItemPlaceholderType == 9)
            {
                CrafterItemPlaceholderType = 0;
                for (int i = 0; i < InventorySlots.Length - 10; i++) //-10 to accounnt for tool and crafting slots. Don't want to check those.
                {
                    if (Items[i] == 0)
                    {
                        Items[i] = 9;
                        break;
                    }
                }
            }

            if (CrafterItemPlaceholderType == 10)
            {
                CrafterItemPlaceholderType = 0;
                for (int i = 0; i < InventorySlots.Length - 10; i++) //-10 to accounnt for tool and crafting slots. Don't want to check those.
                {
                    if (Items[i] == 0)
                    {
                        Items[i] = 10;
                        break;
                    }
                }
            }

        }


    }
}
