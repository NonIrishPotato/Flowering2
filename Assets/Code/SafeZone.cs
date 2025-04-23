using UnityEngine;

public class SafeZone : MonoBehaviour
{
    public DynamicCaveIn caveInController;

    void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player"))
        {
            caveInController.StopCaveIn();
        }
    }
}
