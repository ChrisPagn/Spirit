using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// Gère le chargement et la sauvegarde des données de jeu en local.
/// </summary>
public class LoadAndSaveData : MonoBehaviour
{
    /// <summary>
    /// Instance unique de la classe LoadAndSaveData.
    /// </summary>
    public static LoadAndSaveData instance;

    /// <summary>
    /// Initialise l'instance unique de LoadAndSaveData.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance LoadAndSaveData dans la scène");
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Charge les données sauvegardées au démarrage du jeu.
    /// </summary>
    void Start()
    {
        // Charge le nombre de pièces depuis PlayerPrefs
        // PlayerPrefs stocke les données en local sur le disque de l'utilisateur
        // Les données sont stockées dans un fichier de registre sur Windows ou un fichier .plist sur macOS
        Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
        Inventory.instance.UpdateTextUI();

        // Charge les objets sauvegardés dans l'inventaire
        string[] itemsSaved = PlayerPrefs.GetString("inventoryItems", "").Split(',');

        for (int i = 0; i < itemsSaved.Length; i++)
        {
            if (itemsSaved[i] != "")
            {
                int id = int.Parse(itemsSaved[i]);
                Item currentItem = ItemsDataBase.instance.allItems.Single(x => x.id == id);
                Inventory.instance.contentItems.Add(currentItem);
            }
        }

        Inventory.instance.UpdateInventoryUI();
    }

    /// <summary>
    /// Sauvegarde les données actuelles du jeu.
    /// </summary>
    public void SaveData()
    {
        // Sauvegarde le nombre de pièces dans PlayerPrefs
        // Les données sont sauvegardées en local sur le disque de l'utilisateur
        PlayerPrefs.SetInt("coinsCount", Inventory.instance.coinsCount);

        // Sauvegarde le niveau atteint
        if (CurrentSceneManager.instance.levelToUnlock > PlayerPrefs.GetInt("levelReached", 1))
        {
            PlayerPrefs.SetInt("levelReached", CurrentSceneManager.instance.levelToUnlock);
        }

        // Sauvegarde les objets dans l'inventaire
        // Les données sont converties en une chaîne de caractères séparée par des virgules
        string itemsInInventory = string.Join(",", Inventory.instance.contentItems.Select(x => x.id));
        PlayerPrefs.SetString("inventoryItems", itemsInInventory);
    }
}

//*********************
//Aide mémoire
//*********************

// PlayerPrefs sauvegarde les données dans un fichier de registre sur Windows ou un fichier .plist sur macOS
// Ces fichiers sont situés dans un répertoire spécifique à l'application sur le disque de l'utilisateur

//        Fonctionnement de PlayerPrefs:
//        Stockage Local : PlayerPrefs stocke les données en local sur le disque de l'utilisateur.
//                          Sur Windows, les données sont stockées dans le registre, tandis que sur macOS,
//                          elles sont stockées dans un fichier .plist.

//        Persistance: Les données sauvegardées avec PlayerPrefs persistent entre les sessions de jeu,
//                      ce qui signifie qu'elles ne sont pas perdues lorsque le jeu est fermé.

//        Types de Données: PlayerPrefs peut stocker des types de données simples comme des entiers,
//                          des flottants et des chaînes de caractères.

//        Limites : PlayerPrefs n'est pas conçu pour stocker de grandes quantités de données ou des types de données complexes.
//                  Pour des besoins plus avancés, il est recommandé d'utiliser des solutions de sérialisation ou des bases de données.


//**************************************************************************************************
//En cours de réflexion ==>> Solution a prévoir pour optimiser les sauvegardes des données en local
//**************************************************************************************************

// 1. un fichier JSON crypté. Utiliser des bibliothèques comme Newtonsoft.Json pour la gestion JSON et System.Security.Cryptography
// pour le cryptage en C#.

// 2. SQLite : Une base de données légère qui peut être intégrée directement dans votre application. Elle est idéale pour des
// données relationnelles et offre des fonctionnalités de sécurité comme le cryptage.
