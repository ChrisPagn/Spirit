using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère le comportement d'un power-up de soin qui restaure des points de vie au joueur.
/// </summary>
public class HealPowerUp : MonoBehaviour
{
    /// <summary>
    /// Nombre de points de vie à restaurer lorsque le joueur ramasse le power-up.
    /// </summary>
    public int healthPoints;

    /// <summary>
    /// Source audio pour jouer les sons du power-up.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio à jouer lorsque le joueur ramasse le power-up.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Détecte quand le joueur entre en collision avec le power-up.
    /// </summary>
    /// <param name="collision">Le collider du joueur.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);

            Debug.LogWarning("Passage dans HealPowerUp");
            if (PlayerHealth.playerhealth.currentHealth < PlayerHealth.playerhealth.maxHealth)
            {
                // Rendre de la vie au joueur
                PlayerHealth.playerhealth.HealPlayer(healthPoints);
                // Détruit le power-up après la fin du son
                Destroy(gameObject, sound.length);
            }
        }
    }
}
