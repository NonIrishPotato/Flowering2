using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoleBoss : MonoBehaviour
{
    public GameObject smokeBombPrefab;
    public GameObject stalactitePrefab; // Assign the stalactite prefab
    public ParticleSystem groundCrumblingEffect;
    public GameObject player;

    public float roarDuration = 3f;
    public int roarAfterMinAttacks = 3;
    public int roarAfterMaxAttacks = 5;

    public float stompChargeSpeed = 10f;
    public float stalactiteSpawnDuration = 3f;

    public int minStalactites = 2; // Minimum number of stalactites to spawn
    public int maxStalactites = 4; // Maximum number of stalactites to spawn

    public float burrowYOffset = -3f;
    public float biteWindupTime = 2f;
    public float burrowStalactiteInterval = 1f;

    public float undergroundChance = 0.2f;
    public float attackVarianceScaling = 0.5f;

    private int health = 4;
    private int attacksSinceLastRoar = 0;
    private List<string> availableAttacks = new List<string> { "StompCharge", "StompStalactite", "BurrowBite", "BurrowStalactite" };
    private Vector3 playerPosition;

    private Animator animator;
    private bool isUnderground = false;
    private bool isAttacking = false;
    private float startingYPosition;

    private void Start()
    {
        animator = GetComponent<Animator>();
        StartCoroutine(AttackLoop());
        startingYPosition = transform.position.y;
    }

    private IEnumerator AttackLoop()
    {
        while (health > 0)
        {
            if (!isAttacking)
            {
                PerformAttack(FindPlayerPosition());
            }
            yield return null;
        }
    }

    private Vector3 FindPlayerPosition()
    {
         
        return player != null ? player.transform.position : transform.position;
    }

    public void PerformAttack(Vector3 playerPos)
    {
        if (isAttacking) return;

        isAttacking = true;
        playerPosition = playerPos;

        if (Random.value < undergroundChance && !isUnderground)
        {
            Debug.Log("Boss is going underground.");
            StartCoroutine(GoUnderground());
            return;
        }

        // Prevent roaring while underground
        if (!isUnderground && attacksSinceLastRoar >= Random.Range(roarAfterMinAttacks, roarAfterMaxAttacks + 1))
        {
            Debug.Log("Boss is roaring.");
            StartCoroutine(Roar());
            return;
        }

        string selectedAttack = SelectAttack();
        attacksSinceLastRoar++;

        Debug.Log($"Boss is performing attack: {selectedAttack}");

        switch (selectedAttack)
        {
            case "StompCharge":
                StartCoroutine(StompChargeAttack());
                break;
            case "StompStalactite":
                StartCoroutine(StompStalactiteAttack());
                break;
            case "BurrowBite":
                if (isUnderground)
                {
                    Debug.Log("Boss is performing Burrow Bite Attack.");
                    StartCoroutine(BurrowBiteAttack());
                }
                else
                {
                    Debug.LogWarning("Boss tried to perform Burrow Bite Attack but is not underground!");
                    isAttacking = false;
                }
                break;
            case "BurrowStalactite":
                if (isUnderground)
                {
                    Debug.Log("Boss is performing Burrow Stalactite Attack.");
                    StartCoroutine(BurrowStalactiteAttack());
                }
                else
                {
                    Debug.LogWarning("Boss tried to perform Burrow Stalactite Attack but is not underground!");
                    isAttacking = false;
                }
                break;
        }
    }

    private string SelectAttack()
    {
        // Filter available attacks based on the boss's state
        List<string> validAttacks = isUnderground
            ? new List<string> { "BurrowBite", "BurrowStalactite" } // Underground attacks only
            : new List<string> { "StompCharge", "StompStalactite"}; // Above-ground attacks only

        // Select a random valid attack
        string attack = validAttacks[Random.Range(0, validAttacks.Count)];
        return attack;
    }

    private IEnumerator StompChargeAttack()
    {
        Debug.Log("Boss is executing Stomp Charge Attack.");
        animator.Play(playerPosition.x < transform.position.x ? "StompChargeLeft" : "StompChargeRight");
        Vector3 targetPosition = new Vector3(playerPosition.x, transform.position.y, transform.position.z);

        while (Vector3.Distance(transform.position, targetPosition) > 0.1f)
        {
            transform.position = Vector3.MoveTowards(transform.position, targetPosition, stompChargeSpeed * Time.deltaTime);
            yield return null;
        }

        isAttacking = false;
    }

    private IEnumerator StompStalactiteAttack()
    {
        Debug.Log("Boss is executing Stomp Stalactite Attack.");

        if (stalactitePrefab == null || player == null)
        {
            Debug.LogError("StalactitePrefab or Player reference is missing!");
            isAttacking = false;
            yield break;
        }

        animator.Play(playerPosition.x < transform.position.x ? "StompStalactiteLeft" : "StompStalactiteRight");

        int stalactiteCount = Random.Range(minStalactites, maxStalactites + 1); // Randomize the number of stalactites
        for (int i = 0; i < stalactiteCount; i++)
        {
            SpawnStalactite(player.transform.position);
            yield return new WaitForSeconds(burrowStalactiteInterval);
        }

        isAttacking = false;
    }

    private IEnumerator BurrowBiteAttack()
    {
        Debug.Log("Boss is executing Burrow Bite Attack.");

        // Lock in the player's current position at the start of the attack
        Vector3 lockedPosition = playerPosition;

        // Ensure the boss does not go above its starting Y position
        lockedPosition.y = Mathf.Min(lockedPosition.y, startingYPosition);

        // Play the ground crumbling effect
        groundCrumblingEffect.Play();

        // Wait for the windup time (3 seconds)
        yield return new WaitForSeconds(biteWindupTime);

        // Move the boss to the locked position
        transform.position = lockedPosition;

        // Play the bite animation
        animator.Play(playerPosition.x < transform.position.x ? "BiteLeft" : "BiteRight");

        // Mark the boss as no longer underground
        isUnderground = false;

        // End the attack
        isAttacking = false;
    }

    private IEnumerator BurrowStalactiteAttack()
    {
        Debug.Log("Boss is executing Burrow Stalactite Attack.");

        if (stalactitePrefab == null || player == null)
        {
            Debug.LogError("StalactitePrefab or Player reference is missing!");
            isAttacking = false;
            yield break;
        }

        int stalactiteCount = Random.Range(minStalactites, maxStalactites + 1); // Randomize the number of stalactites
        for (int i = 0; i < stalactiteCount; i++)
        {
            SpawnStalactite(player.transform.position);
            yield return new WaitForSeconds(burrowStalactiteInterval);
        }

        isAttacking = false;
    }

    private void SpawnStalactite(Vector3 position)
    {
        if (stalactitePrefab == null)
        {
            Debug.LogError("StalactitePrefab is not assigned!");
            return;
        }

        // Generate a random x offset between -2 and 2
        float randomXOffset = Random.Range(-2f, 2f);

        // Adjust the spawn position with the random x offset and 5 units above the player's position
        Vector3 spawnPosition = new Vector3(position.x + randomXOffset, position.y + 5f, position.z);

        // Use the prefab's rotation
        Quaternion spawnRotation = stalactitePrefab.transform.rotation;

        // Instantiate the stalactite prefab at the adjusted position with its assigned rotation
        GameObject stalactite = Instantiate(stalactitePrefab, spawnPosition, spawnRotation);

        // Destroy the stalactite after 5 seconds
        Destroy(stalactite, 5f);
    }

    private IEnumerator GoUnderground()
    {
        Debug.Log("Boss is going underground.");
        animator.Play("Burrow");
        yield return new WaitForSeconds(1f);

        isUnderground = true;
        transform.position = new Vector3(transform.position.x, transform.position.y + burrowYOffset, transform.position.z);
        isAttacking = false;
    }

    private IEnumerator Roar()
    {
        Debug.Log("Boss is roaring.");
        animator.Play("Roar");
        float elapsedTime = 0f;

        while (elapsedTime < roarDuration)
        {
            elapsedTime += Time.deltaTime;

            Collider[] colliders = Physics.OverlapSphere(transform.position, 5f);
            foreach (Collider collider in colliders)
            {
                if (collider.gameObject == smokeBombPrefab)
                {
                    TakeDamage(1);
                    Destroy(collider.gameObject);
                }
            }

            yield return null;
        }
        Debug.Log("Roar finished.");
        attacksSinceLastRoar = 0;
        isAttacking = false;
    }


    private void TakeDamage(int damage)
    {
        health -= damage;
        Debug.Log($"Boss took {damage} damage. Remaining health: {health}");
        if (health <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        Debug.Log("Boss has died.");
        animator.Play("Die");
        Destroy(gameObject);
    }
   
}
