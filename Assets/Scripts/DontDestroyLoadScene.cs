using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Ce script marque les objets sp�cifi�s pour qu'ils ne soient pas d�truits lors du chargement d'une nouvelle sc�ne.
/// </summary>
public class DontDestroyLoadScene : MonoBehaviour
{
    // Tableau des objets qui ne doivent pas �tre d�truits lors du changement de sc�ne
    public GameObject[] gameObjects;

    /// <summary>
    /// M�thode appel�e � l'initialisation de l'objet. Elle v�rifie chaque �l�ment du tableau et applique la m�thode DontDestroyOnLoad si n�cessaire.
    /// </summary>
    void Awake()
    {
        // Parcourt chaque �l�ment du tableau de GameObjects
        foreach (var element in gameObjects)
        {
            // V�rifie que l'�l�ment n'est pas nul et qu'il n'est pas d�j� marqu� comme "DontDestroyOnLoad"
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                // Marque cet objet pour qu'il ne soit pas d�truit lors du chargement d'une nouvelle sc�ne
                DontDestroyOnLoad(element);
            }
        }
    }
}
