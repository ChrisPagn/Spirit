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
    public SaveData saveData;
    /// <summary>
    /// Instance unique du DataOrchestrator (Singleton).
    /// </summary>
    public static DataOrchestrator instance { get; private set; }


    /// <summary>
    /// Initialise l'instance unique du DataOrchestrator et les gestionnaires de sauvegarde.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            //DontDestroyOnLoad(gameObject);
            Debug.Log("DataOrchestrator initialisé.");

            //// Initialisation des gestionnaires de sauvegarde
            //_firebaseManager = FindObjectOfType<LoadAndSaveDataFirebase>();
            //_localManager = FindObjectOfType<LoadAndSaveData>();

            //if (_firebaseManager == null || _localManager == null)
            //{
            //    Debug.LogError("Erreur : LoadAndSaveDataFirebase ou LoadAndSaveData n'ont pas été trouvés dans la scène !");
            //}
        }
    }

    /// <summary>
    /// Sauvegarde les données du jeu à la fois en local et sur Firebase.
    /// </summary>
    public async Task SaveData()
    {
        // Création de l'objet SaveData avant de l'envoyer à Firebase
        var saveData = new SaveData(
          FirebaseEmailAuthentication.instance.idUser, // Ajoutez l'ID utilisateur
          FirebaseEmailAuthentication.instance.displayNameInputField.text,
          Inventory.instance.coinsCount,
          CurrentSceneManager.instance.levelToUnlock,
          Inventory.instance.contentItems.ConvertAll(item => item.id),
          Inventory.instance.contentItems.ConvertAll(item => item.name),
          DateTime.UtcNow,
          Inventory.instance.lastLevelPlayed  == null ? "level01" : Inventory.instance.lastLevelPlayed
          );
        // Sauvegarde des données localement
        LoadAndSaveData.instance.SaveDataToLocal();

        // Sauvegarde des données sur Firebase
        await LoadAndSaveDataFirebase.instance.SaveDataToFirebase(saveData);
        //await _firebaseManager.SaveDataToFirebase(saveData);
    }

    /// <summary>
    /// Charge les données du jeu depuis la sauvegarde locale et Firebase, puis applique la version la plus récente.
    /// </summary>
    public async Task LoadData()
    {
        // Attendre que Firebase soit bien initialisé
        await LoadAndSaveDataFirebase.instance.Init();

        // Chargement des données locales
        SaveData localData = await Task.Run(() => LoadAndSaveData.instance.LoadData());

        // Chargement des données Firebase
        SaveData firebaseData = await LoadAndSaveDataFirebase.instance.LoadDataFromFirebaseAsync();

        Debug.Log(firebaseData.CoinsCount + " " + firebaseData.CoinsCount);

        // Fusion des données pour obtenir la version la plus récente
        SaveData finalData = MergeSaveData(localData, firebaseData);

        Debug.Log(finalData.CoinsCount + " " + finalData.CoinsCount);

        GameManager.instance.OnLoadLevel(finalData.LastLevelPlayed,finalData);

        // Application des données chargées sur l'ui
        //ApplyDataOnUI(finalData);
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
    public void ApplyDataOnUI(SaveData saveData)
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
