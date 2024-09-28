using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Ce script gère la zone de mort dans un jeu.
/// Lorsqu'un joueur entre en collision avec une zone de mort (DeathZone), il est renvoyé à son dernier point de réapparition (spawn).
/// </summary>
public class DeathZone : MonoBehaviour
{
    // Référence au point de réapparition du joueur
    private Transform playerSpawn;

    // Référence à l'Animator qui gère l'animation de transition (comme un effet de fondu)
    private Animator fadeSystem;

    // Référence au GameObject du joueur
    private GameObject player;

    //// Référence au gestionnaire des DeathZones
    //private DeathZonesManager zoneManager;

    /// <summary>
    /// Méthode appelée à l'initialisation de l'objet. 
    /// Elle permet d'assigner les références nécessaires au bon fonctionnement de la DeathZone.
    /// </summary>
    private void Awake()
    {
        // Trouve l'objet ayant le tag "PlayerSpawn" et récupère son Transform
        GameObject spawnObject = GameObject.FindGameObjectWithTag("PlayerSpawn");

        // Trouve l'objet ayant le tag "FadeSystem" et récupère son Animator
        GameObject fadeObject = GameObject.FindGameObjectWithTag("FadeSystem");

        //// Trouve le gestionnaire de zones de mort
        //zoneManager = FindObjectOfType<DeathZonesManager>();

        // Trouve le joueur au début du jeu
        player = GameObject.FindGameObjectWithTag("Player");

        // Vérifie si l'objet de spawn du joueur a été trouvé, sinon log un avertissement
        if (spawnObject != null)
        {
            playerSpawn = spawnObject.transform;
        }
        else
        {
            Debug.LogWarning("Le PlayerSpawn est introuvable !");
        }

        // Vérifie si l'objet de système de fondu a été trouvé, sinon log un avertissement
        if (fadeObject != null)
        {
            fadeSystem = fadeObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("Le FadeSystem est introuvable !");
        }
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre dans le collider de la DeathZone.
    /// Ici, si l'objet est le joueur, on déclenche le processus de réapparition.
    /// </summary>
    /// <param name="collision">Le collider de l'objet entrant en collision avec la zone.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'objet en collision est le joueur, lancer la réapparition
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ReplacePlayer());
        }
    }

    /// <summary>
    /// Coroutine qui gère le déplacement du joueur vers son point de réapparition après une animation de fondu.
    /// </summary>
    private IEnumerator ReplacePlayer()
    {
        // Si un fade system est disponible, déclencher l'animation de fondu et attendre qu'elle se termine
        if (fadeSystem != null)
        {
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
        }

        // S'assurer que les références au joueur et au point de spawn sont valides avant de déplacer le joueur
        if (player != null && playerSpawn != null)
        {
            // Déplacer le joueur à la position du spawn
            player.transform.position = playerSpawn.position;
            Debug.Log("Le joueur a été déplacé à la position de spawn.");
        }
        else
        {
            Debug.LogWarning("Le joueur ou le PlayerSpawn est manquant.");
        }
    }
}
