using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// G�re une base de donn�es d'objets dans le jeu.
/// </summary>
public class ItemsDataBase : MonoBehaviour
{
    /// <summary>
    /// Tableau contenant tous les objets disponibles dans le jeu.
    /// </summary>
    public Item[] allItems;

    /// <summary>
    /// Instance unique de la base de donn�es d'objets.
    /// </summary>
    public static ItemsDataBase instance;

    /// <summary>
    /// Initialise l'instance unique de la base de donn�es d'objets.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance ItemsDataBase dans la sc�ne");
            return;
        }

        instance = this;
    }
}
