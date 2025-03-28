using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Gestionnaire de sauvegarde et de chargement des données sur Firebase Firestore.
/// Gère l'authentification anonyme de l'utilisateur et la synchronisation des données.
/// </summary>
public class LoadAndSaveDataFirebase : MonoBehaviour
{
    /// <summary>
    /// Instance unique du LoadAndSaveDataFirebase (Singleton).
    /// </summary>
    public static LoadAndSaveDataFirebase instance;

    /// <summary>
    /// Référence à la collection Firestore contenant les données des utilisateurs.
    /// </summary>
    private CollectionReference dbReference;

    /// <summary>
    /// Instance d'authentification Firebase.
    /// </summary>
    private FirebaseAuth auth;

    /// <summary>
    /// Utilisateur Firebase actuellement authentifié.
    /// </summary>
    private FirebaseUser user;

    /// <summary>
    /// Initialise l'instance singleton et configure Firebase.
    /// </summary>
    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                FirebaseApp app = FirebaseApp.DefaultInstance;
                dbReference = FirebaseFirestore.DefaultInstance.Collection("users");
                auth = FirebaseAuth.DefaultInstance;

                // Ne pas authentifier anonymement ici
                user = auth.CurrentUser;
                if (user != null)
                {
                    Debug.Log("Connecté à Firebase avec l'ID: " + user.UserId);
                }
                else
                {
                    Debug.LogWarning("Aucun utilisateur connecté.");
                }
            }
            else
            {
                Debug.LogError("Firebase non disponible : " + task.Result);
            }
        });
    }

    /// <summary>
    /// Sauvegarde les données de l'utilisateur sur Firestore.
    /// </summary>
    /// <param name="saveData">Les données à sauvegarder.</param>
    public async Task SaveDataToFirebase(SaveData saveData)
    {
        // Vérification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connecté.");
            return;
        }

        try
        {
            // Création d'un dictionnaire pour stocker les données
            var docData = new Dictionary<string, object>
            {
                { "DisplayName", saveData.DisplayName },
                { "CoinsCount", saveData.CoinsCount },
                { "LevelReached", saveData.LevelReached },
                { "InventoryItems", saveData.InventoryItems },
                { "InventoryItemsName", saveData.InventoryItemsName },
                { "LastModified", Timestamp.FromDateTime(saveData.LastModified.ToUniversalTime()) }
            };

            // Sauvegarde des données dans Firestore sous le document de l'utilisateur
            DocumentReference docRef = dbReference.Document(user.UserId);
            await docRef.SetAsync(docData);
            Debug.Log("Données sauvegardées sur Firestore avec succès!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors de la sauvegarde sur Firestore: {ex.Message}");
        }
    }

    /// <summary>
    /// Charge les données de l'utilisateur depuis Firestore.
    /// </summary>
    /// <returns>Les données chargées ou null si aucune donnée trouvée.</returns>
    public async Task<SaveData> LoadDataFromFirebaseAsync()
    {
        // Vérification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connecté.");
            return null;
        }

        try
        {
            // Référence au document de l'utilisateur
            DocumentReference docRef = dbReference.Document(user.UserId);

            // Récupération des données depuis Firestore
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();
            if (snapshot.Exists)
            {
                var docData = snapshot.ToDictionary();

                // Création d'un objet SaveData à partir des données récupérées
                var saveData = new SaveData
                {
                    DisplayName = Convert.ToString(docData["DisplayName"]),
                    CoinsCount = Convert.ToInt32(docData["CoinsCount"]),
                    LevelReached = Convert.ToInt32(docData["LevelReached"]),
                    LastModified = ((Timestamp)docData["LastModified"]).ToDateTime()
                };

                // Conversion des InventoryItems
                if (docData.ContainsKey("InventoryItems") && docData["InventoryItems"] is IEnumerable<object> items)
                {
                    saveData.InventoryItems = items.Select(item => Convert.ToInt32(item)).ToList();
                }

                Debug.Log("Données chargées depuis Firestore avec succès!");
                return saveData;
            }
            else
            {
                // Log d'avertissement si aucune donnée n'est trouvée
                Debug.LogWarning("Aucune donnée trouvée sur Firestore.");
                return null;
            }
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors du chargement depuis Firestore: {ex.Message}");
            return null;
        }
    }
}
