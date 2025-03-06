using TMPro;
using UnityEngine;

public class ShopTrigger : MonoBehaviour
{
    private bool isInRange;

    private TextMeshProUGUI interactUI;

    public string npcName;
    public Item[] itemsToShell;
    private void Awake()
    {
        interactUI = GameObject.FindGameObjectWithTag("InteractUILadder").GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (isInRange && Input.GetKeyDown(KeyCode.E))
        {
            ShopManager.instance.OpenShop(itemsToShell, npcName);
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = true;
            interactUI.enabled = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            isInRange = false;
            interactUI.enabled = false;
            ShopManager.instance.CloseShop();
        }
    }
}
