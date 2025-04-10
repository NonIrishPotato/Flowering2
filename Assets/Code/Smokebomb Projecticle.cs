using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SmokebombProjecticle : MonoBehaviour
{
    // Reference to the smoke bomb game object
    public GameObject smokeBomb;
    public int SmokeBombTimer = 5;
    private GameManager gameManager;
    public GameObject particlePrefab;
    public Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        // Ensure the smoke bomb is assigned
        if (smokeBomb == null)
        {
            Debug.LogError("Smoke bomb game object is not assigned.");
        }

        gameManager = GameManager.Instance;

        // Ignore collision with the player
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            Collider2D playerCollider = player.GetComponent<Collider2D>();
            Collider2D projectileCollider = GetComponent<Collider2D>();
            if (playerCollider != null && projectileCollider != null)
            {
                Physics2D.IgnoreCollision(playerCollider, projectileCollider);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        // Update logic if needed
    }

    // This method is called when the collider enters a collision in 2D
    void OnCollisionEnter2D(Collision2D collision)
    {
        // Log the collision
        Debug.Log("Collision detected with: " + collision.gameObject.name);

        // Trigger the smoke bomb logic
        GameObject partical = Instantiate(particlePrefab, rb.position, Quaternion.identity);
        StartCoroutine(SmokeBombCoroutine());
    }

    IEnumerator SmokeBombCoroutine()
    {
        gameManager.smokeBombActive = true;
        yield return new WaitForSeconds(SmokeBombTimer);
        gameManager.smokeBombActive = false;
        Destroy(smokeBomb, 1f);
    }
}
