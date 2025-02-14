using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script gère la santé du joueur, la prise de dégâts, l'invincibilité temporaire après avoir pris des dégâts,
/// et la mise à jour de la barre de vie.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    // La santé maximale du joueur
    public int maxHealth = 100;

    // La santé actuelle du joueur
    public int currentHealth;

    // Délai entre les flashs du sprite lors de l'invincibilité (en secondes)
    public float invincibilityFlashDelay = 0.2f;

    // Temps d'invincibilité après avoir pris des dégâts (en secondes)
    public float invincibilityTimeAfterHit = 3f;

    // Booléen pour vérifier si le joueur est actuellement invincible
    public bool isInvincible = false;

    // Référence à la barre de vie du joueur (script HealthBar)
    public HealthBar healthBar;

    // Référence au SpriteRenderer pour rendre le joueur clignotant lorsqu'il est invincible
    public SpriteRenderer graphics;

    public static PlayerHealth instance;

    public AudioSource audioSource;
    public AudioClip sound;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance PlayerHealth dans la scène");
            return;
        }

        instance = this;
    }

    // Start est appelé au début du jeu
    void Start()
    {
        // Initialisation de la santé actuelle au maximum
        currentHealth = maxHealth;

        // Initialisation de la barre de vie avec la valeur maximale de santé
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogError("La référence à la barre de vie (HealthBar) est manquante.");
        }
    }

    // Update est appelé à chaque frame
    void Update()
    {
        // Test de prise de dégâts avec la touche H (pour les tests en développement)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(110); // Réduit la santé de X si on appuie sur H
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le joueur reprend de la vie.
    /// </summary>
    /// <param name="amount">Quantité de récupération de vie au joueur.</param>
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    /// <summary>
    /// Méthode appelée lorsque le joueur prend des dégâts.
    /// </summary>
    /// <param name="damage">Quantité de dégâts infligés au joueur.</param>
    public void TakeDamage(int damage)
    {
        // Si le joueur n'est pas invincible, il peut subir des dégâts
        if (!isInvincible)
        {
            audioSource.PlayOneShot(sound);

            // Réduction de la santé
            currentHealth -= damage;

            // Mise à jour de la barre de vie
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            // Vérifie si le joueur est toujours vivant
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            // Activer l'invincibilité pour empêcher d'autres dégâts
            isInvincible = true;

            // Lancer les coroutines pour l'animation de flash et la gestion du délai d'invincibilité
            StartCoroutine(InvincibiltyFlash());
            StartCoroutine(HandleInvincibilityDealy());
        }
    }

    /// <summary>
    /// Méthode pour la mort du joueur
    /// </summary>
    public void Die()
    {
        Debug.Log("Le joueur est éliminé");

        // Vérifie que l'instance de PlayerMovement n'est pas null
        if (PlayerMovement.instance != null)
        {
            // Bloquer les mouvements du personnage (passage de l'instance singleton à false)
            PlayerMovement.instance.enabled = false;

            // Jouer l'animation d'élimination en passant par le singleton => animation die
            PlayerMovement.instance.animator.SetTrigger("Die");

            // Empêcher les interactions physiques avec les autres éléments de la scène
            PlayerMovement.instance.rigidbody.bodyType = RigidbodyType2D.Kinematic;
            PlayerMovement.instance.rigidbody.velocity = Vector2.zero;
            PlayerMovement.instance.playerCollider.enabled = false;
        }
        else
        {
            Debug.LogError("L'instance de PlayerMovement est null.");
        }

        // Affichage du menu gameOver
        if (GameOverManager.instance != null)
        {
            GameOverManager.instance.OnPlayerDeath();
        }
        else
        {
            Debug.LogError("L'instance de GameOverManager est null.");
        }
    }


    public void Respawn()
    {
        Debug.Log("Le joueur est replacé au début");

        // Vérifie que l'instance de PlayerMovement n'est pas null
        if (PlayerMovement.instance != null)
        {
            // Activer les mouvements du personnage (passage de l'instance singleton à true)
            PlayerMovement.instance.enabled = true;

            // Jouer l'animation d'élimination en passant par le singleton => animation die
            PlayerMovement.instance.animator.SetTrigger("Respawn");

            // Empêcher les interactions physiques avec les autres éléments de la scène
            PlayerMovement.instance.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            PlayerMovement.instance.playerCollider.enabled = true;
            // chargement de la vie du joueur
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogError("L'instance de PlayerMovement est null.");
        }

        
    }

    /// <summary>
    /// Coroutine qui fait clignoter le sprite du joueur pour indiquer l'invincibilité.
    /// </summary>
    public IEnumerator InvincibiltyFlash()
    {
        // Tant que le joueur est invincible, alterner la visibilité du sprite
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
    /// Coroutine qui gère la durée de l'invincibilité après avoir pris des dégâts.
    /// </summary>
    public IEnumerator HandleInvincibilityDealy()
    {
        // Attendre la fin de la période d'invincibilité
        yield return new WaitForSeconds(invincibilityTimeAfterHit);

        // Désactiver l'invincibilité pour que le joueur puisse à nouveau subir des dégâts
        isInvincible = false;
    }
}
