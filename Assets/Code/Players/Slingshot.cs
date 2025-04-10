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
    private InventorySystem inventorySystem;
    private GameManager gameManager;
    private bool isHolding = false;
    private float holdTime = 0f;
    public const float requiredHoldTime = .5f; // Required hold time in seconds
    public GameObject inventoryWhole; // Reference to the InventoryWhole game object

    // Add references to the Canvas UI objects
    public GameObject smokeBombUI;
    public GameObject rockUI;

    void Start()
    {
        inventorySystem = InventorySystem.Instance;
        gameManager = FindObjectOfType<GameManager>();
        SwitchAmmoType();
        lineRenderer.positionCount = lineSegmentCount;
        gameManager = GameManager.Instance;
    }

    void Update()
    {
        // Check if InventoryWhole is active
        if (inventoryWhole.activeSelf || gameManager.Frose || gameManager.PlayerFrozen)
        {
            return; // Exit the Update method if InventoryWhole is active
        }

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
            rockUI.SetActive(true);
            smokeBombUI.SetActive(false);
        }
        else if (currentAmmoType == 1)
        {
            currentAmmoPrefab = smokeBombPrefab;
            rockUI.SetActive(false);
            smokeBombUI.SetActive(true);
        }
    }

    void LaunchAmmo()
    {
        Debug.Log("Attempting to launch ammo. Current ammo type: " + currentAmmoType);

        if (currentAmmoType == 1 && !HasSmokeBomb())
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
            UseSmokeBomb();
            Debug.Log("Smoke bomb launched.");
        }
    }

    bool HasSmokeBomb()
    {
        for (int i = 0; i < inventorySystem.Items.Length; i++)
        {
            if (inventorySystem.Items[i] == 8) // 8 represents a smoke bomb
            {
                return true;
            }
        }
        return false;
    }

    void UseSmokeBomb()
    {
        for (int i = 0; i < inventorySystem.Items.Length; i++)
        {
            if (inventorySystem.Items[i] == 8) // 8 represents a smoke bomb
            {
                inventorySystem.Items[i] = 0; // Remove the smoke bomb from the inventory
                inventorySystem.UpdateInventorySlots(); // Update the inventory UI
                break;
            }
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
