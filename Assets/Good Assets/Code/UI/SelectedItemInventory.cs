using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.ParticleSystem;

public class SelectedItemInventory : MonoBehaviour
{

    public GameObject[] selection;
    public InventorySystem inventorySystem;
    public GameManager gameManager;
    public int SmokeBombTimer = 5;
    private int currentIndex = 0;
    public GameObject particlePrefab;
    public Rigidbody2D rb;
    // Start is called before the first frame update
    void Start()
    {
        rb = GameObject.Find("Player").GetComponent<Rigidbody2D>();
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        
        if (scroll != 0)
        {
            // Calculate the new index based on the scroll direction
            int newIndex = currentIndex + (scroll > 0 ? 1 : -1);

            // Ensure the index stays within bounds
            if (newIndex < 0)
            {
                newIndex = selection.Length - 1;
            }
            else if (newIndex >= selection.Length)
            {
                newIndex = 0;
            }

            

            // Activate the new item slot and deactivate the current one
            selection[currentIndex].SetActive(false);
            selection[newIndex].SetActive(true);

            // Update the current index
            currentIndex = newIndex;
        }
        if (Input.GetKeyDown(KeyCode.E))
        {
            Debug.Log("E Pressed");
            Debug.Log(inventorySystem.Items[currentIndex + 28]);
            if (inventorySystem.Items[currentIndex + 28] == 4 && gameManager.currentHealth != gameManager.maxHealth)
            {
                inventorySystem.Items[currentIndex + 28] = 0;
                gameManager.currentHealth += 1;
                //NectarBloom Here
            }
            if (inventorySystem.Items[currentIndex + 28] == 8)
            {
                inventorySystem.Items[currentIndex + 28] = 0;
                GameObject partical = Instantiate(particlePrefab, rb.position, Quaternion.identity);
                StartCoroutine(smokeBomb(partical));
                //Smoke Bomb here
            }
            if (inventorySystem.Items[currentIndex + 28] == 9 && gameManager.currentHealth != gameManager.maxHealth)
            {
                inventorySystem.Items[currentIndex + 28] = 0;
                gameManager.currentHealth = gameManager.maxHealth;
                //Healing Honey Here
            }
            if (inventorySystem.Items[currentIndex + 28] == 10 && gameManager.InfectionBar != 0)
            {
                inventorySystem.Items[currentIndex + 28] = 0;
                if(gameManager.InfectionBar <= .5f)
                {
                    gameManager.InfectionBar = 0;
                }
                else
                {
                    gameManager.InfectionBar -= .5f;
                }
                //Preventative Here
            }
            
        }
        inventorySystem.UpdateToolSlots();

        IEnumerator smokeBomb(GameObject partical)
        {
            gameManager.smokeBombActive = true;
            yield return new WaitForSeconds(SmokeBombTimer);
            gameManager.smokeBombActive = false;
            Destroy(partical, 1f);
        }

    }
}
