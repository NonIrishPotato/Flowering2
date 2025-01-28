using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class SoundHazard : MonoBehaviour
{
    public GameManager gameManager;
    public bool smallHazard = true;
    public bool mediumHazard = false;
    public bool largeHazard = false;

    public float timeAffected = 1f;

    // Start is called before the first frame update
    void Start()
    {
        gameManager = GameManager.Instance;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if(smallHazard)
            {
                StartCoroutine(smallHazardHit());
            }
            else if (mediumHazard)
            {
                StartCoroutine(mediumHazardHit());
            }
            else if (largeHazard)
            {
                StartCoroutine(largeHazardHit());
            }
        }
    }

    IEnumerator smallHazardHit()
    {
        gameManager.smallHazardHit = true;
        yield return new WaitForSeconds(timeAffected);
        gameManager.smallHazardHit = false;
    }

    IEnumerator mediumHazardHit()
    {
        gameManager.mediumHazardHit = true;
        yield return new WaitForSeconds(timeAffected);
        gameManager.mediumHazardHit = false;
    }

    IEnumerator largeHazardHit()
    {
        gameManager.largeHazardHit = true;
        yield return new WaitForSeconds(timeAffected);
        gameManager.largeHazardHit = false;
    }
}
