using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Ce script g�re la zone de mort dans un jeu.
/// Lorsqu'un joueur entre en collision avec une zone de mort (DeathZone), il est renvoy� � son dernier point de r�apparition (spawn).
/// </summary>
public class DeathZone : MonoBehaviour
{
    // R�f�rence au point de r�apparition du joueur
    private Transform playerSpawn;

    // R�f�rence � l'Animator qui g�re l'animation de transition (comme un effet de fondu)
    private Animator fadeSystem;

    // R�f�rence au GameObject du joueur
    private GameObject player;

    //// R�f�rence au gestionnaire des DeathZones
    //private DeathZonesManager zoneManager;

    /// <summary>
    /// M�thode appel�e � l'initialisation de l'objet. 
    /// Elle permet d'assigner les r�f�rences n�cessaires au bon fonctionnement de la DeathZone.
    /// </summary>
    private void Awake()
    {
        // Trouve l'objet ayant le tag "PlayerSpawn" et r�cup�re son Transform
        GameObject spawnObject = GameObject.FindGameObjectWithTag("PlayerSpawn");

        // Trouve l'objet ayant le tag "FadeSystem" et r�cup�re son Animator
        GameObject fadeObject = GameObject.FindGameObjectWithTag("FadeSystem");

        //// Trouve le gestionnaire de zones de mort
        //zoneManager = FindObjectOfType<DeathZonesManager>();

        // Trouve le joueur au d�but du jeu
        player = GameObject.FindGameObjectWithTag("Player");

        // V�rifie si l'objet de spawn du joueur a �t� trouv�, sinon log un avertissement
        if (spawnObject != null)
        {
            playerSpawn = spawnObject.transform;
        }
        else
        {
            Debug.LogWarning("Le PlayerSpawn est introuvable !");
        }

        // V�rifie si l'objet de syst�me de fondu a �t� trouv�, sinon log un avertissement
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
    /// M�thode appel�e lorsqu'un autre objet entre dans le collider de la DeathZone.
    /// Ici, si l'objet est le joueur, on d�clenche le processus de r�apparition.
    /// </summary>
    /// <param name="collision">Le collider de l'objet entrant en collision avec la zone.</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'objet en collision est le joueur, lancer la r�apparition
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ReplacePlayer());
        }
    }

    /// <summary>
    /// Coroutine qui g�re le d�placement du joueur vers son point de r�apparition apr�s une animation de fondu.
    /// </summary>
    private IEnumerator ReplacePlayer()
    {
        // Si un fade system est disponible, d�clencher l'animation de fondu et attendre qu'elle se termine
        if (fadeSystem != null)
        {
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
        }

        // S'assurer que les r�f�rences au joueur et au point de spawn sont valides avant de d�placer le joueur
        if (player != null && playerSpawn != null)
        {
            // D�placer le joueur � la position du spawn
            player.transform.position = playerSpawn.position;
            Debug.Log("Le joueur a �t� d�plac� � la position de spawn.");
        }
        else
        {
            Debug.LogWarning("Le joueur ou le PlayerSpawn est manquant.");
        }
    }
}
