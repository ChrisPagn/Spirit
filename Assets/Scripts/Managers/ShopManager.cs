using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ShopManager : MonoBehaviour
{
    public TextMeshProUGUI npcNameText;
    public Animator animator;
    public GameObject sellButtonPrefab;
    public Transform sellButtonsParent;

    /// <summary>
    /// singleton de l'instance
    /// </summary>
    public static ShopManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance ShopManager dans la scène");
            return;
        }
        instance = this;
    }

    public void OpenShop(Item[] items, string mpcName)
    {
        npcNameText.text = mpcName;
        UpdateItemsToSell(items);
        animator.SetBool("isOpen", true);
    }

    public void UpdateItemsToSell(Item[] items)
    {
        // supprime les potentiels doutons present dans le parent
        for (int i = 0; i < sellButtonsParent.childCount; i++)
        {
            Destroy(sellButtonsParent.GetChild(i).gameObject);
        }

        // Instancie un bouton pour chaque item a vendre et l'instancie
        for (int i = 0; i < items.Length; i++) 
        {
            GameObject button = Instantiate(sellButtonPrefab, sellButtonsParent);
            SellButtonItem sellButton = button.GetComponent<SellButtonItem>();
            sellButton.itemName.text = items[i].name;
            sellButton.itemImage.sprite = items[i].image;
            sellButton.itemPrice.text = items[i].price.ToString();

            sellButton.item = items[i];
 
            button.GetComponent<Button>().onClick.AddListener(delegate { sellButton.BuyItem();});
        }
    }

    public void CloseShop()
    {
        animator.SetBool("isOpen", false);
    }

   
}
