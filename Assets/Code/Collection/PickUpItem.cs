using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    // Start is called before the first frame update

    public GameManager Manager;

    public bool isNecterbloom = false;
    public bool isBloatedFungus = false;
    public bool isRootClippings = false;

    private bool isPlayerInSpot = false;
    public GameObject berry;

    private bool isPicked = false;
    void Start()
    {
        Manager = GameManager.Instance;
    }

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
    // Update is called once per frame
    void Update()
    {
        if (isPlayerInSpot && Input.GetKeyDown(KeyCode.E) && !isPicked)
        {
            berry.SetActive(false);
            if (isNecterbloom)
            {
                Manager.necterblooms += 1;
                isPicked = true;
            }
            if (isBloatedFungus)
            {
                Manager.bloatedFungus += 1;
                isPicked = true;
            }
            if (isRootClippings)
            {
                Manager.rootClippings += 1;
                isPicked = true;

            }
        }
    }
}
