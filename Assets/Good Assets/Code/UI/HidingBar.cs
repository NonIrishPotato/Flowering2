using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HidingBar : MonoBehaviour
{

    private GameManager gameManager;
    public Slider breath;
    public float drainspeed = .01f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
        breath.value = 1f;
        breath.gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
        if (gameManager.IsPlayerHiding == true)
        {
            breath.gameObject.SetActive(true);
        }
    





        if (gameManager.IsPlayerHiding == true && breath.value >= 0)
        {
            breath.gameObject.SetActive(true);
            breath.value -= drainspeed;
            if(breath.value == 0)
            {
                gameManager.canHide = false;
            }
        }
        else if (gameManager.IsPlayerHiding == false)
        {
            if (breath.value < .70f)
            {
                gameManager.canHide = false;
            }
            else
            {
                gameManager.canHide = true;
            }
            if (breath.value <= 1)
            {
                breath.value += drainspeed / 1.5f;
                if (breath.value == 1)
                {
                    breath.gameObject.SetActive(false);
                }
            }

        }


    }
}
