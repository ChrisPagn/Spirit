using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;
using System.Collections;
using UnityEngine.Networking;

/// <summary>
/// Classe responsable de la sauvegarde et du chargement des données de jeu.
/// </summary>
public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData instance;
    private string filePath;
    private string encryptionKey;

    /// <summary>
    /// Initialise l'instance singleton du gestionnaire de sauvegarde et définit le chemin du fichier de sauvegarde.
    /// Cette méthode charge également la clé de chiffrement en lançant une coroutine pour lire le fichier de configuration.
    /// </summary>
    private void Awake()
    {
        // Vérifie si une instance existe déjà, sinon crée une nouvelle instance unique (Singleton)
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // Détruit l'objet en double pour éviter les conflits
            return;
        }

        instance = this; // Définit l'instance unique du script
        DontDestroyOnLoad(gameObject); // Conserve l'objet entre les scènes

        // Définit le chemin du fichier de sauvegarde qui stocke les données du joueur
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("Chemin du fichier de sauvegarde : " + filePath);

        // Lance la coroutine pour charger la clé de chiffrement depuis config.json
        StartCoroutine(LoadEncryptionKey());
    }

    /// <summary>
    /// Sauvegarde les données actuelles du jeu dans un fichier local.
    /// </summary>
    public void SaveDataToLocal()
    {
        var saveData = new SaveData
        {
            UserId = FirebaseEmailAuthentication.instance.idUser, // Ajoutez l'ID utilisateur
            DisplayName = FirebaseEmailAuthentication.instance.displayNameInputField.text,
            CoinsCount = Inventory.instance.coinsCount,
            LevelReached = CurrentSceneManager.instance.levelToUnlock,
            InventoryItems = Inventory.instance.contentItems.ConvertAll(item => item.id),
            InventoryItemsName = Inventory.instance.contentItems.ConvertAll(item => item.name),
            LastModified = DateTime.UtcNow
        };

        try
        {
            string json = JsonConvert.SerializeObject(saveData);
            string encryptedJson = EncryptString(json, encryptionKey);
            File.WriteAllText(filePath, encryptedJson);
            Debug.Log("Données sauvegardées localement avec succès !");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors de la sauvegarde des données : {ex.Message}");
        }
    }


    /// <summary>
    /// Charge les données sauvegardées à partir du fichier local.
    /// </summary>
    /// <returns>Les données de sauvegarde chargées, ou null si le chargement échoue.</returns>
    public SaveData LoadData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string encryptedJson = File.ReadAllText(filePath);
                string json = DecryptString(encryptedJson, encryptionKey);
                var saveData = JsonConvert.DeserializeObject<SaveData>(json);

                // Vérifiez que l'ID utilisateur correspond à celui de l'utilisateur actuel
                if (saveData.UserId == FirebaseEmailAuthentication.instance.idUser)
                {
                    Debug.Log("Données locales chargées avec succès !");
                    return saveData;
                }
                else
                {
                    Debug.LogWarning("L'ID utilisateur ne correspond pas. Les données locales ne seront pas chargées.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erreur chargement local : {ex.Message}");
            }
        }
        return null;
    }

    /// <summary>
    /// Charge la clé de chiffrement à partir d'un fichier de configuration JSON situé dans le dossier StreamingAssets.
    /// Cette méthode est exécutée en coroutine car elle utilise UnityWebRequest, ce qui est nécessaire pour Android et WebGL.
    /// </summary>
    /// <returns>Une coroutine qui charge la clé de chiffrement.</returns>
    private IEnumerator LoadEncryptionKey()
    {
        // Détermine le chemin du fichier config.json dans StreamingAssets.
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
        Debug.Log("Chemin du fichier config.json : " + configPath);

        // Utilisation de UnityWebRequest pour lire le fichier (obligatoire pour Android & WebGL)
        using (UnityWebRequest www = UnityWebRequest.Get(configPath))
        {
            yield return www.SendWebRequest(); // Attend la fin du téléchargement

            // Vérifie si une erreur s'est produite pendant la requête HTTP
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la lecture du fichier config.json : " + www.error);
                yield break; // Arrête la coroutine si le fichier ne peut pas être chargé
            }

            // Récupère le contenu JSON du fichier
            string json = www.downloadHandler.text;
            Debug.Log("Contenu de config.json : " + json);

            // Désérialise le JSON en un dictionnaire clé/valeur
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // Vérifie si le JSON contient bien la clé "encryptionKey"
            if (config != null && config.TryGetValue("encryptionKey", out string key))
            {
                encryptionKey = key; // Stocke la clé de chiffrement pour une utilisation ultérieure
                Debug.Log("Clé de chiffrement chargée avec succès !");
            }
            else
            {
                Debug.LogError("Clé de chiffrement absente du fichier config.json !");
            }
        }
    }

    /// <summary>
    /// Chiffre une chaîne de texte en utilisant une clé de chiffrement.
    /// </summary>
    /// <param name="plainText">Texte à chiffrer.</param>
    /// <param name="key">Clé de chiffrement.</param>
    /// <returns>Texte chiffré.</returns>
    private string EncryptString(string plainText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            // Utilise un sel pour générer une clé sécurisée.
            byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
            aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);

            // Génère un vecteur d'initialisation (IV) aléatoire.
            aes.GenerateIV();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // Écrit l'IV au début du flux.
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                // Chiffre le texte et l'écrit dans le flux.
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                // Retourne le texte chiffré sous forme de chaîne Base64.
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    /// <summary>
    /// Déchiffre une chaîne de texte en utilisant une clé de déchiffrement.
    /// </summary>
    /// <param name="cipherText">Texte chiffré.</param>
    /// <param name="key">Clé de déchiffrement.</param>
    /// <returns>Texte déchiffré.</returns>
    private string DecryptString(string cipherText, string key)
    {
        try
        {
            // Convertit le texte chiffré de Base64 en tableau d'octets.
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
                aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);

                // Extrait l'IV et le texte chiffré du tableau d'octets.
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.IV = iv;

                // Déchiffre le texte et le lit dans un flux.
                using (MemoryStream memoryStream = new MemoryStream(cipher))
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateDecryptor(), CryptoStreamMode.Read))
                using (StreamReader streamReader = new StreamReader(cryptoStream))
                {
                    return streamReader.ReadToEnd();
                }
            }
        }
        catch (Exception ex)
        {
            // Affiche une erreur si le déchiffrement échoue.
            Debug.LogError($"Erreur de déchiffrement : {ex.Message}");
            return null;
        }
    }
}




#region Aide-mémoire

/*
*********************
Aide mémoire
*********************

PlayerPrefs sauvegarde les données dans un fichier de registre sur Windows ou un fichier .plist sur macOS.
Ces fichiers sont situés dans un répertoire spécifique à l'application sur le disque de l'utilisateur.

Fonctionnement de PlayerPrefs:
- Stockage Local : PlayerPrefs stocke les données en local sur le disque de l'utilisateur.
                  Sur Windows, les données sont stockées dans le registre,
                  (cmd :Windows + r ==>> taper " regedit ").
                  Tandis que sur macOS, elles sont stockées dans un fichier .plist.
- Persistance: Les données sauvegardées avec PlayerPrefs persistent entre les sessions de jeu,
               ce qui signifie qu'elles ne sont pas perdues lorsque le jeu est fermé.
- Types de Données: PlayerPrefs peut stocker des types de données simples comme des entiers,
                    des flottants et des chaînes de caractères.
- Limites : PlayerPrefs n'est pas conçu pour stocker de grandes quantités de données ou des types de données complexes.
            Pour des besoins plus avancés, il est recommandé d'utiliser des solutions de sérialisation ou des bases de données.

**************************************************************************************************
En cours de réflexion ==>> Solution à prévoir pour optimiser les sauvegardes des données en local
**************************************************************************************************

1. Un fichier JSON crypté. Utiliser des bibliothèques comme Newtonsoft.Json pour la gestion JSON et System.Security.Cryptography
   pour le cryptage en C#.

2. SQLite : Une base de données légère qui peut être intégrée directement dans votre application. Elle est idéale pour des
   données relationnelles et offre des fonctionnalités de sécurité comme le cryptage.
*/

#endregion
