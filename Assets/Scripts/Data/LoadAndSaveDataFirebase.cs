using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

public class LoadAndSaveDataFirebase : MonoBehaviour
{
    // Instance unique du gestionnaire de données Firebase
    public static LoadAndSaveDataFirebase instance;

    // Référence à la collection Firestore
    private CollectionReference dbReference;

    // Instance d'authentification Firebase
    private FirebaseAuth auth;

    // Utilisateur Firebase actuel
    private FirebaseUser user;

    private void Awake()
    {
        // Implémentation du pattern Singleton pour garantir une seule instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // Détruire l'objet s'il existe déjà pour éviter les duplications
            Destroy(gameObject);
            return;
        }

        // Vérification et correction des dépendances Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialisation de l'application Firebase
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // Référence à la collection Firestore
                dbReference = FirebaseFirestore.DefaultInstance.Collection("users");

                // Initialisation de l'authentification Firebase
                auth = FirebaseAuth.DefaultInstance;

                // Connexion anonyme à Firebase
                auth.SignInAnonymouslyAsync().ContinueWith(authTask =>
                {
                    if (authTask.IsCompletedSuccessfully)
                    {
                        // Récupération de l'utilisateur actuel
                        user = auth.CurrentUser;
                        Debug.Log("Connecté à Firebase avec l'ID: " + user.UserId);
                    }
                    else
                    {
                        // Log d'erreur en cas d'échec de la connexion
                        Debug.LogError("Erreur connexion Firebase: " + authTask.Exception);
                    }
                });
            }
            else
            {
                // Log d'erreur si Firebase n'est pas disponible
                Debug.LogError("Firebase non disponible : " + task.Result);
            }
        });
    }

    /// <summary>
    /// Sauvegarde les données du jeu sur Firestore
    /// </summary>
    public async Task SaveDataToFirebase(SaveData saveData)
    {
        // Vérification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connecté.");
            return;
        }

        Debug.Log($"Tentative de sauvegarde pour l'utilisateur: {user.UserId}");
        Debug.Log($"Données à sauvegarder: Pièces={saveData.CoinsCount}, Niveau={saveData.LevelReached}");

        try
        {         
            // Création d'un dictionnaire pour stocker les données
            Dictionary<string, object> docData = new Dictionary<string, object>
        {
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
    /// Charge les données du jeu depuis Firestore et retourne une tâche avec les données.
    /// </summary>
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
                Dictionary<string, object> docData = snapshot.ToDictionary();

                SaveData saveData = new SaveData
                {
                    CoinsCount = Convert.ToInt32(docData["CoinsCount"]),
                    LevelReached = Convert.ToInt32(docData["LevelReached"]),
                    LastModified = ((Timestamp)docData["LastModified"]).ToDateTime()
                };

                // Conversion des InventoryItems
                if (docData.ContainsKey("InventoryItems") && docData["InventoryItems"] is IEnumerable<object> items)
                {
                    saveData.InventoryItems = items.Select(item => Convert.ToInt32(item)).ToList();
                }
                else
                {
                    saveData.InventoryItems = new List<int>();
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
