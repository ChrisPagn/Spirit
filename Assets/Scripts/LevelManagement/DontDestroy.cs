using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroy : MonoBehaviour
{
    void Awake()
    {
        // Empêche cet objet d'être détruit lors du changement de scène
        DontDestroyOnLoad(gameObject);
    }
}
