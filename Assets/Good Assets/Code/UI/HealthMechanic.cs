using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class HealthMechanic : MonoBehaviour
{

    public GameManager gameManager;

    public GameObject FullHealth;
    public GameObject thirdHealth;
    public GameObject secondHealth;
    public GameObject lastHealth;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        if(gameManager.currentHealth == 4)
        {
            FullHealth.SetActive(true);
            thirdHealth.SetActive(false);
            secondHealth.SetActive(false);
            lastHealth.SetActive(false);
        }
        if (gameManager.currentHealth == 3)
        {
            FullHealth.SetActive(false);
            thirdHealth.SetActive(true);
            secondHealth.SetActive(false);
            lastHealth.SetActive(false);
        }
        if (gameManager.currentHealth == 2)
        {
            FullHealth.SetActive(false);
            thirdHealth.SetActive(false);
            secondHealth.SetActive(true);
            lastHealth.SetActive(false);
        }
        if (gameManager.currentHealth == 1)
        {
            FullHealth.SetActive(false);
            thirdHealth.SetActive(false);
            secondHealth.SetActive(false);
            lastHealth.SetActive(true);
        }
    }
}
