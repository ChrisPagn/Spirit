using System.Collections;
using System.Collections.Generic;
using TMPro; // Ajoute TextMeshPro namespace
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool isInRange;

    private TextMeshProUGUI interactUILadder; // Remplacer Text par TextMeshProUGUI

    private PlayerMovement playerMovement;

    public BoxCollider2D topCollider2D;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        interactUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI
    }


    void Start()
    {

    }


    void Update()
    {
        if (isInRange && playerMovement.isClimbing && Input.GetKeyDown(KeyCode.E))
        {
            // Descendre de l'echelle
            playerMovement.isClimbing = false;
            topCollider2D.isTrigger = false;
            return;
        }

        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            playerMovement.isClimbing = true;
            topCollider2D.isTrigger = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            interactUILadder.enabled = true; // Affiche l'UI d'interaction
            isInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            // On ne désactive isClimbing que si le joueur touche le sol
            if (playerMovement.isGrounded)
            {
                playerMovement.isClimbing = false;
                topCollider2D.isTrigger = false; // Pour marcher sur le collider (sol au dessus de l'echelle)
                interactUILadder.enabled = false; // Cache l'UI d'interaction
            }
        }
    }
}
