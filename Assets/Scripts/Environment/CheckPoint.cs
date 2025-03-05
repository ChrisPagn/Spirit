using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Script de gestion des points de contrôle (checkpoints) dans le jeu.
/// Lorsqu'un joueur entre en collision avec le checkpoint, le point de réapparition (spawn) est déplacé à la position du checkpoint.
/// </summary>
public class CheckPoint : MonoBehaviour
{
 

    public TextMeshProUGUI InteractUIChP;

    /// <summary>
    /// Appelé au moment où l'objet est initialisé.
    /// Ici, on cherche et assigne le Transform du spawn du joueur via son tag "PlayerSpawn".
    /// </summary>
    private void Awake()
    {
      
        InteractUIChP = GameObject.FindGameObjectWithTag("InteractUIChP").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI

    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre en collision avec ce GameObject (le checkpoint).
    /// Ici, on vérifie si l'objet entrant en collision est le joueur.
    /// </summary>
    /// <param name="collision">Le collider de l'objet entrant en collision.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant en collision a le tag "Player".
        if (collision.CompareTag("Player"))
        {
            Debug.LogWarning("Le joueur est passé par le checkpoint");

            // Déplace la position du spawn du joueur à la position actuelle du checkpoint.
           CurrentSceneManager.instance.respawnPoint = transform.position;

            // Désactive le collider du checkpoint pour éviter qu'il soit déclenché à nouveau.
            gameObject.GetComponent<BoxCollider2D>().enabled = false;

            Debug.LogWarning("Checkpoint désactivé");

            // Lance la coroutine pour afficher le message pendant 2 secondes.
            StartCoroutine(ShowMessageForDuration(2f));
        }
    }

    /// <summary>
    /// Methode coroutine  pour le temps d'affichage du message 
    /// </summary>
    /// <param name="duration"></param>
    /// <returns></returns>
    private IEnumerator ShowMessageForDuration(float duration)
    {
        // Active le message.
        InteractUIChP.enabled = true;

        // Attend pendant la durée spécifiée.
        yield return new WaitForSeconds(duration);

        // Désactive le message après la durée spécifiée.
        InteractUIChP.enabled = false;
    }
}
