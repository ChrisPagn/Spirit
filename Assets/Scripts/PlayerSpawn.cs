using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui gère le spawn du joueur dans une scène.
/// </summary>
public class PlayerSpawn : MonoBehaviour
{
    /// <summary>
    /// Méthode appelée au démarrage du script.
    /// Déplace le joueur à la position du script si il est trouvé dans la scène.
    /// </summary>
    private void Awake()
    {
        // Recherche le joueur dans la scène en utilisant son tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // Déplace le joueur à la position du script
            player.transform.position = transform.position;
        }
        else
        {
            // Affiche un avertissement si le joueur n'est pas trouvé dans la scène
            Debug.LogWarning("Player introuvable dans la scène script PlayerSpawn!");
        }
    }
}