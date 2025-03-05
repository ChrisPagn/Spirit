using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class PickUpItem : MonoBehaviour
{
    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    public Item item;
    public AudioSource audioSource;
    public AudioClip sound;


    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            TakeItem();
        }
    }

    public void TakeItem()
    {
        Inventory.instance.contentItems.Add(item);
        Inventory.instance.UpdateInventoryUI();
        audioSource.PlayOneShot(sound);
        InteractUILadder.enabled = false;
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractUILadder.enabled = true;
            IsInRange = true;
        }
    }


    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            InteractUILadder.enabled = false;
            IsInRange = false;
        }
    }
}
