using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script g�re la sant� du joueur, la prise de d�g�ts, l'invincibilit� temporaire apr�s avoir pris des d�g�ts,
/// et la mise � jour de la barre de vie.
/// </summary>
public class PlayerHealth : MonoBehaviour
{
    /// <summary>
    /// La sant� maximale du joueur.
    /// </summary>
    public int maxHealth = 100;

    /// <summary>
    /// La sant� actuelle du joueur.
    /// </summary>
    public int currentHealth;

    /// <summary>
    /// D�lai entre les flashs du sprite lors de l'invincibilit� (en secondes).
    /// </summary>
    public float invincibilityFlashDelay = 0.2f;

    /// <summary>
    /// Temps d'invincibilit� apr�s avoir pris des d�g�ts (en secondes).
    /// </summary>
    public float invincibilityTimeAfterHit = 3f;

    /// <summary>
    /// Indique si le joueur est actuellement invincible.
    /// </summary>
    public bool isInvincible = false;

    /// <summary>
    /// R�f�rence � la barre de vie du joueur.
    /// </summary>
    public HealthBar healthBar;

    /// <summary>
    /// R�f�rence au SpriteRenderer pour rendre le joueur clignotant lorsqu'il est invincible.
    /// </summary>
    public SpriteRenderer graphics;

    /// <summary>
    /// Instance statique pour g�rer l'acc�s global � PlayerHealth.
    /// </summary>
    public static PlayerHealth instance;

    /// <summary>
    /// Source audio du joueur.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Son jou� lorsque le joueur prend des d�g�ts.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// S'ex�cute avant le Start, initialise l'instance unique de PlayerHealth.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance PlayerHealth dans la sc�ne");
            return;
        }
        instance = this;
    }

    /// <summary>
    /// Initialise les valeurs de sant� et met � jour la barre de vie au d�marrage du jeu.
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
            Debug.LogError("La r�f�rence � la barre de vie (HealthBar) est manquante.");
        }
    }

    /// <summary>
    /// V�rifie les entr�es utilisateur chaque frame, notamment pour tester la prise de d�g�ts.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            TakeDamage(40);
        }
    }

    /// <summary>
    /// Ajoute de la sant� au joueur.
    /// </summary>
    /// <param name="amount">Quantit� de points de vie � ajouter.</param>
    public void HealPlayer(int amount)
    {
        currentHealth += amount;
        if (healthBar != null)
        {
            healthBar.SetHealth(currentHealth);
        }
    }

    /// <summary>
    /// R�duit la sant� du joueur en fonction des d�g�ts subis.
    /// </summary>
    /// <param name="damage">Quantit� de d�g�ts inflig�s au joueur.</param>
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
    /// G�re la mort du joueur.
    /// </summary>
    public void Die()
    {
        Debug.Log("Le joueur est �limin�");
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
    /// R�initialise la position et la sant� du joueur apr�s une mort.
    /// </summary>
    public void Respawn()
    {
        Debug.Log("Le joueur est replac� au d�but");
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
    /// Coroutine qui fait clignoter le sprite du joueur pour signaler l'invincibilit�.
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
    /// Coroutine qui g�re la dur�e de l'invincibilit� apr�s avoir pris des d�g�ts.
    /// </summary>
    public IEnumerator HandleInvincibilityDealy()
    {
        yield return new WaitForSeconds(invincibilityTimeAfterHit);
        isInvincible = false;
    }
}