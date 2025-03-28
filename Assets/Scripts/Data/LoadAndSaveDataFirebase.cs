using Firebase;
using Firebase.Firestore;
using Firebase.Auth;
using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

/// <summary>
/// Gestionnaire de sauvegarde et de chargement des donn�es sur Firebase Firestore.
/// G�re l'authentification anonyme de l'utilisateur et la synchronisation des donn�es.
/// </summary>
public class LoadAndSaveDataFirebase : MonoBehaviour
{
    /// <summary>
    /// Instance unique du LoadAndSaveDataFirebase (Singleton).
    /// </summary>
    public static LoadAndSaveDataFirebase instance;

    /// <summary>
    /// R�f�rence � la collection Firestore contenant les donn�es des utilisateurs.
    /// </summary>
    private CollectionReference dbReference;

    /// <summary>
    /// Instance d'authentification Firebase.
    /// </summary>
    private FirebaseAuth auth;

    /// <summary>
    /// Utilisateur Firebase actuellement authentifi�.
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
                    Debug.Log("Connect� � Firebase avec l'ID: " + user.UserId);
                }
                else
                {
                    Debug.LogWarning("Aucun utilisateur connect�.");
                }
            }
            else
            {
                Debug.LogError("Firebase non disponible : " + task.Result);
            }
        });
    }

    /// <summary>
    /// Sauvegarde les donn�es de l'utilisateur sur Firestore.
    /// </summary>
    /// <param name="saveData">Les donn�es � sauvegarder.</param>
    public async Task SaveDataToFirebase(SaveData saveData)
    {
        // V�rification de la connexion de l'utilisateur
        if (user == null)
        {
            Debug.LogError("Utilisateur Firebase non connect�.");
            return;
        }

        try
        {
            // Cr�ation d'un dictionnaire pour stocker les donn�es
            var docData = new Dictionary<string, object>
            {
                { "DisplayName", saveData.DisplayName },
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
    /// Charge les donn�es de l'utilisateur depuis Firestore.
    /// </summary>
    /// <returns>Les donn�es charg�es ou null si aucune donn�e trouv�e.</returns>
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
                var docData = snapshot.ToDictionary();

                // Cr�ation d'un objet SaveData � partir des donn�es r�cup�r�es
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
