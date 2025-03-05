using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

/// <summary>
/// Ce script gère la zone de mort dans un jeu.
/// Lorsqu'un joueur entre en collision avec une zone de mort (DeathZone).
/// </summary>
public class DeathZone : MonoBehaviour
{
    

    // Référence à l'Animator qui gère l'animation de transition (comme un effet de fondu)
    private Animator fadeSystem;

    // Référence au GameObject du joueur
    private GameObject player;

    /// <summary>
    /// Méthode appelée à l'initialisation de l'objet. 
    /// Elle permet d'assigner les références nécessaires au bon fonctionnement de la DeathZone.
    /// </summary>
    private void Awake()
    {
       
        // Trouve l'objet ayant le tag "FadeSystem" et récupère son Animator
        GameObject fadeObject = GameObject.FindGameObjectWithTag("FadeSystem");

        // Trouve le joueur au début du jeu
        player = GameObject.FindGameObjectWithTag("Player");

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

          fadeSystem.SetTrigger("FadeIn");
          yield return new WaitForSeconds(0.2f);
          player.transform.position = CurrentSceneManager.instance.respawnPoint;
        
    }
}
