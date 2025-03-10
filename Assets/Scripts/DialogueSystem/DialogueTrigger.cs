using System;
using TMPro;
using UnityEngine;

/// <summary>
/// Déclenche un dialogue lorsque le joueur interagit avec un objet dans le jeu.
/// </summary>
public class DialogueTrigger : MonoBehaviour
{
    /// <summary>
    /// Dialogue à déclencher lorsque le joueur interagit avec l'objet.
    /// </summary>
    public Dialogue dialogue;

    /// <summary>
    /// Indique si le joueur est à portée d'interaction avec l'objet.
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// Référence à l'interface utilisateur d'interaction.
    /// </summary>
    private TextMeshProUGUI interactUI;

    /// <summary>
    /// Initialise les composants nécessaires au démarrage.
    /// </summary>
    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Met à jour le comportement de l'objet à chaque frame.
    /// </summary>
    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    /// <summary>
    /// Déclenche le dialogue associé à l'objet.
    /// </summary>
    public void TriggerDialogue()
    {
        DialogueManager.instance.StartDialogue(dialogue);
    }

    /// <summary>
    /// Détecte quand le joueur entre dans la zone d'interaction de l'objet.
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
    /// Détecte quand le joueur sort de la zone d'interaction de l'objet.
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
