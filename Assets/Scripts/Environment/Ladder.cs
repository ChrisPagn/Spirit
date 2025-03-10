using TMPro; 
using UnityEngine;

/// <summary>
/// Gère l'interaction du joueur avec une échelle, permettant de monter et descendre.
/// </summary>
public class Ladder : MonoBehaviour
{
    /// <summary>
    /// Indique si le joueur est à portée de l'échelle.
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// Référence à l'interface utilisateur d'interaction pour l'échelle.
    /// </summary>
    private TextMeshProUGUI interactUILadder;

    /// <summary>
    /// Référence au script de mouvement du joueur.
    /// </summary>
    private PlayerMovement playerMovement;

    /// <summary>
    /// Collider en haut de l'échelle pour gérer les interactions avec le joueur.
    /// </summary>
    public BoxCollider2D topCollider2D;

    /// <summary>
    /// Initialise les composants nécessaires au démarrage.
    /// </summary>
    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        interactUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Met à jour le comportement de l'échelle à chaque frame.
    /// </summary>
    void Update()
    {
        if (isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            // Descendre de l'échelle
            playerMovement.isClimbing = false;
            topCollider2D.isTrigger = false;
            return;
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = true;
            topCollider2D.isTrigger = true;
        }
    }

    /// <summary>
    /// Détecte quand le joueur entre dans la zone d'interaction de l'échelle.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUILadder.enabled = true;
            isInRange = true;
        }
    }

    /// <summary>
    /// Détecte quand le joueur sort de la zone d'interaction de l'échelle.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            if (playerMovement.isGrounded)
            {
                playerMovement.isClimbing = false;
                topCollider2D.isTrigger = false;
                interactUILadder.enabled = false;
            }
        }
    }
}
