using System.Collections;
using UnityEngine;

public class Player_Movement : MonoBehaviour
{
    public GameManager gameManager;

    [Header("Movement Settings")]
    public float walkSpeed = 3f;
    public float crouchSpeed = 2f;
    public float sprintSpeed = 8f;
    public float sprintDuration = 3f;
    public float sprintCooldown = 5f;
    public float jumpForce = 5f;
    public float slowfallGravity = 0.2f;

    [Header("Damage Settings")]
    public float damageForce = 10f;
    public float damageCooldown = 2f;

    private Rigidbody2D rb;
    private bool isGrounded;
    private Transform groundCheck;
    private Collider2D myCollider;
    private float moveSpeed;

    private bool isCrouching = false;
    private bool isSprinting = false;
    private bool canSprint = true;
    private bool canTakeDamage = true;
    private bool isJumping = false;
    private bool localIsWalking = false;

    [Header("Animation")]
    private static Animator animator;
    private static bool isFacingLeft, isFacingRight;
    private static string _currentState;

    private void Start()
    {
        gameManager = GameManager.Instance;
        rb = GetComponent<Rigidbody2D>();
        myCollider = GetComponent<Collider2D>();
        groundCheck = transform.Find("GroundCheck");
        animator = GetComponent<Animator>();
        moveSpeed = walkSpeed;
    }

    private void Update()
    {
        if (PauseMenuScript.isPaused) return;

        isGrounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
        if (isGrounded) isJumping = false;

        if (!canTakeDamage) return;

        HandleCrouch();
        HandleSprint();
        MoveCharacter();
        Jump();
        ApplySlowfall();
        IdleState();
    }

    private void HandleCrouch()
    {
        if (Input.GetKey(KeyCode.LeftControl) && !isSprinting)
        {
            isCrouching = true;
            moveSpeed = crouchSpeed;
            ChangeAnimationState(isFacingRight ? "Crouch_Idle_FR" : "Crouch_Idle_FL");
        }
        else
        {
            isCrouching = false;
            moveSpeed = walkSpeed;
        }
    }

    private void HandleSprint()
    {
        if (Input.GetKey(KeyCode.LeftShift) && !isCrouching && canSprint)
        {
            isSprinting = true;
            moveSpeed = sprintSpeed;
            StartCoroutine(SprintCooldown());
        }
        else
        {
            isSprinting = false;
            moveSpeed = walkSpeed;
        }
    }

    private void MoveCharacter()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        rb.velocity = new Vector2(horizontalInput * moveSpeed, rb.velocity.y);

        if (horizontalInput != 0)
        {
            isFacingRight = horizontalInput > 0;
            isFacingLeft = horizontalInput < 0;
            localIsWalking = true;

            if (isGrounded && !isCrouching)
            {
                ChangeAnimationState(isFacingRight ? "WalK_FR" : "WalK_FL");
            }
        }
        else
        {
            localIsWalking = false;
        }

        if (localIsWalking && isGrounded && !AudioManager.Instance.sfxSource.isPlaying)
        {
            AudioManager.Instance.PlaySFX("Walk");
        }
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump"))
        {
            if (isGrounded)
            {
                rb.velocity = new Vector2(rb.velocity.x, jumpForce);
                ChangeAnimationState(isFacingRight ? "Jump_FR" : "Jump_FL");
                StartCoroutine(AnimationTransition());
            }
            else if (!isJumping)
            {
                isJumping = true;
                ChangeAnimationState(isFacingRight ? "Mid_Air_Glide_FR" : "Mid_Air_Glide_FL");
            }

            AudioManager.Instance.PlaySFXtheSequal("Jump");
        }

        if (Input.GetButtonUp("Jump") || isGrounded)
        {
            isJumping = false;
            AudioManager.Instance.sfxSourceTheSequal.Stop();
        }
    }

    private IEnumerator AnimationTransition()
    {
        yield return new WaitForSeconds(0.4f);
        ChangeAnimationState(isFacingRight ? "Mid_Air_Glide_FR" : "Mid_Air_Glide_FL");
    }

    private void ApplySlowfall()
    {
        rb.gravityScale = (Input.GetButton("Jump") && !isGrounded) ? slowfallGravity : 1f;
    }

    private void IdleState()
    {
        if (!isJumping && isGrounded && !localIsWalking)
        {
            ChangeAnimationState(isFacingRight ? "Player_Idle_FR" : "Id_FL");
        }
    }

    private IEnumerator SprintCooldown()
    {
        yield return new WaitForSeconds(sprintCooldown);
        canSprint = true;
    }

    private IEnumerator DamageCooldown()
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
        rb.velocity = Vector2.zero;
        Vector2 launchDirection = (transform.position - myCollider.transform.position).normalized;
        rb.AddForce(launchDirection * damageForce, ForceMode2D.Impulse);

        AudioManager.Instance.PlaySFX("Player is Hurt");
        StartCoroutine(DamageCooldown());
    }

    private void ChangeAnimationState(string newState)
    {
        if (_currentState == newState) return;
        animator.Play(newState);
        _currentState = newState;
    }
}
