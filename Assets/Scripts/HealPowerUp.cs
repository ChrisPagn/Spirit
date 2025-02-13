using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealPowerUp : MonoBehaviour
{
    public int healthPoints;

    public AudioSource audioSource;
    public AudioClip sound;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);

            Debug.LogWarning("passage dans healpowerup");
            if (PlayerHealth.instance.currentHealth < PlayerHealth.instance.maxHealth)
            {
                // rendre de la vie au joueur
                PlayerHealth.instance.HealPlayer(healthPoints);
                // Détruit après le temps du son
                Destroy(gameObject, sound.length);
            }
            
        }
    }
}
