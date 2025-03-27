using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    // Variable statique pour stocker l'instance unique
    private static DontDestroy instance;

    void Awake()
    {
        // V�rifie si une instance existe d�j�
        if (instance == null)
        {
            // Si aucune instance n'existe, d�finissez celle-ci comme l'instance unique
            instance = this;
            // Emp�che cet objet d'�tre d�truit lors du changement de sc�ne
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Si une instance existe d�j�, d�truisez cet objet
            Destroy(gameObject);
        }
    }
}
