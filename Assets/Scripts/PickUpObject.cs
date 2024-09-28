using UnityEngine;

/// <summary>
/// Ce script g�re la collecte d'un objet (comme des pi�ces ou autres items) par le joueur.
/// Lorsqu'un joueur entre en collision avec l'objet, celui-ci est ajout� � l'inventaire puis d�truit.
/// </summary>
public class PickUpObject : MonoBehaviour
{
    // Start est appel� avant la premi�re image (frame) du jeu.
    void Start()
    {
        // Ce bloc est laiss� vide car aucune initialisation n'est n�cessaire dans ce cas.
    }

    // Update est appel� une fois par frame (image).
    void Update()
    {
        // Ce bloc est �galement laiss� vide, car aucune mise � jour continue n'est n�cessaire pour cet objet.
    }

    /// <summary>
    /// M�thode appel�e lorsqu'un autre objet entre dans le collider de cet objet (qui est marqu� comme "trigger").
    /// Si c'est le joueur qui entre dans la zone, l'objet est ramass� et d�truit.
    /// </summary>
    /// <param name="collision">Le Collider2D de l'objet qui entre dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'objet entrant en collision est le joueur
        if (collision.CompareTag("Player"))
        {
            // Ajoute une pi�ce (ou un autre objet) � l'inventaire via le Singleton Inventory
            Inventory.Instance.AddCoins(1);

            // D�truit l'objet collect� du jeu apr�s la collecte
            Destroy(gameObject);
        }
    }
}
