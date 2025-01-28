using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureBerryBush : MonoBehaviour
{
    public GameManager gameManager;
    public static GameObject Instance;
    public int whichBerryBush = 0;
    public bool berryTaken = false;

    public bool isPlayerInSpot = false;
    private BerryController berryController;
    public GameObject berry;

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInSpot = true;
        }
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        // Check if the exiting collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInSpot = false;
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        berryController = FindObjectOfType<BerryController>();

        if (berryController == null)
        {
            Debug.LogError("BerryController not found in the scene!");
        }
        gameManager = GameManager.Instance;
    }

    private void Awake()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        BerryState berryState = berryController.berryStates[whichBerryBush];
        if(berryState.isPicked)
        {
            berry.SetActive(false);
            berryTaken = true;
        }

        if (isPlayerInSpot && Input.GetKeyDown(KeyCode.E))
        {
            if (!berryTaken)
            {
                // Perform actions for picking the berry
                berryController.PickBerry(whichBerryBush);
                gameManager.amountOfBerrys += 1;
                gameManager.cureBerrys += 1;
                berry.SetActive(false);
                Debug.Log("Berry picked! Total Berries: " + gameManager.amountOfBerrys);
                // Add any additional actions for when the player picks the berry
            }
            else
            {
                
                Debug.Log("No berry to pick!");
                
            }
        }
    }
}
