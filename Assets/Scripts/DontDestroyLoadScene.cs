using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DontDestroyLoadScene : MonoBehaviour
{
    public GameObject[] gameObjects;

    void Awake()
    {
        foreach (var element in gameObjects)
        {
            // Si l'objet est déjà marqué comme "DontDestroyOnLoad", ne pas l'ajouter à nouveau
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                DontDestroyOnLoad(element);
            }
        }
    }
}

