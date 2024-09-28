using UnityEngine;

/// <summary>
/// Ce script gère la collecte d'un objet (comme des pièces ou autres items) par le joueur.
/// Lorsqu'un joueur entre en collision avec l'objet, celui-ci est ajouté à l'inventaire puis détruit.
/// </summary>
public class PickUpObject : MonoBehaviour
{
    // Start est appelé avant la première image (frame) du jeu.
    void Start()
    {
        // Ce bloc est laissé vide car aucune initialisation n'est nécessaire dans ce cas.
    }

    // Update est appelé une fois par frame (image).
    void Update()
    {
        // Ce bloc est également laissé vide, car aucune mise à jour continue n'est nécessaire pour cet objet.
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre dans le collider de cet objet (qui est marqué comme "trigger").
    /// Si c'est le joueur qui entre dans la zone, l'objet est ramassé et détruit.
    /// </summary>
    /// <param name="collision">Le Collider2D de l'objet qui entre dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'objet entrant en collision est le joueur
        if (collision.CompareTag("Player"))
        {
            // Ajoute une pièce (ou un autre objet) à l'inventaire via le Singleton Inventory
            Inventory.Instance.AddCoins(1);

            // Détruit l'objet collecté du jeu après la collecte
            Destroy(gameObject);
        }
    }
}
