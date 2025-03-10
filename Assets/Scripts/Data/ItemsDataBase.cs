using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Gère une base de données d'objets dans le jeu.
/// </summary>
public class ItemsDataBase : MonoBehaviour
{
    /// <summary>
    /// Tableau contenant tous les objets disponibles dans le jeu.
    /// </summary>
    public Item[] allItems;

    /// <summary>
    /// Instance unique de la base de données d'objets.
    /// </summary>
    public static ItemsDataBase instance;

    /// <summary>
    /// Initialise l'instance unique de la base de données d'objets.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance ItemsDataBase dans la scène");
            return;
        }

        instance = this;
    }
}
