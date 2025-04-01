using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseMenuOPj : MonoBehaviour
{
    public GameObject pauseTb;
    private void Awake()
    {
        this.gameObject.SetActive(false);   
    }
}
