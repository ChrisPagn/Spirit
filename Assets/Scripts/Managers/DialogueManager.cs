using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI npcNameText;

    public TextMeshProUGUI dialogueText;

    public Animator animator;
    private TextMeshProUGUI interactUI;

    private Queue<string> sentences;

    /// <summary>
    /// singleton de l'instance
    /// </summary>
    public static DialogueManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance DialogueManager dans la scène");
            return;
        }
        instance = this;
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        animator.SetBool("isOpen", true);

        npcNameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSetence();
    }

    public void DisplayNextSetence()
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string setence = sentences.Dequeue();
        StopAllCoroutines();
        interactUI.enabled = false;
        StartCoroutine(TypeSentence(setence));  
    }

    IEnumerator TypeSentence (string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
