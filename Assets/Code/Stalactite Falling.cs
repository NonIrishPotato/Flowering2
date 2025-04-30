using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StalactiteFalling : MonoBehaviour
{
    public GameObject stalactitePrefab; // Assign the stalactite prefab in the Inspector

    // Method to spawn a stalactite above the player
    public void SpawnStalactite(Vector3 playerPosition)
    {
        // Calculate a random x-offset between -2 and 2
        float randomXOffset = Random.Range(-2f, 2f);

        // Determine the spawn position
        Vector3 spawnPosition = new Vector3(playerPosition.x + randomXOffset, playerPosition.y + 5f, playerPosition.z);

        // Instantiate the stalactite with the prefab's original rotation
        GameObject stalactite = Instantiate(stalactitePrefab, spawnPosition, stalactitePrefab.transform.rotation);

        // Destroy the stalactite after 5 seconds
        Destroy(stalactite, 5f);
    }
}
