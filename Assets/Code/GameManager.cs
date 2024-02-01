using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{

    public bool IsPlayerHiding = false;
    public bool IsPlayerCrouching = false;
    public bool IsPlayerWalking = true;
    public bool IsPlayerSprinting = false;
    public int currentHealth = 4;
    public int maxHealth = 4;



    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (currentHealth <= 0)
        {
            Debug.Log("Death");
        }
    }
}
