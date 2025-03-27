using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UnityEngine;

/// <summary>
/// Orchestrateur de gestion des données du jeu. 
/// Gère la sauvegarde et le chargement des données en local et sur Firebase.
/// </summary>
public class DataOrchestrator : MonoBehaviour
{
    /// <summary>
    /// Instance unique du DataOrchestrator (Singleton).
    /// </summary>
    public static DataOrchestrator instance { get; private set; }

    private LoadAndSaveDataFirebase _firebaseManager;
    private LoadAndSaveData _localManager;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DataOrchestrator initialisé.");
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        // Initialisation des gestionnaires de sauvegarde
        _firebaseManager = FindObjectOfType<LoadAndSaveDataFirebase>();
        _localManager = FindObjectOfType<LoadAndSaveData>();

        if (_firebaseManager == null || _localManager == null)
        {
            Debug.LogError("Erreur : LoadAndSaveDataFirebase ou LoadAndSaveData n'ont pas été trouvés dans la scène !");
        }
    }

    /// <summary>
    /// Sauvegarde les données du jeu à la fois en local et sur Firebase.
    /// </summary>
    public async Task SaveData()
    {
        // Création de l'objet SaveData avant de l'envoyer à Firebase
        var saveData = new SaveData
        {
            CoinsCount = Inventory.instance.coinsCount,
            LevelReached = CurrentSceneManager.instance.levelToUnlock,
            InventoryItems = Inventory.instance.contentItems.ConvertAll(item => item.id),
            InventoryItemsName = Inventory.instance.contentItems.ConvertAll(item => item.name),
            LastModified = DateTime.UtcNow
        };

        _localManager.SaveDataToLocal(); // Sauvegarde locale
        await _firebaseManager.SaveDataToFirebase(saveData); // Sauvegarde en ligne
    }

    /// <summary>
    /// Charge les données du jeu depuis la sauvegarde locale et Firebase, puis applique la version la plus récente.
    /// </summary>
    public async Task LoadData()
    {
        SaveData localData = await Task.Run(() => _localManager.LoadData()); // Chargement des données locales
        SaveData firebaseData = await _firebaseManager.LoadDataFromFirebaseAsync(); // Chargement des données Firebase
        SaveData finalData = MergeSaveData(localData, firebaseData); // Fusion des données
        ApplyData(finalData); // Application des données
    }

    /// <summary>
    /// Fusionne les données locales et celles de Firebase en gardant la plus récente.
    /// </summary>
    /// <param name="localData">Données locales.</param>
    /// <param name="firebaseData">Données Firebase.</param>
    /// <returns>Les données les plus récentes entre localData et firebaseData.</returns>
    private SaveData MergeSaveData(SaveData localData, SaveData firebaseData)
    {
        if (localData == null) return firebaseData;
        if (firebaseData == null) return localData;
        return (firebaseData.LastModified > localData.LastModified) ? firebaseData : localData;
    }

    /// <summary>
    /// Applique les données chargées dans les systèmes de jeu.
    /// </summary>
    /// <param name="saveData">Les données à appliquer.</param>
    private void ApplyData(SaveData saveData)
    {
        if (saveData != null)
        {
            // Mise à jour des pièces du joueur
            Inventory.instance.coinsCount = saveData.CoinsCount;
            Inventory.instance.UpdateTextUI();

            // Mise à jour de l'inventaire
            Inventory.instance.contentItems.Clear();
            foreach (int id in saveData.InventoryItems)
            {
                Item currentItem = ItemsDataBase.instance.allItems.FirstOrDefault(x => x.id == id);
                if (currentItem != null)
                {
                    Inventory.instance.contentItems.Add(currentItem);
                }
            }

            Inventory.instance.UpdateInventoryUI();

            // Mise à jour du niveau débloqué
            CurrentSceneManager.instance.levelToUnlock = saveData.LevelReached;
        }
    }
}
