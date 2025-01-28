using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InfectionBar : MonoBehaviour
{
    public GameManager gameManager;
    public float value;
    public Slider infection;

    public Image Background;
    public Image Fill;
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

        Color spriteColor1 = Background.color;
        Color spriteColor2 = Fill.color;

        spriteColor1.a = 1 - value;

        spriteColor2.a = value;

        Background.color = spriteColor1;
        Fill.color = spriteColor2;


        if (infection.value == 1)
        {
            gameManager.currentHealth = 0;
        }
    }
}
