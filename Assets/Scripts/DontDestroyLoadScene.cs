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
            // Si l'objet est d�j� marqu� comme "DontDestroyOnLoad", ne pas l'ajouter � nouveau
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                DontDestroyOnLoad(element);
            }
        }
    }
}

