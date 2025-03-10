using TMPro;
using UnityEngine;

/// <summary>
/// D�tecte l'entr�e et la sortie du joueur dans la zone d'un marchand, permettant d'afficher l'interface d'interaction et d'ouvrir ou fermer la boutique.
/// </summary>
public class ShopTrigger : MonoBehaviour
{
    /// <summary>
    /// Indique si le joueur est dans la zone d'interaction du marchand.
    /// </summary>
    private bool isInRange;

    /// <summary>
    /// R�f�rence � l'�l�ment UI indiquant la possibilit� d'interagir.
    /// </summary>
    private TextMeshProUGUI interactUI;

    /// <summary>
    /// Nom du marchand.
    /// </summary>
    public string npcName;

    /// <summary>
    /// Liste des objets que le marchand peut vendre.
    /// </summary>
    public Item[] itemsToShell;

    /// <summary>
    /// Initialise la r�f�rence � l'�l�ment UI d'interaction lors du chargement du script.
    /// </summary>
    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    /// <summary>
    /// V�rifie si le joueur est dans la zone et appuie sur la touche d'interaction pour ouvrir la boutique.
    /// </summary>
    private void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShopManager.instance.OpenShop(itemsToShell, npcName);
        }
    }

    /// <summary>
    /// D�tecte l'entr�e du joueur dans la zone du marchand et affiche l'UI d'interaction.
    /// </summary>
    /// <param name="collision">Collider entrant dans la zone.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactUI.enabled = true;
        }
    }

    /// <summary>
    /// D�tecte la sortie du joueur de la zone du marchand, masque l'UI d'interaction et ferme la boutique.
    /// </summary>
    /// <param name="collision">Collider sortant de la zone.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            interactUI.enabled = false;
            ShopManager.instance.CloseShop();
        }
    }
}
