using UnityEngine;

/// <summary>
/// Classe qui g�re la destruction d'un objet lorsqu'un joueur entre en collision avec une zone sp�cifique.
/// </summary>
public class WeakSpot : MonoBehaviour
{
    /// <summary>
    /// Objet � d�truire lorsqu'un joueur entre en collision avec la zone.
    /// </summary>
    public GameObject objectToDestroy;

    /// <summary>
    /// M�thode appel�e lorsque le joueur entre en collision avec la zone.
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si le collider est le joueur
        if (collision.CompareTag("Player"))
        {
            // D�truit l'objet sp�cifi�
            Destroy(objectToDestroy);
        }
    }
}