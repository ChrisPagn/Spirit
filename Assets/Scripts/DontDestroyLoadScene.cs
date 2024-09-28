using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script marque les objets spécifiés pour qu'ils ne soient pas détruits lors du chargement d'une nouvelle scène.
/// </summary>
public class DontDestroyLoadScene : MonoBehaviour
{
    // Tableau des objets qui ne doivent pas être détruits lors du changement de scène
    public GameObject[] gameObjects;

    /// <summary>
    /// Méthode appelée à l'initialisation de l'objet. Elle vérifie chaque élément du tableau et applique la méthode DontDestroyOnLoad si nécessaire.
    /// </summary>
    void Awake()
    {
        // Parcourt chaque élément du tableau de GameObjects
        foreach (var element in gameObjects)
        {
            // Vérifie que l'élément n'est pas nul et qu'il n'est pas déjà marqué comme "DontDestroyOnLoad"
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                // Marque cet objet pour qu'il ne soit pas détruit lors du chargement d'une nouvelle scène
                DontDestroyOnLoad(element);
            }
        }
    }
}
