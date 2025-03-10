using TMPro;
using UnityEngine;

/// <summary>
/// Gestion du coffre à bonus.
/// </summary>
public class Chest : MonoBehaviour
{
    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    /// <summary>
    /// Animator pour gérer les animations du coffre.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Source audio pour jouer les sons du coffre.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio à jouer lors de l'ouverture du coffre.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Nombre de pièces à ajouter à l'inventaire lors de l'ouverture du coffre.
    /// </summary>
    public int coinsToAdd;

    /// <summary>
    /// Initialise les composants nécessaires au démarrage.
    /// </summary>
    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI
    }

    /// <summary>
    /// Met à jour le comportement du coffre à chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            OpenChest();
        }
    }

    /// <summary>
    /// Ouvre le coffre, ajoute des pièces à l'inventaire, joue un son et désactive les collisions.
    /// </summary>
    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        Inventory.instance.AddCoins(coinsToAdd);
        audioSource.PlayOneShot(sound);
        GetComponent<BoxCollider2D>().enabled = false;
        InteractUILadder.enabled = false;
    }

    /// <summary>
    /// Détecte quand le joueur entre dans la zone d'interaction du coffre.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractUILadder.enabled = true;
            IsInRange = true;
        }
    }

    /// <summary>
    /// Détecte quand le joueur sort de la zone d'interaction du coffre.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractUILadder.enabled = false;
            IsInRange = false;
        }
    }
}
