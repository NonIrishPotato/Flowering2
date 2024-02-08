using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CureDispenser : MonoBehaviour
{
    public GameManager gameManager;
    public static GameObject Instance;
    private bool isPlayerInSpot = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Check if the entering collider is the player
        if (other.CompareTag("Player"))
        {
            isPlayerInSpot = true;
            Debug.Log("True");
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

    // Update is called once per frame
    void Update()
    {
        if (gameManager.amountOfBerrys > 0 && isPlayerInSpot && Input.GetKeyDown(KeyCode.LeftShift))
        {
            gameManager.amountOfBerrys--;
            gameManager.amountOfBerrysCollected++;
            Debug.Log(gameManager.amountOfBerrysCollected);
        }
    }
}
