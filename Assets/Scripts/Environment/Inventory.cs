using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

/// <summary>
/// Gère l'inventaire du joueur, y compris le compteur de pièces et l'interface utilisateur associée.
/// </summary>
public class Inventory : MonoBehaviour
{
    /// <summary>
    /// Nombre actuel de pièces dans l'inventaire.
    /// </summary>
    public int coinsCount;

    /// <summary>
    /// Texte standard (UnityEngine.UI) pour afficher le nombre de pièces dans l'interface utilisateur.
    /// </summary>
    public Text coinsCountText;

    /// <summary>
    /// Liste des objets contenus dans l'inventaire.
    /// </summary>
    public List<Item> contentItems = new List<Item>();

    /// <summary>
    /// Index de l'objet actuellement sélectionné dans l'inventaire.
    /// </summary>
    private int contentCurrentIndex = 0;

    /// <summary>
    /// Image de l'objet actuellement sélectionné dans l'interface utilisateur.
    /// </summary>
    public Image itemImageUI;

    /// <summary>
    /// Image affichée lorsque l'inventaire est vide.
    /// </summary>
    public Sprite emptyItemImage;

    /// <summary>
    /// Texte (TextMeshPro) pour afficher le nom de l'objet actuellement sélectionné dans l'interface utilisateur.
    /// </summary>
    public TextMeshProUGUI itemNameUI;

    /// <summary>
    /// Texte (TextMeshPro) pour afficher le nombre de pièces dans l'interface utilisateur.
    /// </summary>
    public TMP_Text coinsText;

    /// <summary>
    /// Référence aux effets du joueur pour appliquer les effets des objets consommés.
    /// </summary>
    public PlayerEffects playerEffects;

    /// <summary>
    /// Instance unique de l'inventaire (Singleton).
    /// </summary>
    public static Inventory instance;

    /// <summary>
    /// Initialise l'instance unique de l'inventaire.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance de Inventory dans la scène!");
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Met à jour l'interface utilisateur de l'inventaire au démarrage.
    /// </summary>
    public void Start()
    {
        UpdateInventoryUI();
    }

    /// <summary>
    /// Consomme l'objet actuellement sélectionné dans l'inventaire, applique ses effets, et le retire de l'inventaire.
    /// </summary>
    public void ConsumeItem()
    {
        if (contentItems.Count == 0)
        {
            return;
        }
        Item currentItem = contentItems[contentCurrentIndex];
        PlayerHealth.instance.HealPlayer(currentItem.hpGiven);
        playerEffects.AddSpeed(currentItem.speedGiven, currentItem.speedDuration);
        contentItems.Remove(currentItem);
        GetNextItem();
        UpdateInventoryUI();
    }

    /// <summary>
    /// Sélectionne l'objet suivant dans l'inventaire.
    /// </summary>
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

    /// <summary>
    /// Sélectionne l'objet précédent dans l'inventaire.
    /// </summary>
    public void GetPreviousItem()
    {
        contentCurrentIndex--;
        if (contentCurrentIndex < 0)
        {
            contentCurrentIndex = contentItems.Count - 1;
        }
        UpdateInventoryUI();
    }

    /// <summary>
    /// Met à jour l'interface utilisateur pour refléter l'état actuel de l'inventaire.
    /// </summary>
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
    /// Ajoute un certain nombre de pièces à l'inventaire et met à jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pièces à ajouter.</param>
    public void AddCoins(int count)
    {
        coinsCount += count;
        UpdateTextUI();
    }

    /// <summary>
    /// Retire un certain nombre de pièces de l'inventaire et met à jour l'interface utilisateur.
    /// </summary>
    /// <param name="count">Le nombre de pièces à retirer.</param>
    public void RemoveCoins(int count)
    {
        coinsCount -= count;
        UpdateTextUI();
    }

    /// <summary>
    /// Met à jour le texte de l'interface utilisateur pour refléter le nombre actuel de pièces.
    /// </summary>
    public void UpdateTextUI()
    {
        if (coinsText != null)
        {
            coinsText.text = coinsCount.ToString();
        }
        if (coinsCountText != null)
        {
            coinsCountText.text = coinsCount.ToString();
        }
    }
}
