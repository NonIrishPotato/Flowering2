using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class NPC : MonoBehaviour
{
    private GameObject dialogue;
    bool isPlayerInsideNPC;
    public KeyCode key;
    public string WhichDialogueBox;

    void Start()
    {
        dialogue = GameObject.Find(WhichDialogueBox);
        Debug.Log("NPC Script found Dialogue Box");
    }

    void Update()
    {
        if(isPlayerInsideNPC && Input.GetKeyDown(key))
        {
            dialogue.SetActive(true);
            dialogue.GetComponent<Image>().enabled = true;
            dialogue.GetComponent<Dialogue>().enabled = true;
            dialogue.GetComponentInChildren<TextMeshProUGUI>().enabled = true;
            Debug.Log("Player has triggered NPC dialogue");
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        if(other.gameObject.tag == "Player")
        {
            isPlayerInsideNPC = true;
            Debug.Log("Player is inside NPC");
            gameObject.GetComponentInChildren<TextMeshPro>().enabled = true;
        }
    }
    void OnTriggerExit2D(Collider2D other)
    {
        if (other.gameObject.tag == "Player")
        {
            isPlayerInsideNPC = false;
            Debug.Log("Player is outside NPC");
            gameObject.GetComponentInChildren<TextMeshPro>().enabled = false;
        }
    }
}