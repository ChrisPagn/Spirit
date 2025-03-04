using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour
{
    public TextMeshProUGUI nameText;

    public TextMeshProUGUI dialogueText;

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

        sentences = new Queue<string>();
    }

    public void StartDialogue(Dialogue dialogue)
    {
        nameText.text = dialogue.name;
        sentences.Clear();

        foreach (string sentence in dialogue.sentences)
        {
            sentences.Enqueue(sentence);
        }

        DisplayNextSetence();
    }

    private void DisplayNextSetence()
    {
        if (sentences.Count == 0) 
        {
            EndDialogue();
            return;
        }
        string setence = sentences.Dequeue();
        dialogueText.text = setence;
    }

    private void EndDialogue()
    {
        throw new NotImplementedException();
    }
}
