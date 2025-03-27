using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère l'affichage et le déroulement des dialogues dans le jeu.
/// </summary>
public class DialogueManager : MonoBehaviour
{

    /// <summary>
    /// Texte affichant le nom du PNJ qui parle.
    /// </summary>
    public TextMeshProUGUI npcNameText;

    /// <summary>
    /// Texte affichant les phrases du dialogue.
    /// </summary>
    public TextMeshProUGUI dialogueText;

    /// <summary>
    /// Animator pour gérer les animations de la boîte de dialogue.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Interface utilisateur d'interaction pour indiquer au joueur qu'il peut interagir.
    /// </summary>
    private TextMeshProUGUI interactUI;

    /// <summary>
    /// File contenant les phrases du dialogue à afficher.
    /// Utilisé pour stocker les phrases du dialogue dans l'ordre où elles doivent être affichées.
    /// </summary>
    private Queue<string> sentences;

    /// <summary>
    /// Instance unique de DialogueManager (Singleton).
    /// </summary>
    public static DialogueManager instance;

    /// <summary>
    /// Initialise l'instance unique de DialogueManager et prépare la file de phrases.
    /// </summary>
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

        //S'assurer que le message : interaction est désactif lors du démarrage :
        Debug.Log("je change de scene");

    }

    /// <summary>
    /// Démarre un dialogue en affichant le nom du PNJ et en initialisant la file de phrases.
    /// </summary>
    /// <param name="dialogue">Le dialogue à afficher.</param>
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

    /// <summary>
    /// Affiche la phrase suivante du dialogue.
    /// </summary>
    public void DisplayNextSetence()
    {
        if (sentences.Count == 0)
        {
            EndDialogue();
            return;
        }
        string sentence = sentences.Dequeue();
        StopAllCoroutines();
        interactUI.enabled = false;
        StartCoroutine(TypeSentence(sentence));
    }

    /// <summary>
    /// Coroutine pour afficher une phrase lettre par lettre
    /// avec un délai, créant un effet de machine à écrire.
    /// </summary>
    /// <param name="sentence">La phrase à afficher.</param>
    IEnumerator TypeSentence(string sentence)
    {
        dialogueText.text = "";
        foreach (char letter in sentence.ToCharArray())
        {
            dialogueText.text += letter;
            yield return new WaitForSeconds(0.1f);
        }
    }

    /// <summary>
    /// Termine le dialogue et ferme la boîte de dialogue.
    /// </summary>
    public void EndDialogue()
    {
        animator.SetBool("isOpen", false);
    }
}
