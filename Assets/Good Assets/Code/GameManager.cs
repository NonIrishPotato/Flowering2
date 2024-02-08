using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public bool IsPlayerHiding = false;
    public bool IsPlayerCrouching = false;
    public bool IsPlayerWalking = true;
    public bool IsPlayerSprinting = false;
    public int currentHealth;
    public int maxHealth;
    public int amountOfBerrys = 0;



    // Start is called before the first frame update
    void Start()
    {
        // If you have any initialization code, you can put it here
    }

    // Update is called once per frame
    void Update()
    {
        Debug.Log(amountOfBerrys);
        if (currentHealth <= 0)
        {
            Debug.Log("Death");
            // Add any additional actions for when the player dies
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
