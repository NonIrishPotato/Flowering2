using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class CureDispenser : MonoBehaviour
{
    public GameManager gameManager;
    public static GameObject Instance;
    public int amount = 0;
    public TextMeshProUGUI Text;
    private bool isPlayerInSpot = false;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
       Text =  GetComponentInChildren<TextMeshProUGUI>();

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
            gameManager.amountOfBerrysCollected = gameManager.amountOfBerrys;
            gameManager.amountOfBerrys = gameManager.amountOfBerrys - gameManager.amountOfBerrysCollected;
            amount++;
            Text.text = "" + amount;
            Debug.Log(gameManager.amountOfBerrysCollected);
        }
    }
}
