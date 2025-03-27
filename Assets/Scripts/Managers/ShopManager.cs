using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Gère l'interface et les fonctionnalités de la boutique dans le jeu.
/// </summary>
public class ShopManager : MonoBehaviour
{
    public ShopUIReference shopUIReference;
    /// <summary>
    /// Texte affichant le nom du PNJ vendeur.
    /// </summary>
    public TextMeshProUGUI npcNameText;

    /// <summary>
    /// Animator pour gérer les animations de la boutique.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// Préfab du bouton de vente pour les objets.
    /// </summary>
    public GameObject sellButtonPrefab;

    /// <summary>
    /// Parent des boutons de vente dans l'interface utilisateur.
    /// </summary>
    public Transform sellButtonsParent;

    /// <summary>
    /// Instance unique de ShopManager (Singleton).
    /// </summary>
    public static ShopManager instance;

    /// <summary>
    /// Initialise l'instance unique de ShopManager.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance ShopManager dans la scène");
            return;
        }
        instance = this;

        OnLoadReference();
    }

    /// <summary>
    /// Modif flo : Ajout de la méthode OnLoadReference pour charger les références de l'interface utilisateur. 
    /// </summary>
    private void OnLoadReference()
    {
        try
        {
            npcNameText = shopUIReference.npcNameReference;
            animator = shopUIReference.animatorReference;
            sellButtonsParent = shopUIReference.sellButtonsParentReference;
        }
        catch (Exception)
        {
            Debug.LogError("probleme avec les ref");
        }
    
    }

    /// <summary>
    /// Ouvre la boutique et affiche les objets à vendre.
    /// </summary>
    /// <param name="items">Tableau des objets à vendre.</param>
    /// <param name="mpcName">Nom du PNJ vendeur.</param>
    public void OpenShop(Item[] items, string mpcName)
    {
        npcNameText.text = mpcName;
        UpdateItemsToSell(items);
        animator.SetBool("isOpen", true);
    }

    /// <summary>
    /// Met à jour les objets à vendre dans la boutique.
    /// </summary>
    /// <param name="items">Tableau des objets à vendre.</param>
    public void UpdateItemsToSell(Item[] items)
    {
        // Supprime les boutons existants dans le parent
        for (int i = 0; i < sellButtonsParent.childCount; i++)
        {
            Destroy(sellButtonsParent.GetChild(i).gameObject);
        }

        // Instancie un bouton pour chaque objet à vendre
        for (int i = 0; i < items.Length; i++)
        {
            GameObject button = Instantiate(sellButtonPrefab, sellButtonsParent);
            SellButtonItem sellButton = button.GetComponent<SellButtonItem>();
            sellButton.itemName.text = items[i].name;
            sellButton.itemImage.sprite = items[i].image;
            sellButton.itemPrice.text = items[i].price.ToString();

            sellButton.item = items[i];

            button.GetComponent<Button>().onClick.AddListener(delegate { sellButton.BuyItem(); });
        }
    }

    /// <summary>
    /// Ferme la boutique.
    /// </summary>
    public void CloseShop()
    {
        animator.SetBool("isOpen", false);
    }
}
