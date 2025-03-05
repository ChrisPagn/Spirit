using UnityEngine;

/// <summary>
/// Ce script g�re la collecte d'un objet (comme des pi�ces ou autres items) par le joueur.
/// Lorsqu'un joueur entre en collision avec l'objet, celui-ci est ajout� � l'inventaire puis d�truit.
/// </summary>
public class PickUpCoin : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;

   

    /// <summary>
    /// M�thode appel�e lorsqu'un autre objet entre dans le collider de cet objet (qui est marqu� comme "trigger").
    /// Si c'est le joueur qui entre dans la zone, l'objet est ramass� et d�truit.
    /// </summary>
    /// <param name="collision">Le Collider2D de l'objet qui entre dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            Inventory.instance.AddCoins(1);
            CurrentSceneManager.instance.coinsPickedUpInThisSceneCount++;

            // D�sactive le collider et le renderer pour �viter les interactions visuelles
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // D�truit apr�s le temps du son
            Destroy(gameObject, sound.length);
        }
    }
}
