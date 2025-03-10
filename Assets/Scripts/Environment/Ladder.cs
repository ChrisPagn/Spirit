using TMPro; 
using UnityEngine;

/// <summary>
/// G�re l'interaction du joueur avec une �chelle, permettant de monter et descendre.
/// </summary>
public class Ladder : MonoBehaviour
{
    /// <summary>
    /// Indique si le joueur est � port�e de l'�chelle.
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// R�f�rence � l'interface utilisateur d'interaction pour l'�chelle.
    /// </summary>
    private TextMeshProUGUI interactUILadder;

    /// <summary>
    /// R�f�rence au script de mouvement du joueur.
    /// </summary>
    private PlayerMovement playerMovement;

    /// <summary>
    /// Collider en haut de l'�chelle pour g�rer les interactions avec le joueur.
    /// </summary>
    public BoxCollider2D topCollider2D;

    /// <summary>
    /// Initialise les composants n�cessaires au d�marrage.
    /// </summary>
    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        interactUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// Met � jour le comportement de l'�chelle � chaque frame.
    /// </summary>
    void Update()
    {
        if (isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            // Descendre de l'�chelle
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
    /// D�tecte quand le joueur entre dans la zone d'interaction de l'�chelle.
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
    /// D�tecte quand le joueur sort de la zone d'interaction de l'�chelle.
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
