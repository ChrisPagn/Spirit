using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class DeathZone : MonoBehaviour
{
    private Transform playerSpawn;
    private Animator fadeSystem;
    private GameObject player; // Assigne correctement le joueur
    private DeathZonesManager zoneManager;

    private void Awake()
    {
        GameObject spawnObject = GameObject.FindGameObjectWithTag("PlayerSpawn");
        GameObject fadeObject = GameObject.FindGameObjectWithTag("FadeSystem");
        zoneManager = FindObjectOfType<DeathZonesManager>();

        // Trouver le joueur au début
        player = GameObject.FindGameObjectWithTag("Player");

        if (spawnObject != null)
        {
            playerSpawn = spawnObject.transform;
        }
        else
        {
            Debug.LogWarning("PlayerSpawn not found!");
        }

        if (fadeObject != null)
        {
            fadeSystem = fadeObject.GetComponent<Animator>();
        }
        else
        {
            Debug.LogWarning("FadeSystem not found!");
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(ReplacePlayer());
        }
    }

    private IEnumerator ReplacePlayer()
    {
        if (fadeSystem != null)
        {
            fadeSystem.SetTrigger("FadeIn");
            yield return new WaitForSeconds(1f);
        }

        // Assure-toi que player et playerSpawn sont valides avant de changer la position
        if (player != null && playerSpawn != null)
        {
            player.transform.position = playerSpawn.position;
            Debug.Log("Joueur déplacé à la position de spawn");
        }
        else
        {
            Debug.LogWarning("Le joueur ou le PlayerSpawn est manquant.");
        }
    }
}


