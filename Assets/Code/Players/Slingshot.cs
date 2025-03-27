using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Slingshot : MonoBehaviour
{
    public GameObject infiniteAmmoPrefab;
    public GameObject smokeBombPrefab;
    public Transform launchPoint;
    public float launchForce = 10f;
    public float spinForce = 100f; // New variable to control the spin force
    public Animator animator;
    public LineRenderer lineRenderer;
    public int lineSegmentCount = 20;
    public float lineLength = 5f; // New variable to control the length of the line renderer

    private GameObject currentAmmoPrefab;
    private int currentAmmoType = 0; // 0 for infinite ammo, 1 for smoke bomb
    private GameManager gameManager;
    private bool isHolding = false;
    private float holdTime = 0f;
    public const float requiredHoldTime = .5f; // Required hold time in seconds

    void Start()
    {
        gameManager = FindObjectOfType<GameManager>();
        SwitchAmmoType();
        lineRenderer.positionCount = lineSegmentCount;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Q))
        {
            currentAmmoType = (currentAmmoType + 1) % 2; // Switch between 0 and 1, allows for more ammo types in the future
            SwitchAmmoType();
        }

        if (Input.GetMouseButtonDown(0) && !isHolding)
        {
            StartCoroutine(StartHolding());
        }

        if (Input.GetMouseButton(0) && isHolding)
        {
            holdTime += Time.deltaTime;
            UpdateTrajectory();
        }

        if (Input.GetMouseButtonUp(0) && isHolding)
        {
            StopHolding();
        }
    }

    IEnumerator StartHolding()
    {
        isHolding = true;
        holdTime = 0f;
        animator.SetTrigger("StartHolding");
        yield return new WaitForSeconds(0.5f); // Wait for 0.5 seconds

        if (Input.GetMouseButton(0)) // Check if the mouse button is still held down
        {
            animator.SetBool("IsHolding", true);
        }
        else
        {
            isHolding = false;
            animator.SetBool("IsHolding", false);
        }
    }

    void StopHolding()
    {
        isHolding = false;
        animator.SetBool("IsHolding", false);
        animator.SetTrigger("Release");

        if (holdTime >= requiredHoldTime)
        {
            LaunchAmmo();
        }
        else
        {
            Debug.Log("Hold time was not sufficient to launch ammo.");
        }

        lineRenderer.enabled = false; // Hide the trajectory line
    }

    void SwitchAmmoType()
    {
        if (currentAmmoType == 0)
        {
            currentAmmoPrefab = infiniteAmmoPrefab;
        }
        else if (currentAmmoType == 1)
        {
            currentAmmoPrefab = smokeBombPrefab;
        }
    }

    void LaunchAmmo()
    {
        Debug.Log("Attempting to launch ammo. Current ammo type: " + currentAmmoType);
        Debug.Log("Current smoke bombs in inventory: " + gameManager.smokeBombs);

        if (currentAmmoType == 1 && gameManager.smokeBombs <= 0)
        {
            Debug.Log("No smoke bombs left!");
            return;
        }

        GameObject ammo = Instantiate(currentAmmoPrefab, launchPoint.position, Quaternion.identity);
        Rigidbody2D rb = ammo.GetComponent<Rigidbody2D>();
        Vector2 launchDirection = (GetMouseWorldPosition() - (Vector2)launchPoint.position).normalized;
        rb.AddForce(launchDirection * launchForce, ForceMode2D.Impulse);

        // Determine the spin direction based on the cursor position
        float spinDirection = GetMouseWorldPosition().x < launchPoint.position.x ? -1f : 1f;
        rb.angularVelocity = spinForce * spinDirection; // Apply spin to the ammo

        Destroy(ammo, 3f); // Destroy the ammo object after 3 seconds

        if (currentAmmoType == 1)
        {
            gameManager.smokeBombs--;
            Debug.Log("Smoke bomb launched. Remaining smoke bombs: " + gameManager.smokeBombs);
        }
    }

    void UpdateTrajectory()
    {
        Vector2 launchDirection = (GetMouseWorldPosition() - (Vector2)launchPoint.position).normalized;
        Vector2[] trajectoryPoints = CalculateTrajectory(launchPoint.position, launchDirection * launchForce, lineSegmentCount);

        lineRenderer.enabled = true;
        lineRenderer.positionCount = trajectoryPoints.Length;
        for (int i = 0; i < trajectoryPoints.Length; i++)
        {
            lineRenderer.SetPosition(i, trajectoryPoints[i]);
        }
    }

    Vector2[] CalculateTrajectory(Vector2 startPosition, Vector2 initialVelocity, int segmentCount)
    {
        Vector2[] segments = new Vector2[segmentCount];
        segments[0] = startPosition;
        Vector2 velocity = initialVelocity;

        for (int i = 1; i < segmentCount; i++)
        {
            float time = (i / (float)segmentCount) * lineLength; // Use lineLength to control the length of the trajectory
            segments[i] = segments[0] + velocity * time + 0.5f * Physics2D.gravity * time * time;
        }

        return segments;
    }

    Vector2 GetMouseWorldPosition()
    {
        Vector3 mouseScreenPosition = Input.mousePosition;
        mouseScreenPosition.z = Camera.main.WorldToScreenPoint(launchPoint.position).z;
        return Camera.main.ScreenToWorldPoint(mouseScreenPosition);
    }
}
