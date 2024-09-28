using UnityEngine;

/// <summary>
/// Classe qui gère la destruction d'un objet lorsqu'un joueur entre en collision avec une zone spécifique.
/// </summary>
public class WeakSpot : MonoBehaviour
{
    /// <summary>
    /// Objet à détruire lorsqu'un joueur entre en collision avec la zone.
    /// </summary>
    public GameObject objectToDestroy;

    /// <summary>
    /// Méthode appelée lorsque le joueur entre en collision avec la zone.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si le collider est le joueur
        if (collision.CompareTag("Player"))
        {
            // Détruit l'objet spécifié
            Destroy(objectToDestroy);
        }
    }
}