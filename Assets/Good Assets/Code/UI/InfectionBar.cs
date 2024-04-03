using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionBar : MonoBehaviour
{
    public GameManager gameManager;
    public float value;
    public Slider infection;
    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        value = gameManager.InfectionBar;
        infection.value = value;

        if (infection.value == 1)
        {
            gameManager.currentHealth = 0;
        }
    }
}
