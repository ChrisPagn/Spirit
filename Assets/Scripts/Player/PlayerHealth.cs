using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script gère la santé du joueur, la prise de dégâts, l'invincibilité temporaire après avoir pris des dégâts,
/// et la mise à jour de la barre de vie.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// La santé maximale du joueur.
    /// </summary>
    public int maxHealth = 100;

    /// <summary>
    /// La santé actuelle du joueur.
    /// </summary>
    public int currentHealth;

    /// <summary>
    /// Délai entre les flashs du sprite lors de l'invincibilité (en secondes).
    /// </summary>
    public float invincibilityFlashDelay = 0.2f;

    /// <summary>
    /// Temps d'invincibilité après avoir pris des dégâts (en secondes).
    /// </summary>
    public float invincibilityTimeAfterHit = 3f;

    /// <summary>
    /// Indique si le joueur est actuellement invincible.
    /// </summary>
    public bool isInvincible = false;

    /// <summary>
    /// Référence à la barre de vie du joueur.
    /// </summary>
    public HealthBar healthBar;

    /// <summary>
    /// Référence au SpriteRenderer pour rendre le joueur clignotant lorsqu'il est invincible.
    /// </summary>
    public SpriteRenderer graphics;

    /// <summary>
    /// Instance statique pour gérer l'accès global à PlayerHealth.
    /// </summary>
    public static PlayerHealth instance;

    /// <summary>
    /// Source audio du joueur.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Son joué lorsque le joueur prend des dégâts.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// S'exécute avant le Start, initialise l'instance unique de PlayerHealth.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance PlayerHealth dans la scène");
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Initialise les valeurs de santé et met à jour la barre de vie au démarrage du jeu.
    /// </summary>
    void Start()
    {
        currentHealth = maxHealth;
        if (healthBar != null)
        {
            healthBar.SetMaxHealth(maxHealth);
        }
        else
        {
            Debug.LogError("La référence à la barre de vie (HealthBar) est manquante.");
        }
    }

    /// <summary>
    /// Vérifie les entrées utilisateur chaque frame, notamment pour tester la prise de dégâts.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(40);
        }
    }

    /// <summary>
    /// Ajoute de la santé au joueur.
    /// </summary>
    /// <param name="amount">Quantité de points de vie à ajouter.</param>
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    /// <summary>
    /// Réduit la santé du joueur en fonction des dégâts subis.
    /// </summary>
    /// <param name="damage">Quantité de dégâts infligés au joueur.</param>
    public void TakeDamage(int damage)
    {
        if (!isInvincible)
        {
            audioSource.PlayOneShot(sound);
            currentHealth -= damage;
            if (healthBar != null)
            {
                healthBar.SetHealth(currentHealth);
            }
            if (currentHealth <= 0)
            {
                Die();
                return;
            }
            isInvincible = true;
            StartCoroutine(InvincibiltyFlash());
            StartCoroutine(HandleInvincibilityDealy());
        }
    }

    /// <summary>
    /// Gère la mort du joueur.
    /// </summary>
    public void Die()
    {
        Debug.Log("Le joueur est éliminé");
        if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.enabled = false;
            PlayerMovement.instance.animator.SetTrigger("Die");
            PlayerMovement.instance.rigidbody.bodyType = RigidbodyType2D.Kinematic;
            PlayerMovement.instance.rigidbody.velocity = Vector2.zero;
            PlayerMovement.instance.playerCollider.enabled = false;
        }
        else
        {
            Debug.LogError("L'instance de PlayerMovement est null.");
        }
        if (GameOverManager.instance != null)
        {
            GameOverManager.instance.OnPlayerDeath();
        }
        else
        {
            Debug.LogError("L'instance de GameOverManager est null.");
        }
    }

    /// <summary>
    /// Réinitialise la position et la santé du joueur après une mort.
    /// </summary>
    public void Respawn()
    {
        Debug.Log("Le joueur est replacé au début");
        if (PlayerMovement.instance != null)
        {
            PlayerMovement.instance.enabled = true;
            PlayerMovement.instance.animator.SetTrigger("Respawn");
            PlayerMovement.instance.rigidbody.bodyType = RigidbodyType2D.Dynamic;
            PlayerMovement.instance.playerCollider.enabled = true;
            currentHealth = maxHealth;
            healthBar.SetHealth(currentHealth);
        }
        else
        {
            Debug.LogError("L'instance de PlayerMovement est null.");
        }
    }

    /// <summary>
    /// Coroutine qui fait clignoter le sprite du joueur pour signaler l'invincibilité.
    /// </summary>
    public IEnumerator InvincibiltyFlash()
    {
        while (isInvincible)
        {
            graphics.color = new Color(1f, 1f, 1f, 0);
            yield return new WaitForSeconds(invincibilityFlashDelay);
            graphics.color = new Color(1f, 1f, 1f, 1f);
            yield return new WaitForSeconds(invincibilityFlashDelay);
        }
    }

    /// <summary>
    /// Coroutine qui gère la durée de l'invincibilité après avoir pris des dégâts.
    /// </summary>
    public IEnumerator HandleInvincibilityDealy()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvincible = false;
    }
}