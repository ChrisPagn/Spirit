using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Script de gestion des points de contr�le (checkpoints) dans le jeu.
/// Lorsqu'un joueur entre en collision avec le checkpoint, le point de r�apparition (spawn) est d�plac� � la position du checkpoint.
/// </summary>
public class CheckPoint : MonoBehaviour
{
    // R�f�rence � la position de r�apparition du joueur (playerSpawn) sous forme de Transform.
    private Transform playerSpawn;

    /// <summary>
    /// Appel� au moment o� l'objet est initialis�.
    /// Ici, on cherche et assigne le Transform du spawn du joueur via son tag "PlayerSpawn".
    /// </summary>
    private void Awake()
    {
        // Trouve l'objet avec le tag "PlayerSpawn" et r�cup�re son Transform.
        playerSpawn = GameObject.FindGameObjectWithTag("PlayerSpawn").transform;
    }

    /// <summary>
    /// M�thode appel�e lorsqu'un autre objet entre en collision avec ce GameObject (le checkpoint).
    /// Ici, on v�rifie si l'objet entrant en collision est le joueur.
    /// </summary>
    /// <param name="collision">Le collider de l'objet entrant en collision.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant en collision a le tag "Player".
        if (collision.CompareTag("Player"))
        {
            Debug.LogWarning("Le joueur est pass� par le checkpoint");

            // D�place la position du spawn du joueur � la position actuelle du checkpoint.
            playerSpawn.position = transform.position;

            // D�sactive le collider du checkpoint pour �viter qu'il soit d�clench� � nouveau.
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Debug.LogWarning("Checkpoint d�sactiv�");
        }
    }
}
