using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// G�re le ramassage d'objets par le joueur.
/// </summary>
public class PickUpItem : MonoBehaviour
{
    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    /// <summary>
    /// Objet � ramasser.
    /// </summary>
    public Item item;

    /// <summary>
    /// Source audio pour jouer les sons lors du ramassage de l'objet.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio � jouer lorsque l'objet est ramass�.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Initialise les composants n�cessaires au d�marrage.
    /// </summary>
    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // R�cup�ration du TextMeshProUGUI
    }

    /// <summary>
    /// Met � jour le comportement de l'objet � chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            TakeItem();
        }
    }

    /// <summary>
    /// Ramasse l'objet, l'ajoute � l'inventaire, joue un son, et d�truit l'objet.
    /// </summary>
    public void TakeItem()
    {
        Inventory.instance.contentItems.Add(item);
        Inventory.instance.UpdateInventoryUI();
        audioSource.PlayOneShot(sound);
        InteractUILadder.enabled = false;
        Destroy(gameObject);
    }

    /// <summary>
    /// D�tecte quand le joueur entre dans la zone d'interaction de l'objet.
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
    /// D�tecte quand le joueur sort de la zone d'interaction de l'objet.
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
