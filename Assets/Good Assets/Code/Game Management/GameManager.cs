using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    private BerryController berryController;

    public static GameManager Instance;
    public bool IsPlayerHiding = false;
    public bool IsPlayerCrouching = false;
    public bool IsPlayerWalking = true;
    public bool IsPlayerSprinting = false;

    public bool smallHazardHit = false;
    public bool mediumHazardHit = false;
    public bool largeHazardHit = false;

    public int currentHealth;
    public int maxHealth;
    public int amountOfBerrys = 0;

    public int tempBerrysForMySanity = 0;
    public int cureBerrys = 0;
    public int necterblooms = 0;
    public int bloatedFungus = 0;
    public int rootClippings = 0;
    public int rocks = 0;
    public int smokeBombs = 0;
    public int healingHoney = 0;
    public int preventative = 0;

    public int deathTic = 0;

    public int[] intArray; // Array of integers to be saved
    public int[] keyArray;



    public int amountOfBerrysCollected;



    public bool canHide = true;
    public float InfectionBar = 0;

    public bool smokeBombActive = false;

    // Start is called before the first frame update
    void Start()
    {
        LoadIntArray();
        LoadKeyArray();
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentHealth <= 0 && deathTic != 1)
        {
            SceneManager.LoadScene("Game_Over2");
            deathTic = 1;
            currentHealth = maxHealth;
            // Add any additional actions for when the player dies
        }
        if (amountOfBerrysCollected == 8)
        {
            SceneManager.LoadScene("Win_Screen");
        }

    }
    void OnDestroy()
    {
        // Save the array to PlayerPrefs when the game object is destroyed
        SaveIntArray();
        SaveKeyArray();
    }

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

    void SaveIntArray()
    {
        // Convert the array to a string using JSON serialization
        string jsonArray = JsonUtility.ToJson(intArray);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString("IntArray", jsonArray);

        // Save PlayerPrefs immediately to ensure the data is saved
        PlayerPrefs.Save();
    }

    void LoadIntArray()
    {
        // Check if PlayerPrefs has the key "IntArray"
        if (PlayerPrefs.HasKey("IntArray"))
        {
            // Retrieve the JSON string from PlayerPrefs
            string jsonArray = PlayerPrefs.GetString("IntArray");

            // Convert the JSON string back to an array of integers using JSON deserialization
            intArray = JsonUtility.FromJson<int[]>(jsonArray);
        }
        else
        {
            // If PlayerPrefs does not have the key, initialize the array with default values
            intArray = new int[32]; // Example: initialize with 10 elements
        }
    }

    void SaveKeyArray()
    {
        // Convert the array to a string using JSON serialization
        string jsonArray = JsonUtility.ToJson(keyArray);

        // Save the JSON string to PlayerPrefs
        PlayerPrefs.SetString("KeyArray", jsonArray);

        // Save PlayerPrefs immediately to ensure the data is saved
        PlayerPrefs.Save();
    }

    void LoadKeyArray()
    {
        // Check if PlayerPrefs has the key "IntArray"
        if (PlayerPrefs.HasKey("KeyArray"))
        {
            // Retrieve the JSON string from PlayerPrefs
            string jsonArray = PlayerPrefs.GetString("KeyArray");

            // Convert the JSON string back to an array of integers using JSON deserialization
            keyArray = JsonUtility.FromJson<int[]>(jsonArray);
        }
        else
        {
            // If PlayerPrefs does not have the key, initialize the array with default values
            keyArray = new int[8]; // Example: initialize with 10 elements
        }
    }
}
