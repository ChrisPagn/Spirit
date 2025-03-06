using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class SellButtonItem : MonoBehaviour
{
   public TextMeshProUGUI itemName;
   public Image itemImage;
   public TextMeshProUGUI itemPrice;

    public Item item;

    public void BuyItem()
    {
        Inventory inventory = Inventory.instance;

        if (inventory.coinsCount >= item.price)
        {
            inventory.contentItems.Add(item);
            inventory.UpdateInventoryUI();
            inventory.coinsCount -= item.price;
            inventory.UpdateTextUI();
        }
    }
}
