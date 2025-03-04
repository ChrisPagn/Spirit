using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Ce script gère l'inventaire, plus précisément le compteur de pièces du joueur, 
/// et met à jour l'interface utilisateur en conséquence.
/// </summary>
public class Inventory : MonoBehaviour
{
    // Nombre actuel de pièces
    public int coinsCount;

    // Texte standard pour afficher le nombre de pièces dans l'interface utilisateur (UnityEngine.UI)
    public Text coinsCountText;

    public List<Item> contentItems = new List<Item>();
    public int contentCurrentIndex = 0;
    public Image itemImageUI;
    public Sprite emptyItemImage;
    public TextMeshProUGUI itemNameUI;

    // Texte pour TextMeshPro pour afficher le nombre de pièces dans l'interface utilisateur (TMPro)
    public TMP_Text coinsText;

    // Instance Singleton de l'inventaire
    public static Inventory instance;

    /// <summary>
    /// La méthode Awake est appelée lorsque le script est chargé pour la première fois. 
    /// Elle initialise le Singleton de l'inventaire.
    /// </summary>
    private void Awake()
    {
        // Vérifie si une autre instance d'Inventory existe déjà
        if (instance != null)
        {
            // Avertissement dans la console Unity si plus d'une instance est présente
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène!");

            // Supprime cette instance supplémentaire pour maintenir le modèle Singleton
            Destroy(gameObject);
            return;
        }

        // Assigne cette instance comme étant l'instance unique (Singleton)
        instance = this;
    }

    public void Start()
    {
        UpdateInventoryUI();
    }

    public void ConsumeItem()
    {
        if (contentItems.Count == 0)
        {
            return;
        }
        Item currentItem = contentItems[contentCurrentIndex];
        PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
        PlayerMovement.instance.moveSpeed += currentItem.speedGiven;
        contentItems.Remove(currentItem);
        GetNextItem();
        UpdateInventoryUI();
    }

    public void GetNextItem()
    {
        if (contentItems.Count == 0)
        {
            return;
        }
        contentCurrentIndex++;
        if (contentCurrentIndex > contentItems.Count - 1)
        {
            contentCurrentIndex = 0;
        }
        UpdateInventoryUI();
    }

    public void GetPreviousItem()
    {
        contentCurrentIndex--;
        if (contentCurrentIndex < 0)
        {
            contentCurrentIndex = contentItems.Count - 1;
        }
        UpdateInventoryUI();
    }

    public void UpdateInventoryUI()
    {
        if (contentItems.Count > 0)
        {
            itemImageUI.sprite = contentItems[contentCurrentIndex].image;
            itemNameUI.text = contentItems[contentCurrentIndex].name;
        }
        else
        {
            itemImageUI.sprite = emptyItemImage;
            itemNameUI.text = "";
        }
    }

    /// <summary>
    /// Méthode pour ajouter un certain nombre de pièces à l'inventaire et mettre à jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pièces à ajouter</param>
    public void AddCoins(int count)
    {
        // Augmente le compteur de pièces
        coinsCount += count;

        // Si un texte standard (UnityEngine.UI) est assigné, met à jour son affichage avec le nouveau nombre de pièces
        if (coinsCountText != null)
        {
               UpdateTextUI();
        }

        // Si un texte TextMeshPro est assigné, met à jour son affichage avec le nouveau nombre de pièces
        if (coinsText != null)
        {
               UpdateTextUI();
        }
    }

    /// <summary>
    /// Méthode pour retirer un certain nombre de pièces de l'inventaire et mettre à jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pièces à retirer</param>
    public void RemoveCoins(int count)
    {
        // Diminue le compteur de pièces
        coinsCount -= count;

        // Si un texte standard (UnityEngine.UI) est assigné, met à jour son affichage avec le nouveau nombre de pièces
        if (coinsCountText != null)
        {
               UpdateTextUI();
          }

        // Si un texte TextMeshPro est assigné, met à jour son affichage avec le nouveau nombre de pièces
        if (coinsText != null)
        {
               UpdateTextUI();
        }
    }

     public void UpdateTextUI()
     {
          coinsText.text = coinsCount.ToString();
     }
}
