using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
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
    public float jumpJetpackInitialFuelCost = 11f; // The initial fuel cost to start the jump jetpack
    public float jumpJetpackFuelCostPerSecond = 10f; // The fuel cost per second while using the jump jetpack
    public float maxFuel = 25f; // The maximum fuel capacity
    public float fuelRechargeRate = 100000000f; // The rate at which fuel recharges per second

    private float currentFuel;
    private bool isJumping = false;

    private Rigidbody2D rb;
    public bool isGrounded;
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
    public static bool isFacingLeft;

    public static Player_Movement Instance;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
        else if (Instance != null)
        {
            Destroy(gameObject);
            Instance = this;
        }
    }

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
                animator.SetTrigger("isLanded");
            }

            if (!isGrounded && !isJumping)
            {
                animator.SetBool("isFalling", true);
            }

            if (!isGrounded&&isJumping&&currentFuel<=0)
            {
                animator.SetBool("isFalling", true);
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
        Debug.Log("Crouching: " + isCrouching); // Debug to check if crouching is being set correctly
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

        if (Mathf.Abs(horizontalInput) > 0.1f)
        {
            animator.SetBool("isWalking", true); // plays the walking animation if in motion
            animator.SetBool("isIdle", false);

            Vector2 moveDirection = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);
            rb.velocity = moveDirection;

            bool isMovingLeft = horizontalInput < 0;

            localIsWalking = true;

            if (isMovingLeft)
            {
                isFacingLeft = true;
                Debug.Log("Facing Left: " + isFacingLeft); // Debug to check if facing left
            }
            else
            {
                isFacingLeft = false;
                Debug.Log("Facing Right: " + isFacingLeft); // Debug to check if facing right
            }

            animator.SetBool("isFacingLeft", isFacingLeft);

            if (localIsWalking && isGrounded && !AudioManager.Instance.sfxSource.isPlaying)
            {
                AudioManager.Instance.sfxSource.Play();
            }
        }
        else
        {
            animator.SetBool("isWalking", false); // stops the walking animation if not in motion
            localIsWalking = false;
            IdleState();
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

    public void CheckForDamage()
    {
        if (canTakeDamage)
        {
            TakeDamage();
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
        animator.ResetTrigger("TakeDamage");
        Debug.Log("reset Trigger");
        canTakeDamage = true;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Enemy") && canTakeDamage)
        {
            CheckForDamage();
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
        bool isIdle;
        if (isGrounded && !localIsWalking && !isCrouching)
            isIdle = true;
        else
            isIdle = false;
        animator.SetBool("isIdle", isIdle);
    }
}

