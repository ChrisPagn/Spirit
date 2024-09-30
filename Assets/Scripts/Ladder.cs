using System.Collections;
using System.Collections.Generic;
using TMPro; // Ajoute TextMeshPro namespace
using UnityEngine;

public class Ladder : MonoBehaviour
{
    private bool isInRange;

    public TextMeshProUGUI interactUI; // Remplacer Text par TextMeshProUGUI

    private PlayerMovement playerMovement;

    public BoxCollider2D topCollider2D;

    private void Awake()
    {
        playerMovement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerMovement>();
        interactUI = GameObject.FindGameObjectWithTag("InteractUI").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
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
            interactUI.enabled = true; // Affiche l'UI d'interaction
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
                topCollider2D.isTrigger = false;
                interactUI.enabled = false; // Cache l'UI d'interaction
            }
        }
    }
}
