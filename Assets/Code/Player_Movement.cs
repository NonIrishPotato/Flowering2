using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 5f;
    public float crouchSpeed = 2f;
    public float sprintSpeed = 8f;
    public float sprintDuration = 3f;
    public float sprintCooldown = 5f;
    public float jumpForce = 5f;
    public float slowfallGravity = 0.2f;

    public float damageForce = 10f; // Adjust this value for the force applied to the player when damaged
    public float damageCooldown = 2f; // Adjust this value for the cooldown after taking damage

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform groundCheck;
    private Collider2D myCollider;

    private bool isCrouching = false;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private bool canSprint = true;
    private bool canTakeDamage = true;

    private void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        groundCheck = transform.Find("GroundCheck");
    }

    private void Update()
    {
        // Check if the character is grounded
        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));

        // Check for damage cooldown
        if (!canTakeDamage)
            return;

        // Crouch
        if (Input.GetKey(KeyCode.S) && !isSprinting)
        {
            Crouch();
        }
        else
        {
            StandUp();
        }

        // Sprint
        if (Input.GetKey(KeyCode.W) && !isCrouching && canSprint)
        {
            Sprint();
        }
        else
        {
            StopSprinting();
        }

        // Move the character
        MoveCharacter();

        // Jump
        Jump();

        // Slowfall
        ApplySlowfall();

        // Check for damage
        CheckForDamage();
    }

    private void Crouch()
    {
        gameManager.IsPlayerWalking = false;
        gameManager.IsPlayerCrouching = true;

        isCrouching = true;
        moveSpeed = crouchSpeed;
    }

    private void StandUp()
    {
        gameManager.IsPlayerCrouching = false;
        gameManager.IsPlayerWalking = true;

        isCrouching = false;
        moveSpeed = isCrouching ? crouchSpeed : 5f;
    }

    private void Sprint()
    {
        gameManager.IsPlayerWalking = false;
        gameManager.IsPlayerSprinting = true;

        isSprinting = true;
        moveSpeed = sprintSpeed;
        sprintTimer += Time.deltaTime;

        // Check if sprint duration is reached
        if (sprintTimer >= sprintDuration)
        {
            StopSprinting();
            StartCoroutine(SprintCooldown());
        }
    }

    private void StopSprinting()
    {
        gameManager.IsPlayerWalking = true;
        gameManager.IsPlayerSprinting = false;

        isSprinting = false;
        moveSpeed = isCrouching ? crouchSpeed : 5f;
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);
    }

    private void Jump()
    {
        //commented out grounded check just for the demo of sprint 2, need to fly around
        if (/*isGrounded && */Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
        }
    }

    private void ApplySlowfall()
    {
        if (Input.GetButton("Jump") && !isGrounded)
        {
            rb.gravityScale = slowfallGravity;
        }
        else
        {
            rb.gravityScale = 1f;
        }
    }

    private void CheckForDamage()
    {
        if (canTakeDamage)
        {
            // Implement player damage detection here (e.g., using OnCollisionEnter2D)
        }
    }

    IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
    }

    IEnumerator DamageCooldown()
    {
        yield return new WaitForSeconds(damageCooldown);
        canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            TakeDamage();
        }
    }

    private void TakeDamage()
    {
        canTakeDamage = false;

        // Halt the player briefly
        rb.velocity = Vector2.zero;

        // Apply force to launch the player back
        Vector2 launchDirection = (transform.position + myCollider.transform.position).normalized;
        rb.AddForce(launchDirection * damageForce, ForceMode2D.Impulse);

        StartCoroutine(DamageCooldown());
    }
}