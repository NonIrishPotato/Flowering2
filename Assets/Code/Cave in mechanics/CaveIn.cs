using System.Collections;
using UnityEngine;

public class DynamicCaveIn : MonoBehaviour
{
    public GameObject rockPrefab;
    public Transform[] spawnPoints; // Places rocks fall from
    public float fallInterval = 1.5f;
    public float startDelay = 1f;
    public AudioClip caveInSound;

    private bool caveInActive = false;
    private bool playerSafe = false;
    private AudioSource audioSource;

    void Start()
    {
        if (caveInSound != null)
        {
            audioSource = gameObject.AddComponent<AudioSource>();
            audioSource.clip = caveInSound;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if (!caveInActive && other.CompareTag("Player"))
        {
            caveInActive = true;
            if (audioSource) audioSource.Play();
            StartCoroutine(SpawnFallingRocks());
        }
    }

    IEnumerator SpawnFallingRocks()
    {
        yield return new WaitForSeconds(startDelay);

        while (!playerSafe)
        {
            SpawnRock();
            yield return new WaitForSeconds(fallInterval);
        }
    }

    void SpawnRock()
    {
        Transform spawnPoint = spawnPoints[Random.Range(0, spawnPoints.Length)];
        Instantiate(rockPrefab, spawnPoint.position, Quaternion.identity);
    }

    public void StopCaveIn()
    {
        playerSafe = true;
    }
}
