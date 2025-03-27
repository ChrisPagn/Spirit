using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Variable statique pour stocker l'instance unique
    private static DontDestroy instance;

    void Awake()
    {
        // Vérifie si une instance existe déjà
        if (instance == null)
        {
            // Si aucune instance n'existe, définissez celle-ci comme l'instance unique
            instance = this;
            // Empêche cet objet d'être détruit lors du changement de scène
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si une instance existe déjà, détruisez cet objet
            Destroy(gameObject);
        }
    }
}
