using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using static TMPro.SpriteAssetUtilities.TexturePacker_JsonArray;

public class Player_Movement : MonoBehaviour
{
    public GameManager gameManager;
    public float moveSpeed = 5f;
    public float walkSpeed = 3f;
    public float crouchSpeed = 2f;
    public float sprintSpeed = 8f;
    public float sprintDuration = 3f;
    public float sprintCooldown = 5f;
    public float jumpForce = 5f;

    public float damageForce = 10f; // Adjust this value for the force applied to the player when damaged
    public float damageCooldown = 2f; // Adjust this value for the cooldown after taking damage

    public float jumpJetpackForce = 2f; // The force applied by the jump jetpack
    public float jumpJetpackInitialFuelCost = 20f; // The initial fuel cost to start the jump jetpack
    public float jumpJetpackFuelCostPerSecond = 5f; // The fuel cost per second while using the jump jetpack
    public float maxFuel = 25f; // The maximum fuel capacity
    public float fuelRechargeRate = 25f; // The rate at which fuel recharges per second

    private float currentFuel;
    private bool isJumping = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform groundCheck;
    private Collider2D myCollider;

    private bool isCrouching = false;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private bool canSprint = true;
    private bool canTakeDamage = true;

    public Animator playerAnim;

    private bool localIsWalking = false; // Created this since the Gamemanager "isWalking" will always play the walking sound

    // Animation States
    private bool hasChangedAnimation = false;
    public static Animator animator;
    public static bool isFacingLeft, isFacingRight;

    private void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        currentFuel = maxFuel;
    }

    private void Update()
    {
        if (!PauseMenuScript.isPaused) // Everything will work until the game is paused. This is also to prevent sounds from playing while in the pause menu
        {
            if (gameManager.Frose == true || gameManager.PlayerFrozen)
            {
                rb.velocity = Vector2.zero;

                // ANIMATION: IDLE WHEN FROZEN
                if (!hasChangedAnimation)
                {
                    animator.SetBool("isIdle", true);
                    hasChangedAnimation = true; // Set the flag to true
                }
                return;
            }
            else
            {
                hasChangedAnimation = false; // Reset the flag when Frose becomes false
            }

            // Check if the character is grounded
            isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
            if (isGrounded)
            {
                isJumping = false;

            }

            if(!isGrounded && !isJumping)
            {
                animator.setBool(isFalling, true);
            }

            if(!isGrounded&&isJumping&&currentFuel<=0)
            {
                animator.setBool(isFalling, true);
            }

            // Check for damage cooldown
            if (!canTakeDamage)
                return;

            // Crouch
            if (Input.GetKey(KeyCode.LeftControl) && !isSprinting)
            {
                Crouch();
            }
            else
            {
                StandUp();
            }

            // Sprint
            if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && canSprint)
            {
                Sprint();
            }
            else if (!isCrouching)
            {
                StopSprinting();
            }

            // Move the character
            MoveCharacter();

            // Jump
            Jump();

            // Recharge fuel
            RechargeFuel();

            // Check for damage
            CheckForDamage();

            // Idle State
            IdleState();
        }
    }

    private void Crouch()
    {
        gameManager.IsPlayerWalking = false;
        gameManager.IsPlayerCrouching = true;

        isCrouching = true;
        moveSpeed = crouchSpeed;

        // ANIMATION: CROUCHING
        animator.SetBool("isCrouching", true);
        animator.SetBool("isWalking", localIsWalking);
    }

    private void StandUp()
    {
        gameManager.IsPlayerCrouching = false;
        gameManager.IsPlayerWalking = true;

        isCrouching = false;
        moveSpeed = walkSpeed;

        // ANIMATION: STAND UP FROM CROUCH
        animator.SetBool("isCrouching", false);
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

        // ANIMATION: SPRINTING
        animator.SetBool("isSprinting", true);
    }

    private void StopSprinting()
    {
        gameManager.IsPlayerWalking = true;
        gameManager.IsPlayerSprinting = false;

        isSprinting = false;
        moveSpeed = walkSpeed;

        // ANIMATION: STOP SPRINTING
        animator.SetBool("isSprinting", false);
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        Vector2 moveDirection = new Vector2(horizontalInput, 0);
        rb.velocity = new Vector2(moveDirection.x * moveSpeed, rb.velocity.y);

        if (Input.GetKeyDown(KeyCode.A) || Input.GetKeyDown(KeyCode.D))
        {
            localIsWalking = true;
        }
        else if (Input.GetKeyUp(KeyCode.A) || Input.GetKeyUp(KeyCode.D))
        {
            localIsWalking = false;
        }

        if (Input.GetKey(KeyCode.D)) // For the Right Side
        {
            isFacingRight = true;
            isFacingLeft = false;

            // ANIMATION: FACING RIGHT
            animator.SetBool("isFacingRight", true);
            animator.SetBool("isFacingLeft", false);
        }
        else if (Input.GetKey(KeyCode.A)) // For the Left Side
        {
            isFacingRight = false;
            isFacingLeft = true;

            // ANIMATION: FACING LEFT
            animator.SetBool("isFacingRight", false);
            animator.SetBool("isFacingLeft", true);
        }

        // ANIMATION: WALKING
        animator.SetBool("isWalking", localIsWalking);

        if (localIsWalking && !AudioManager.Instance.sfxSource.isPlaying && isGrounded)
        {
            AudioManager.Instance.sfxSource.Play();
        }
    }

    private void Jump()
    {
        if (isCrouching)
        {
            return; // Do not allow jumping if the player is crouching
        }

        if (isGrounded && Input.GetButtonDown("Jump"))
        {
            rb.velocity = new Vector2(rb.velocity.x, jumpForce);
            isJumping = true;
        }

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            // ANIMATION: JUMPING
            animator.SetTrigger("Jump");
        }
        else if (Input.GetButtonUp("Jump") || isGrounded)
        {
            isJumping = false;
            AudioManager.Instance.sfxSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.D) && !isGrounded)
        {
            // ANIMATION: GLIDING RIGHT
            animator.SetBool("isGliding", true);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isGrounded)
        {
            // ANIMATION: GLIDING LEFT
            animator.SetBool("isGliding", true);
        }

        if (Input.GetButtonDown("Jump"))
        {
            AudioManager.Instance.sfxSource.Play();
        }

        // Use the jump jetpack
        UseJumpJetpack();
    }

    // JETPACK is the naming convention for the flight mechanic for the player, it is not a jetpack.
    private void UseJumpJetpack()
    {
        if (isCrouching)
        {
            return; // Do not allow using the jetpack if the player is crouching
        }

        if (!isGrounded && Input.GetButtonDown("Jump") && currentFuel >= jumpJetpackInitialFuelCost)
        {
            // Initial activation of the jetpack
            rb.velocity = new Vector2(rb.velocity.x, jumpJetpackForce);
            currentFuel -= jumpJetpackInitialFuelCost;
            isJumping = true;

            // ANIMATION: JETPACK ACTIVATION
            animator.SetBool("isJumping", true);
        }

        if (!isGrounded && Input.GetButton("Jump") && isJumping && currentFuel > 0)
        {
            // Continuous usage of the jetpack
            rb.velocity = new Vector2(rb.velocity.x, jumpJetpackForce);
            currentFuel -= jumpJetpackFuelCostPerSecond * Time.deltaTime;

            // ANIMATION: JETPACK IN USE
            animator.SetBool("isJumping", true);
        }

        if (Input.GetButtonUp("Jump") || currentFuel <= 0)
        {
            // Stop using the jetpack
            isJumping = false;

            // ANIMATION: JETPACK STOP
            animator.SetBool("isJumping", false);
        }
    }

    private void RechargeFuel()
    {
        if (!isJumping && currentFuel < maxFuel && isGrounded)
        {
            currentFuel += fuelRechargeRate * Time.deltaTime;
            if (currentFuel > maxFuel)
            {
                currentFuel = maxFuel;
            }
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
        sprintTimer = 0;
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

        // ANIMATION: TAKING DAMAGE
        animator.SetTrigger("TakeDamage");

        AudioManager.Instance.sfxSource.Play();

        StartCoroutine(DamageCooldown());
    }

    void IdleState()
    {
        if (isGrounded && !localIsWalking && !isCrouching)
        {
            // ANIMATION: IDLE
            animator.SetBool("isIdle", true);
        }
        else
        {
            animator.SetBool("isIdle", false);
        }
    }
}
