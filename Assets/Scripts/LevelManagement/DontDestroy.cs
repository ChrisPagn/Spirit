using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        // Emp�che cet objet d'�tre d�truit lors du changement de sc�ne
        DontDestroyOnLoad(gameObject);
    }
}
