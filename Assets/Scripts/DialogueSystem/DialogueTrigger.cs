using System;
using TMPro;
using UnityEngine;

/// <summary>
/// D�clenche un dialogue lorsque le joueur interagit avec un objet dans le jeu.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    /// <summary>
    /// Dialogue � d�clencher lorsque le joueur interagit avec l'objet.
    /// </summary>
    public Dialogue dialogue;

    /// <summary>
    /// Indique si le joueur est � port�e d'interaction avec l'objet.
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// R�f�rence � l'interface utilisateur d'interaction.
    /// </summary>
    private TextMeshProUGUI interactUI;

    /// <summary>
    /// Initialise les composants n�cessaires au d�marrage.
    /// </summary>
    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Met � jour le comportement de l'objet � chaque frame.
    /// </summary>
    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    /// <summary>
    /// D�clenche le dialogue associ� � l'objet.
    /// </summary>
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    /// <summary>
    /// D�tecte quand le joueur entre dans la zone d'interaction de l'objet.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactUI.enabled = true;
        }
    }

    /// <summary>
    /// D�tecte quand le joueur sort de la zone d'interaction de l'objet.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
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
