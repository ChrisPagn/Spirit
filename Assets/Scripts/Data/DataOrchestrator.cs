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

    /// <summary>
    /// Initialise l'instance unique du DataOrchestrator et les gestionnaires de sauvegarde.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            Debug.Log("DataOrchestrator initialisé.");

            // Initialisation des gestionnaires de sauvegarde
            _firebaseManager = FindObjectOfType<LoadAndSaveDataFirebase>();
            _localManager = FindObjectOfType<LoadAndSaveData>();

            if (_firebaseManager == null || _localManager == null)
            {
                Debug.LogError("Erreur : LoadAndSaveDataFirebase ou LoadAndSaveData n'ont pas été trouvés dans la scène !");
            }
        }
        else
        {
            Destroy(gameObject);
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
            UserId = FirebaseEmailAuthentication.instance.idUser,
            DisplayName = FirebaseEmailAuthentication.instance.displayNameInputField.text,
            CoinsCount = Inventory.instance.coinsCount,
            LevelReached = CurrentSceneManager.instance.levelToUnlock,
            InventoryItems = Inventory.instance.contentItems.ConvertAll(item => item.id),
            InventoryItemsName = Inventory.instance.contentItems.ConvertAll(item => item.name),
            LastModified = DateTime.UtcNow
        };

        // Sauvegarde des données localement
        _localManager.SaveDataToLocal();

        // Sauvegarde des données sur Firebase
        await _firebaseManager.SaveDataToFirebase(saveData);
    }

    /// <summary>
    /// Charge les données du jeu depuis la sauvegarde locale et Firebase, puis applique la version la plus récente.
    /// </summary>
    public async Task LoadData()
    {
        // Chargement des données locales
        SaveData localData = await Task.Run(() => _localManager.LoadData());

        // Chargement des données Firebase
        SaveData firebaseData = await _firebaseManager.LoadDataFromFirebaseAsync();

        // Fusion des données pour obtenir la version la plus récente
        SaveData finalData = MergeSaveData(localData, firebaseData);

        // Application des données chargées
        ApplyData(finalData);
    }

    /// <summary>
    /// Fusionne les données locales et celles de Firebase en gardant la plus récente.
    /// </summary>
    /// <param name="localData">Données locales.</param>
    /// <param name="firebaseData">Données Firebase.</param>
    /// <returns>Les données les plus récentes entre localData et firebaseData.</returns>
    private SaveData MergeSaveData(SaveData localData, SaveData firebaseData)
    {
        // Si les données locales sont nulles, retourner les données Firebase
        if (localData == null) return firebaseData;

        // Si les données Firebase sont nulles, retourner les données locales
        if (firebaseData == null) return localData;

        // Retourner les données les plus récentes
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

            // Mise à jour du displayName si disponible
            if (!string.IsNullOrEmpty(saveData.DisplayName) &&
                FirebaseEmailAuthentication.instance != null &&
                FirebaseEmailAuthentication.instance.displayNameInputField != null)
            {
                FirebaseEmailAuthentication.instance.displayNameInputField.text = saveData.DisplayName;
            }
        }
    }
}
