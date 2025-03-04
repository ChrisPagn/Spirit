using System;
using TMPro;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    public Dialogue dialogue;

    private bool isInRange;

    private TextMeshProUGUI interactUI;


    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); 
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E)) 
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
       DialogueManager.instance.StartDialogue(dialogue);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactUI.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            interactUI.enabled = false;
            DialogueManager.instance.EndDialogue();
        }
    }
}
