using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private BerryController berryController;

    public static GameManager Instance;
    public bool IsPlayerHiding = false;
    public bool IsPlayerCrouching = false;
    public bool IsPlayerWalking = true;
    public bool IsPlayerSprinting = false;
    public int currentHealth;
    public int maxHealth;
    public int amountOfBerrys = 0;
    public int tempBerrysForMySanity = 0;
    public int amountOfBerrysCollected;


    public bool canHide = true;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
        if (currentHealth <= 0)
        {
            Debug.Log("Death");
            // Add any additional actions for when the player dies
        }
        if (amountOfBerrysCollected == 4)
        {
            Debug.Log("Win");
        }

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
}
