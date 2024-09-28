using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Classe qui g�re le spawn du joueur dans une sc�ne.
/// </summary>
public class PlayerSpawn : MonoBehaviour
{
    /// <summary>
    /// M�thode appel�e au d�marrage du script.
    /// D�place le joueur � la position du script si il est trouv� dans la sc�ne.
    /// </summary>
    private void Awake()
    {
        // Recherche le joueur dans la sc�ne en utilisant son tag "Player"
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            // D�place le joueur � la position du script
            player.transform.position = transform.position;
        }
        else
        {
            // Affiche un avertissement si le joueur n'est pas trouv� dans la sc�ne
            Debug.LogWarning("Player introuvable dans la sc�ne script PlayerSpawn!");
        }
    }
}