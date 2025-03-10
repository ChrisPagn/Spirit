using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Gère le ramassage d'objets par le joueur.
/// </summary>
public class PickUpItem : MonoBehaviour
{
    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    /// <summary>
    /// Objet à ramasser.
    /// </summary>
    public Item item;

    /// <summary>
    /// Source audio pour jouer les sons lors du ramassage de l'objet.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio à jouer lorsque l'objet est ramassé.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Initialise les composants nécessaires au démarrage.
    /// </summary>
    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI
    }

    /// <summary>
    /// Met à jour le comportement de l'objet à chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            TakeItem();
        }
    }

    /// <summary>
    /// Ramasse l'objet, l'ajoute à l'inventaire, joue un son, et détruit l'objet.
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
    /// Détecte quand le joueur entre dans la zone d'interaction de l'objet.
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
    /// Détecte quand le joueur sort de la zone d'interaction de l'objet.
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
