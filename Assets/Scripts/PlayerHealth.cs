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

    public static PlayerHealth instance;

    public AudioSource audioSource;
    public AudioClip sound;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance PlayerHealth dans la sc�ne");
            return;
        }

        instance = this;
    }

    // Start est appel� au d�but du jeu
    void Start()
    {
        // Initialisation de la sant� actuelle au maximum
        currentHealth = maxHealth;

        // Initialisation de la barre de vie avec la valeur maximale de sant�
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogError("La r�f�rence � la barre de vie (HealthBar) est manquante.");
        }
    }

    // Update est appel� � chaque frame
    void Update()
    {
        // Test de prise de d�g�ts avec la touche H (pour les tests en d�veloppement)
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(110); // R�duit la sant� de X si on appuie sur H
        }
    }

    /// <summary>
    /// M�thode appel�e lorsque le joueur reprend de la vie.
    /// </summary>
    /// <param name="amount">Quantit� de r�cup�ration de vie au joueur.</param>
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
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
            audioSource.PlayOneShot(sound);

            // R�duction de la sant�
            currentHealth -= damage;

            // Mise � jour de la barre de vie
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }

            // V�rifie si le joueur est toujours vivant
            if (currentHealth <= 0)
            {
                Die();
                return;
            }

            // Activer l'invincibilit� pour emp�cher d'autres d�g�ts
            isInvincible = true;

            // Lancer les coroutines pour l'animation de flash et la gestion du d�lai d'invincibilit�
            StartCoroutine(InvincibiltyFlash());
            StartCoroutine(HandleInvincibilityDealy());
        }
    }

    /// <summary>
    /// M�thode pour la mort du joueur
    /// </summary>
    public void Die()
    {
        Debug.Log("Le joueur est �limin�");

        // V�rifie que l'instance de PlayerMovement n'est pas null
        if (PlayerMovement.instance != null)
        {
            // Bloquer les mouvements du personnage (passage de l'instance singleton � false)
            PlayerMovement.instance.enabled = false;

            // Jouer l'animation d'�limination en passant par le singleton => animation die
            PlayerMovement.instance.animator.SetTrigger("Die");

            // Emp�cher les interactions physiques avec les autres �l�ments de la sc�ne
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
        Debug.Log("Le joueur est replac� au d�but");

        // V�rifie que l'instance de PlayerMovement n'est pas null
        if (PlayerMovement.instance != null)
        {
            // Activer les mouvements du personnage (passage de l'instance singleton � true)
            PlayerMovement.instance.enabled = true;

            // Jouer l'animation d'�limination en passant par le singleton => animation die
            PlayerMovement.instance.animator.SetTrigger("Respawn");

            // Emp�cher les interactions physiques avec les autres �l�ments de la sc�ne
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
