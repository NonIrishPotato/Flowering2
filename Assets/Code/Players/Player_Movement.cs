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
    private bool isUsingJetpack = false;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform groundCheck;
    private Collider2D myCollider;

    private bool isCrouching = false;
    private bool isSprinting = false;
    private float sprintTimer = 0f;
    private bool canSprint = true;
    private bool canTakeDamage = true;
  

    private bool localIsWalking = false;//Created this since the Gamemanager "isWalking" will always play the walking sound
    private bool isJumping = false;

    //Animation States
    private bool hasChangedAnimation = false;
    public static Animator animator;
    public static bool isFacingLeft, isFacingRight;
    [HideInInspector] public static string _currentState;
    const string PLAYER_IDLE_FR = "Player_Idle_FR";
    const string Id_FL = "Id_FL";
    const string WALK_FR = "WalK_FR";
    const string WALK_LR = "WalK_FL";
    const string Jump_FR = "Jump_FR 0";
    const string Jump_FL = "Jump_FL";
    const string Mid_Air_Glide_FR = "Mid_Air_Glide_FR 0";
    const string Mid_Air_Glide_FL = "Mid_Air_Glide_FL 0";
    const string Crouch_Idle_FR = "Crouch_Idle_FR";
    const string Crouch_Idle_FL = "Crouch_Id_FL";
    const string Crouch_Walk_FR = "Crouch_Walk_FR";
    const string Crouch_Walk_FL = "Crouch_Walk_FL";
    const string PICK_FR = "Player_Pick_FR";
    const string PICK_FL = "PIck_FL";

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

                // Change animation only once when Frose becomes true
                if (!hasChangedAnimation)
                {
                    if (isFacingRight)
                    {
                        ChangeAnimationState(PLAYER_IDLE_FR);
                    }
                    else if (isFacingLeft)
                    {
                        ChangeAnimationState(Id_FL);
                    }
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
                isJumping = false;

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

            //Idle State
            IdleState();

            

        }
    }

    private void Crouch()
    {
        gameManager.IsPlayerWalking = false;
        gameManager.IsPlayerCrouching = true;

        isCrouching = true;
        moveSpeed = crouchSpeed;
        if (isFacingLeft && isCrouching && !localIsWalking)
        {
            ChangeAnimationState(Crouch_Idle_FL);
        }
        if (isFacingRight && isCrouching && !localIsWalking)
        {
            ChangeAnimationState(Crouch_Idle_FR);
        }

        if (isFacingLeft && isCrouching && localIsWalking)
        {
            ChangeAnimationState(Crouch_Walk_FL);
        }
        if (isFacingRight && isCrouching && localIsWalking)
        {
            ChangeAnimationState(Crouch_Walk_FR);
        }
    }

    private void StandUp()
    {
        gameManager.IsPlayerCrouching = false;
        gameManager.IsPlayerWalking = true;

        isCrouching = false;
        moveSpeed = walkSpeed;
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

        //animator.SetBool("isSprinting", true);
    }

    private void StopSprinting()
    {
        gameManager.IsPlayerWalking = true;
        gameManager.IsPlayerSprinting = false;

        isSprinting = false;
        moveSpeed = walkSpeed;
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

        if (Input.GetKey(KeyCode.D)) //For the Right Side
        {
            isFacingRight = true;
            isFacingLeft = false;
            if (!IsAnimationPlaying(animator, Jump_FR) || !IsAnimationPlaying(animator, Jump_FL))
            {
                if (isGrounded && localIsWalking && !isFacingLeft && !isCrouching)
                {
                    ChangeAnimationState(WALK_FR);
                }
            }
        }
        else if (Input.GetKey(KeyCode.A)) //For the Left Side
        {
            isFacingRight = false;
            isFacingLeft = true;
            if (!IsAnimationPlaying(animator, Jump_FR) || !IsAnimationPlaying(animator, Jump_FL))
            {
                if (isGrounded && localIsWalking && !isFacingRight && !isCrouching)
                {
                    ChangeAnimationState(WALK_LR);
                }
            }
        }

        if (localIsWalking && !AudioManager.Instance.sfxSource.isPlaying && isGrounded)
        {
            //AudioManager.Instance.sfxSource.Play();
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
        }

        if (!isJumping && Input.GetButtonDown("Jump"))
        {
            isJumping = true;
            if (isFacingRight)
            {
                ChangeAnimationState(Jump_FR);
                StartCoroutine(AnimationTransistion());
            }
            if (isFacingLeft)
            {
                ChangeAnimationState(Jump_FL);
                StartCoroutine(AnimationTransistion());
            }
        }
        else if (Input.GetButtonUp("Jump") || isGrounded)
        {
            isJumping = false;
            //AudioManager.Instance.sfxSource.Play();
        }

        if (Input.GetKeyDown(KeyCode.D) && !isGrounded)
        {
            ChangeAnimationState(Mid_Air_Glide_FR);
        }
        if (Input.GetKeyDown(KeyCode.A) && !isGrounded)
        {
            ChangeAnimationState(Mid_Air_Glide_FL);
        }

        if (Input.GetButtonDown("Jump"))
        {
            AudioManager.Instance.sfxSource.Play();
        }

        // Use the jump jetpack
        UseJumpJetpack();
    }

    //JETPACK is the naming convention for the flight mechanic for the player, it is not a jetpack.
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
            isUsingJetpack = true;
        }

        if (!isGrounded && Input.GetButton("Jump") && isUsingJetpack && currentFuel > 0)
        {
            // Continuous usage of the jetpack
            rb.velocity = new Vector2(rb.velocity.x, jumpJetpackForce);
            currentFuel -= jumpJetpackFuelCostPerSecond * Time.deltaTime;
        }

        if (Input.GetButtonUp("Jump") || currentFuel <= 0)
        {
            // Stop using the jetpack
            isUsingJetpack = false;
        }
    }

    private void RechargeFuel()
    {
        if (!isUsingJetpack && currentFuel < maxFuel && isGrounded)
        {
            currentFuel += fuelRechargeRate * Time.deltaTime;
            if (currentFuel > maxFuel)
            {
                currentFuel = maxFuel;
            }
        }
    }



    IEnumerator AnimationTransistion()
    {
        yield return new WaitForSeconds(.4f);
        if (isFacingRight)
        {
            ChangeAnimationState(Mid_Air_Glide_FR);
        }
        if (isFacingLeft)
        {
            ChangeAnimationState(Mid_Air_Glide_FL);
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

        AudioManager.Instance.sfxSource.Play();

        StartCoroutine(DamageCooldown());
    }

    // Change animation state
    public void ChangeAnimationState(string newState)
    {
        if (newState == _currentState)
        {
            return;
        }

        animator.Play(newState);

        _currentState = newState;
    }

    // Check if a specific animation is playing
    // Parameter named "0" is the animation layer
    bool IsAnimationPlaying(Animator animator, string stateName)
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName(stateName) &&
            animator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1.0f)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    void IdleState()
    {
        if (!IsAnimationPlaying(animator, Jump_FR))
        {
            if (isGrounded && !localIsWalking && isFacingRight && !isCrouching)
            {
                ChangeAnimationState(PLAYER_IDLE_FR);
            }
            if (isGrounded && !localIsWalking && isFacingLeft && !isCrouching)
            {
                ChangeAnimationState(Id_FL);
            }
        }
    }
}
