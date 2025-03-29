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
/// Classe responsable de la sauvegarde et du chargement des donn�es de jeu.
/// </summary>
public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData instance;
    private string filePath;
    private string encryptionKey;

    /// <summary>
    /// Initialise l'instance singleton du gestionnaire de sauvegarde et d�finit le chemin du fichier de sauvegarde.
    /// Cette m�thode charge �galement la cl� de chiffrement en lan�ant une coroutine pour lire le fichier de configuration.
    /// </summary>
    private void Awake()
    {
        // V�rifie si une instance existe d�j�, sinon cr�e une nouvelle instance unique (Singleton)
        if (instance != null && instance != this)
        {
            Destroy(gameObject); // D�truit l'objet en double pour �viter les conflits
            return;
        }

        instance = this; // D�finit l'instance unique du script
        DontDestroyOnLoad(gameObject); // Conserve l'objet entre les sc�nes

        // D�finit le chemin du fichier de sauvegarde qui stocke les donn�es du joueur
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");
        Debug.Log("Chemin du fichier de sauvegarde : " + filePath);

        // Lance la coroutine pour charger la cl� de chiffrement depuis config.json
        StartCoroutine(LoadEncryptionKey());
    }

    /// <summary>
    /// Sauvegarde les donn�es actuelles du jeu dans un fichier local.
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
            Debug.Log("Donn�es sauvegard�es localement avec succ�s !");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors de la sauvegarde des donn�es : {ex.Message}");
        }
    }


    /// <summary>
    /// Charge les donn�es sauvegard�es � partir du fichier local.
    /// </summary>
    /// <returns>Les donn�es de sauvegarde charg�es, ou null si le chargement �choue.</returns>
    public SaveData LoadData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string encryptedJson = File.ReadAllText(filePath);
                string json = DecryptString(encryptedJson, encryptionKey);
                var saveData = JsonConvert.DeserializeObject<SaveData>(json);

                // V�rifiez que l'ID utilisateur correspond � celui de l'utilisateur actuel
                if (saveData.UserId == FirebaseEmailAuthentication.instance.idUser)
                {
                    Debug.Log("Donn�es locales charg�es avec succ�s !");
                    return saveData;
                }
                else
                {
                    Debug.LogWarning("L'ID utilisateur ne correspond pas. Les donn�es locales ne seront pas charg�es.");
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
    /// Charge la cl� de chiffrement � partir d'un fichier de configuration JSON situ� dans le dossier StreamingAssets.
    /// Cette m�thode est ex�cut�e en coroutine car elle utilise UnityWebRequest, ce qui est n�cessaire pour Android et WebGL.
    /// </summary>
    /// <returns>Une coroutine qui charge la cl� de chiffrement.</returns>
    private IEnumerator LoadEncryptionKey()
    {
        // D�termine le chemin du fichier config.json dans StreamingAssets.
        string configPath = Path.Combine(Application.streamingAssetsPath, "config.json");
        Debug.Log("Chemin du fichier config.json : " + configPath);

        // Utilisation de UnityWebRequest pour lire le fichier (obligatoire pour Android & WebGL)
        using (UnityWebRequest www = UnityWebRequest.Get(configPath))
        {
            yield return www.SendWebRequest(); // Attend la fin du t�l�chargement

            // V�rifie si une erreur s'est produite pendant la requ�te HTTP
            if (www.result != UnityWebRequest.Result.Success)
            {
                Debug.LogError("Erreur lors de la lecture du fichier config.json : " + www.error);
                yield break; // Arr�te la coroutine si le fichier ne peut pas �tre charg�
            }

            // R�cup�re le contenu JSON du fichier
            string json = www.downloadHandler.text;
            Debug.Log("Contenu de config.json : " + json);

            // D�s�rialise le JSON en un dictionnaire cl�/valeur
            var config = JsonConvert.DeserializeObject<Dictionary<string, string>>(json);

            // V�rifie si le JSON contient bien la cl� "encryptionKey"
            if (config != null && config.TryGetValue("encryptionKey", out string key))
            {
                encryptionKey = key; // Stocke la cl� de chiffrement pour une utilisation ult�rieure
                Debug.Log("Cl� de chiffrement charg�e avec succ�s !");
            }
            else
            {
                Debug.LogError("Cl� de chiffrement absente du fichier config.json !");
            }
        }
    }

    /// <summary>
    /// Chiffre une cha�ne de texte en utilisant une cl� de chiffrement.
    /// </summary>
    /// <param name="plainText">Texte � chiffrer.</param>
    /// <param name="key">Cl� de chiffrement.</param>
    /// <returns>Texte chiffr�.</returns>
    private string EncryptString(string plainText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            // Utilise un sel pour g�n�rer une cl� s�curis�e.
            byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
            aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);

            // G�n�re un vecteur d'initialisation (IV) al�atoire.
            aes.GenerateIV();

            using (MemoryStream memoryStream = new MemoryStream())
            {
                // �crit l'IV au d�but du flux.
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                // Chiffre le texte et l'�crit dans le flux.
                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                // Retourne le texte chiffr� sous forme de cha�ne Base64.
                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    /// <summary>
    /// D�chiffre une cha�ne de texte en utilisant une cl� de d�chiffrement.
    /// </summary>
    /// <param name="cipherText">Texte chiffr�.</param>
    /// <param name="key">Cl� de d�chiffrement.</param>
    /// <returns>Texte d�chiffr�.</returns>
    private string DecryptString(string cipherText, string key)
    {
        try
        {
            // Convertit le texte chiffr� de Base64 en tableau d'octets.
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
                aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);

                // Extrait l'IV et le texte chiffr� du tableau d'octets.
                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.IV = iv;

                // D�chiffre le texte et le lit dans un flux.
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
            // Affiche une erreur si le d�chiffrement �choue.
            Debug.LogError($"Erreur de d�chiffrement : {ex.Message}");
            return null;
        }
    }
}




#region Aide-m�moire

/*
*********************
Aide m�moire
*********************

PlayerPrefs sauvegarde les donn�es dans un fichier de registre sur Windows ou un fichier .plist sur macOS.
Ces fichiers sont situ�s dans un r�pertoire sp�cifique � l'application sur le disque de l'utilisateur.

Fonctionnement de PlayerPrefs:
- Stockage Local : PlayerPrefs stocke les donn�es en local sur le disque de l'utilisateur.
                  Sur Windows, les donn�es sont stock�es dans le registre,
                  (cmd :Windows + r ==>> taper " regedit ").
                  Tandis que sur macOS, elles sont stock�es dans un fichier .plist.
- Persistance: Les donn�es sauvegard�es avec PlayerPrefs persistent entre les sessions de jeu,
               ce qui signifie qu'elles ne sont pas perdues lorsque le jeu est ferm�.
- Types de Donn�es: PlayerPrefs peut stocker des types de donn�es simples comme des entiers,
                    des flottants et des cha�nes de caract�res.
- Limites : PlayerPrefs n'est pas con�u pour stocker de grandes quantit�s de donn�es ou des types de donn�es complexes.
            Pour des besoins plus avanc�s, il est recommand� d'utiliser des solutions de s�rialisation ou des bases de donn�es.

**************************************************************************************************
En cours de r�flexion ==>> Solution � pr�voir pour optimiser les sauvegardes des donn�es en local
**************************************************************************************************

1. Un fichier JSON crypt�. Utiliser des biblioth�ques comme Newtonsoft.Json pour la gestion JSON et System.Security.Cryptography
   pour le cryptage en C#.

2. SQLite : Une base de donn�es l�g�re qui peut �tre int�gr�e directement dans votre application. Elle est id�ale pour des
   donn�es relationnelles et offre des fonctionnalit�s de s�curit� comme le cryptage.
*/

#endregion
