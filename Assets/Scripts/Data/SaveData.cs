using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Classe sérialisable représentant les données de sauvegarde du jeu.
/// Contient toutes les informations nécessaires pour sauvegarder et restaurer l'état du jeu.
/// </summary>
[System.Serializable]
public class SaveData
{
    /// <summary>
    /// Identifiant unique de l'utilisateur.
    /// </summary>
    public string UserId;

    /// <summary>
    /// Nom d'affichage de l'utilisateur.
    /// </summary>
    public string DisplayName;

    /// <summary>
    /// Nombre de pièces que possède le joueur.
    /// </summary>
    public int CoinsCount;

    /// <summary>
    /// Niveau le plus élevé atteint par le joueur.
    /// </summary>
    public int LevelReached;

    /// <summary>
    /// Menu stocké en DB qui sera chargé.
    /// </summary>
    public string LastLevelPlayed;
    /// <summary>
    /// Liste des identifiants des objets dans l'inventaire du joueur.
    /// </summary>
    public List<int> InventoryItems;

    /// <summary>
    /// Liste des noms des objets dans l'inventaire du joueur.
    /// </summary>
    public List<string> InventoryItemsName;

    /// <summary>
    /// Date et heure de la dernière modification des données de sauvegarde.
    /// </summary>
    public DateTime LastModified;

    // **Constructeur par défaut requis pour la désérialisation**
    public SaveData()
    {
        InventoryItems = new List<int>();
        InventoryItemsName = new List<string>();
    }

    /// <summary>
    /// Construction basique de la classe SaveData.
    /// </summary>
    /// <param name="userId"></param>
    /// <param name="displayName"></param>
    /// <param name="coinsCount"></param>
    /// <param name="levelReached"></param>
    /// <param name="inventoryItem"></param>
    /// <param name="inventgoryItemsName"></param>
    /// <param name="lastModified"></param>
    public SaveData(string userId, string displayName, int coinsCount, int levelReached, List<int> inventoryItem, List<string> inventgoryItemsName, DateTime lastModified, string lastLevelPlayed)
    {
        try
        {
            UserId = userId;
            DisplayName = displayName;
            CoinsCount = coinsCount;
            LevelReached = levelReached;
            InventoryItems = inventoryItem;
            InventoryItemsName = inventgoryItemsName;
            LastModified = lastModified;
            LastLevelPlayed = lastLevelPlayed;
        }
        catch (Exception ex)
        {
            //On tombe dans ce cas car notre classe à changer et que l'instanciation de l'objet SaveData n'est pas à jour.
            Debug.LogError($"Erreur lors de la création de l'objet SaveData : {ex.Message}");
        }

    }

    /// <summary>
    /// Constructeur de la classe SaveData réduit.
    /// </summary>
    /// <param name="displayName"></param>
    /// <param name="coinsCount"></param>
    /// <param name="levelReached"></param>
    /// <param name="lastModified"></param>
    public SaveData(string displayName, int coinsCount, int levelReached,DateTime lastModified)
    {
        try
        {
            DisplayName = displayName;
            CoinsCount = coinsCount;
            LevelReached = levelReached;
            LastModified = lastModified;
        }
        catch (Exception ex)
        {
            //On tombe dans ce cas car notre classe à changer et que l'instanciation de l'objet SaveData n'est pas à jour.
            Debug.LogError($"Erreur lors de la création de l'objet SaveData : {ex.Message}");
        }

    }
   
}
