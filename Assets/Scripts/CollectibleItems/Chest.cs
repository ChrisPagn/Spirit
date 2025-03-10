using TMPro;
using UnityEngine;

/// <summary>
/// Gestion du coffre � bonus.
/// </summary>
public class Chest : MonoBehaviour
{
    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    /// <summary>
    /// Animator pour g�rer les animations du coffre.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Source audio pour jouer les sons du coffre.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio � jouer lors de l'ouverture du coffre.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Nombre de pi�ces � ajouter � l'inventaire lors de l'ouverture du coffre.
    /// </summary>
    public int coinsToAdd;

    /// <summary>
    /// Initialise les composants n�cessaires au d�marrage.
    /// </summary>
    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // R�cup�ration du TextMeshProUGUI
    }

    /// <summary>
    /// Met � jour le comportement du coffre � chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            OpenChest();
        }
    }

    /// <summary>
    /// Ouvre le coffre, ajoute des pi�ces � l'inventaire, joue un son et d�sactive les collisions.
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
    /// D�tecte quand le joueur entre dans la zone d'interaction du coffre.
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
    /// D�tecte quand le joueur sort de la zone d'interaction du coffre.
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
