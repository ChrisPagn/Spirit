
using TMPro;
using UnityEngine;

public class Chest : MonoBehaviour
{

    private TextMeshProUGUI InteractUILadder;
    private bool IsInRange;

    public Animator animator;
    public AudioSource audioSource;
    public AudioClip sound;

    public int coinsToAdd;

    void Awake()
    {
        InteractUILadder = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>(); // Récupération du TextMeshProUGUI

    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.E) && IsInRange)
        {
            OpenChest();
        }
    }

    public void OpenChest()
    {
        animator.SetTrigger("OpenChest");
        Inventory.instance.AddCoins(coinsToAdd);
        audioSource.PlayOneShot(sound);
        GetComponent<BoxCollider2D>().enabled = false;
        InteractUILadder.enabled = false;
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
