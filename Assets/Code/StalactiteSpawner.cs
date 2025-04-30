using UnityEngine;

public class StalactiteSpawner : MonoBehaviour
{
    public StalactiteFalling stalactiteFalling; // Reference to the StalactiteFalling script
    public Transform player; // Reference to the player's transform

    void Update()
    {
        // Trigger the SpawnStalactite function when the space key is pressed
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (stalactiteFalling != null && player != null)
            {
                stalactiteFalling.SpawnStalactite(player.position);
            }
            else
            {
                Debug.LogError("StalactiteFalling or Player reference is missing!");
            }
        }
    }
}
