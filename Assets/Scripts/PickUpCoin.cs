using UnityEngine;

/// <summary>
/// Ce script gère la collecte d'un objet (comme des pièces ou autres items) par le joueur.
/// Lorsqu'un joueur entre en collision avec l'objet, celui-ci est ajouté à l'inventaire puis détruit.
/// </summary>
public class PickUpCoin : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;

   

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre dans le collider de cet objet (qui est marqué comme "trigger").
    /// Si c'est le joueur qui entre dans la zone, l'objet est ramassé et détruit.
    /// </summary>
    /// <param name="collision">Le Collider2D de l'objet qui entre dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            Inventory.instance.AddCoins(1);
            CurrentSceneManager.instance.coinsPickedUpInThisSceneCount++;

            // Désactive le collider et le renderer pour éviter les interactions visuelles
            GetComponent<Collider2D>().enabled = false;
            GetComponent<SpriteRenderer>().enabled = false;

            // Détruit après le temps du son
            Destroy(gameObject, sound.length);
        }
    }
}
