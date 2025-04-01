using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re le comportement d'un power-up de soin qui restaure des points de vie au joueur.
/// </summary>
public class HealPowerUp : MonoBehaviour
{
    /// <summary>
    /// Nombre de points de vie � restaurer lorsque le joueur ramasse le power-up.
    /// </summary>
    public int healthPoints;

    /// <summary>
    /// Source audio pour jouer les sons du power-up.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio � jouer lorsque le joueur ramasse le power-up.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// D�tecte quand le joueur entre en collision avec le power-up.
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
                // D�truit le power-up apr�s la fin du son
                Destroy(gameObject, sound.length);
            }
        }
    }
}
