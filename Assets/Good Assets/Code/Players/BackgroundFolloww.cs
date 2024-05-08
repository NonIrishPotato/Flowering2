using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundFollow : MonoBehaviour
{
    public Transform player; // Reference to the player's transform
    public Transform Background;
    public float followSpeed = 2f; // Speed at which the background follows the player

    private Vector3 lastPlayerPosition; // Store the player's previous position

    void Start()
    {
        // Initialize lastPlayerPosition to the player's initial position
        lastPlayerPosition = player.position;
    }

    void Update()
    {
        // Calculate the movement vector based on the difference between the current and previous player positions
        Vector3 moveVector = player.position - lastPlayerPosition;

        // Calculate the target position for the background
        Vector3 targetPosition = Background.transform.position + moveVector;

        // Smoothly move the background towards the target position
        Background.transform.position = Vector3.Lerp(Background.transform.position, targetPosition, followSpeed * Time.deltaTime);

        // Update lastPlayerPosition to the current player position
        lastPlayerPosition = player.position;
    }
}