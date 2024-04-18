using System.Collections;
using UnityEngine;

public class Enemy_Patrol : MonoBehaviour
{
    public GameManager gameManager;

    public bool outOfBounds = false;

    public float moveSpeed = 3f;
    public float PatrolSpeed = 3f;
    public float ChaseSpeed = 5f;
    public Transform[] waypoints;
    public float waypointRadius = 1f;
    public float waypointWaitTime = 1f;

    public float playerCrouchDetectionRadius = 1f;
    public float playerWalkDetectionRadius = 2f;
    public float playerRunDetectionRadius = 4f;

    public float smallHazardRadius = 4f;
    public float meduimHazardRadius = 6f;
    public float largeHazardRadius = 8f;

    private float playerDetectionRadius = 0f;
    public LayerMask playerLayer;

    public float jumpForce = 5f;
    public float obstacleDetectionDistance = 1f;

    private Rigidbody2D rb;
    private int currentWaypointIndex = 0;
    private bool isFacingRight = true;
    private bool isWaitingAtWaypoint = false;

    private bool wasPlayerDetected = false;
    public float timeBeforeReturningToPatrol = 2f;
    private float timeSinceLastDetection = 0f;

    private Transform player;
    public Rigidbody2D playerRb;
    private bool canHitPlayer = true;

    private enum EnemyState
    {
        Patrolling,
        Chasing,
        Recover,
        Distracted

    }

    private EnemyState currentState = EnemyState.Patrolling;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        Physics2D.IgnoreLayerCollision(rb.gameObject.layer, LayerMask.NameToLayer("Enemy"), true);
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();

        if (waypoints.Length == 0)
        {
            Debug.LogError("No waypoints assigned to the enemy!");
        }

        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    private void Update()
    {
        if (waypoints.Length == 0)
            return;

        bool isPlayerNearby = Physics2D.OverlapCircle(transform.position, playerDetectionRadius, playerLayer);

        if (outOfBounds)
        {
            currentState = EnemyState.Patrolling;
        }
        else if (isPlayerNearby && gameManager.IsPlayerHiding == false && canHitPlayer == true)
        {
            wasPlayerDetected = true;
            
            currentState = EnemyState.Chasing;
            timeSinceLastDetection = Time.time;
        }
        else if (wasPlayerDetected && Time.time - timeSinceLastDetection > timeBeforeReturningToPatrol && canHitPlayer == true)
        {
            wasPlayerDetected = false;
            currentState = EnemyState.Patrolling;
        }
        else if (!canHitPlayer)
        {
            currentState = EnemyState.Recover;
        }

        switch (currentState)
        {
            case EnemyState.Patrolling:
                moveSpeed = PatrolSpeed;
                Patrol();
                break;
            case EnemyState.Chasing:
                moveSpeed = ChaseSpeed;
                ChasePlayer();
                break;
            case EnemyState.Recover:
                moveSpeed = 0;
                Recover();
                break;
        }


        if (gameManager.IsPlayerCrouching || gameManager.smokeBombActive)
        {
            playerDetectionRadius = playerCrouchDetectionRadius;
        }
        else if (gameManager.IsPlayerWalking)
        {
            playerDetectionRadius = playerWalkDetectionRadius;
        }
        else if (gameManager.IsPlayerSprinting)
        {
            playerDetectionRadius = playerRunDetectionRadius;
        }

        if(gameManager.smallHazardHit)
        {
            playerDetectionRadius = smallHazardRadius;
            Debug.Log("SmallHazardHit " + playerDetectionRadius);
        }
        else if(gameManager.mediumHazardHit)
        {
            playerDetectionRadius = meduimHazardRadius;
            Debug.Log("MediumHazardHit " + playerDetectionRadius);
        }
        else if(gameManager.largeHazardHit)
        {
            playerDetectionRadius = largeHazardRadius;
            Debug.Log("LargeHazardHit " + playerDetectionRadius);
        }

       
    }

    private void Patrol()
    {
        float distanceToWaypoint = Vector2.Distance(transform.position, waypoints[currentWaypointIndex].position);

        if (distanceToWaypoint < waypointRadius && !isWaitingAtWaypoint)
        {
            isWaitingAtWaypoint = true;
            Invoke("SwitchWaypoint", waypointWaitTime);
        }

        if (!isWaitingAtWaypoint)
        {
            Vector2 targetPosition = waypoints[currentWaypointIndex].position;
            Vector2 moveDirection = (targetPosition - (Vector2)transform.position).normalized;
            float distance = Vector2.Distance(transform.position, targetPosition);

            if (distance > waypointRadius)
            {
                rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
                FlipDirection(moveDirection.x);
            }
            else
            {
                rb.velocity = Vector2.zero;
            }

            //CheckForObstacles(moveDirection.x);
        }
    }

    private void ChasePlayer()
    {
        Vector2 moveDirection = (player.position - transform.position).normalized;
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
        FlipDirection(moveDirection.x);

        //CheckForObstacles(moveDirection.x);
        Physics2D.IgnoreLayerCollision(playerRb.gameObject.layer, LayerMask.NameToLayer("Enemy"), false);
    }

    private void Recover()
    {
        //Nothing
        moveSpeed = 0;
    }

    private void SwitchWaypoint()
    {
        isWaitingAtWaypoint = false;
        currentWaypointIndex = (currentWaypointIndex + 1) % waypoints.Length;
    }

    private void FlipDirection(float directionX)
    {
        if ((directionX > 0 && !isFacingRight) || (directionX < 0 && isFacingRight))
        {
            isFacingRight = !isFacingRight;
            transform.localScale = new Vector3(-transform.localScale.x, transform.localScale.y, transform.localScale.z);
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Ground"))
        {
            Jump();
        }
    }

    private void Jump()
    {
        rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        Debug.Log("Jumping with force: " + jumpForce);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && canHitPlayer && gameManager.IsPlayerHiding == false)
        {
            Debug.Log("Hit Player");
            gameManager.currentHealth -= 1;
            Debug.Log(gameManager.currentHealth);
            canHitPlayer = false;
            moveSpeed = 0;
            StartCoroutine(PauseChase());
        }
    }

    private IEnumerator PauseChase()
    {
        Debug.Log("Pause Start");

        yield return new WaitForSeconds(3f); // Adjust this duration as needed

        Debug.Log("Pause End");
        canHitPlayer = true;
    }
}
