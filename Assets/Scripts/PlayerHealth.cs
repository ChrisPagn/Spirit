using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script g�re la sant� du joueur, la prise de d�g�ts, l'invincibilit� temporaire apr�s avoir pris des d�g�ts, 
/// et la mise � jour de la barre de vie.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // La sant� maximale du joueur
    public int maxHealth = 100;

    // La sant� actuelle du joueur
    public int currentHealth;

    // D�lai entre les flashs du sprite lors de l'invincibilit� (en secondes)
    public float invincibilityFlashDelay = 0.2f;

    // Temps d'invincibilit� apr�s avoir pris des d�g�ts (en secondes)
    public float invincibilityTimeAfterHit = 3f;

    // Bool�en pour v�rifier si le joueur est actuellement invincible
    public bool isInvincible = false;

    // R�f�rence � la barre de vie du joueur (script HealthBar)
    public HealthBar healthBar;

    // R�f�rence au SpriteRenderer pour rendre le joueur clignotant lorsqu'il est invincible
    public SpriteRenderer graphics;

    // Start est appel� au d�but du jeu
    void Start()
    {
        // Initialisation de la sant� actuelle au maximum
        currentHealth = maxHealth;

        // Initialisation de la barre de vie avec la valeur maximale de sant�
        healthBar.SetMaxHealth(maxHealth);
    }

    // Update est appel� � chaque frame
    void Update()
    {
        // Test de prise de d�g�ts avec la touche H (pour les tests en d�veloppement)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(20); // R�duit la sant� de 20 si on appuie sur H
        }
    }

    /// <summary>
    /// M�thode appel�e lorsque le joueur prend des d�g�ts.
    /// </summary>
    /// <param name="damage">Quantit� de d�g�ts inflig�s au joueur.</param>
    public void TakeDamage(int damage)
    {
        // Si le joueur n'est pas invincible, il peut subir des d�g�ts
        if (!isInvincible)
        {
            // R�duction de la sant�
            currentHealth -= damage;

            // Mise � jour de la barre de vie
            healthBar.SetHealth(currentHealth);

            // Activer l'invincibilit� pour emp�cher d'autres d�g�ts
            isInvincible = true;

            // Lancer les coroutines pour l'animation de flash et la gestion du d�lai d'invincibilit�
            StartCoroutine(InvincibiltyFlash());
            StartCoroutine(HandleInvincibilityDealy());
        }
    }

    /// <summary>
    /// Coroutine qui fait clignoter le sprite du joueur pour indiquer l'invincibilit�.
    /// </summary>
    public IEnumerator InvincibiltyFlash()
    {
        // Tant que le joueur est invincible, alterner la visibilit� du sprite
        while (isInvincible)
        {
            // Rendre le sprite invisible
            graphics.color = new Color(1f, 1f, 1f, 0);
            yield return new WaitForSeconds(invincibilityFlashDelay);

            // Rendre le sprite visible
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    /// <summary>
    /// Coroutine qui g�re la dur�e de l'invincibilit� apr�s avoir pris des d�g�ts.
    /// </summary>
    public IEnumerator HandleInvincibilityDealy()
    {
        // Attendre la fin de la p�riode d'invincibilit�
        yield return new WaitForSeconds(invincibilityTimeAfterHit);

        // D�sactiver l'invincibilit� pour que le joueur puisse � nouveau subir des d�g�ts
        isInvincible = false;
    }
}
