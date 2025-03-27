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
    // Instance unique du gestionnaire de donn�es Firebase
    public static LoadAndSaveDataFirebase instance;

    // R�f�rence � la collection Firestore
    private CollectionReference dbReference;

    // Instance d'authentification Firebase
    private FirebaseAuth auth;

    // Utilisateur Firebase actuel
    private FirebaseUser user;

    private void Awake()
    {
        // Impl�mentation du pattern Singleton pour garantir une seule instance
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // D�truire l'objet s'il existe d�j� pour �viter les duplications
            Destroy(gameObject);
            return;
        }

        // V�rification et correction des d�pendances Firebase
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            if (task.Result == DependencyStatus.Available)
            {
                // Initialisation de l'application Firebase
                FirebaseApp app = FirebaseApp.DefaultInstance;

                // R�f�rence � la collection Firestore
                dbReference = FirebaseFirestore.DefaultInstance.Collection("users");

                // Initialisation de l'authentification Firebase
                auth = FirebaseAuth.DefaultInstance;

                // Connexion anonyme � Firebase
                auth.SignInAnonymouslyAsync().ContinueWith(authTask =>
                {
                    if (authTask.IsCompletedSuccessfully)
                    {
                        // R�cup�ration de l'utilisateur actuel
                        user = auth.CurrentUser;
                        Debug.Log("Connect� � Firebase avec l'ID: " + user.UserId);
                    }
                    else
                    {
                        // Log d'erreur en cas d'�chec de la connexion
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
    /// Sauvegarde les donn�es du jeu sur Firestore
    /// </summary>
    public async Task SaveDataToFirebase(SaveData saveData)
    {
        // V�rification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connect�.");
            return;
        }

        Debug.Log($"Tentative de sauvegarde pour l'utilisateur: {user.UserId}");
        Debug.Log($"Donn�es � sauvegarder: Pi�ces={saveData.CoinsCount}, Niveau={saveData.LevelReached}");

        try
        {         
            // Cr�ation d'un dictionnaire pour stocker les donn�es
            Dictionary<string, object> docData = new Dictionary<string, object>
        {
            { "CoinsCount", saveData.CoinsCount },
            { "LevelReached", saveData.LevelReached },
            { "InventoryItems", saveData.InventoryItems },
            { "InventoryItemsName", saveData.InventoryItemsName },
            { "LastModified", Timestamp.FromDateTime(saveData.LastModified.ToUniversalTime()) }
        };

        // Sauvegarde des donn�es dans Firestore sous le document de l'utilisateur
        DocumentReference docRef = dbReference.Document(user.UserId);
            await docRef.SetAsync(docData);
            Debug.Log("Donn�es sauvegard�es sur Firestore avec succ�s!");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors de la sauvegarde sur Firestore: {ex.Message}");
        }
    }

    /// <summary>
    /// Charge les donn�es du jeu depuis Firestore et retourne une t�che avec les donn�es.
    /// </summary>
    public async Task<SaveData> LoadDataFromFirebaseAsync()
    {
        // V�rification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connect�.");
            return null;
        }

        try
        {
        // R�f�rence au document de l'utilisateur
        DocumentReference docRef = dbReference.Document(user.UserId);

        // R�cup�ration des donn�es depuis Firestore
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

                Debug.Log("Donn�es charg�es depuis Firestore avec succ�s!");
            return saveData;
        }
        else
        {
            // Log d'avertissement si aucune donn�e n'est trouv�e
            Debug.LogWarning("Aucune donn�e trouv�e sur Firestore.");
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
