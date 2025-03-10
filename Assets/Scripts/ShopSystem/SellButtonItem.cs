using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère l'affichage et l'achat d'un objet via un bouton de vente dans l'interface utilisateur.
/// </summary>
public class SellButtonItem : MonoBehaviour
{
    /// <summary>
    /// Nom de l'objet affiché sur le bouton.
    /// </summary>
    public TextMeshProUGUI itemName;

    /// <summary>
    /// Image représentant l'objet.
    /// </summary>
    public Image itemImage;

    /// <summary>
    /// Prix de l'objet affiché sur le bouton.
    /// </summary>
    public TextMeshProUGUI itemPrice;

    /// <summary>
    /// Référence à l'objet associé à ce bouton de vente.
    /// </summary>
    public Item item;

    /// <summary>
    /// Achète l'objet si le joueur possède suffisamment de pièces, l'ajoute à l'inventaire et met à jour l'interface utilisateur.
    /// </summary>
    public void BuyItem()
    {
        Inventory inventory = Inventory.instance;

        // Vérifie si le joueur a assez de pièces pour acheter l'objet
        if (inventory.coinsCount >= item.price)
        {
            // Ajoute l'objet à l'inventaire
            inventory.contentItems.Add(item);

            // Met à jour l'affichage de l'inventaire
            inventory.UpdateInventoryUI();

            // Déduit le prix de l'objet du total de pièces du joueur
            inventory.coinsCount -= item.price;

            // Met à jour l'affichage du nombre de pièces
            inventory.UpdateTextUI();
        }
    }
}
