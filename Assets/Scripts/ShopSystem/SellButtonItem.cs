using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// G�re l'affichage et l'achat d'un objet via un bouton de vente dans l'interface utilisateur.
/// </summary>
public class SellButtonItem : MonoBehaviour
{
    /// <summary>
    /// Nom de l'objet affich� sur le bouton.
    /// </summary>
    public TextMeshProUGUI itemName;

    /// <summary>
    /// Image repr�sentant l'objet.
    /// </summary>
    public Image itemImage;

    /// <summary>
    /// Prix de l'objet affich� sur le bouton.
    /// </summary>
    public TextMeshProUGUI itemPrice;

    /// <summary>
    /// R�f�rence � l'objet associ� � ce bouton de vente.
    /// </summary>
    public Item item;

    /// <summary>
    /// Ach�te l'objet si le joueur poss�de suffisamment de pi�ces, l'ajoute � l'inventaire et met � jour l'interface utilisateur.
    /// </summary>
    public void BuyItem()
    {
        Inventory inventory = Inventory.instance;

        // V�rifie si le joueur a assez de pi�ces pour acheter l'objet
        if (inventory.coinsCount >= item.price)
        {
            // Ajoute l'objet � l'inventaire
            inventory.contentItems.Add(item);

            // Met � jour l'affichage de l'inventaire
            inventory.UpdateInventoryUI();

            // D�duit le prix de l'objet du total de pi�ces du joueur
            inventory.coinsCount -= item.price;

            // Met � jour l'affichage du nombre de pi�ces
            inventory.UpdateTextUI();
        }
    }
}
