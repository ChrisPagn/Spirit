using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

/// <summary>
/// Classe responsable de la sauvegarde et du chargement des donn�es de jeu.
/// </summary>
public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData instance;

    private string filePath;
    private string encryptionKey = "YourEncryptionKeyHere"; // Remplacez par une cl� s�curis�e

    /// <summary>
    /// Initialise l'instance singleton et d�finit le chemin du fichier de sauvegarde.
    /// </summary>
    private void Awake()
    {
        // V�rifie s'il existe d�j� une instance de cette classe.
        if (instance != null && instance != this)
        {
            // Si une instance existe d�j�, d�truit l'objet actuel pour �viter les duplications.
            Destroy(gameObject);
            return;
        }

        // D�finit l'instance actuelle comme �tant l'instance unique.
        instance = this;

        // D�finit le chemin o� le fichier de sauvegarde sera stock�.
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Si on veut que l'objet persiste entre les sc�nes, d�commentez la ligne suivante :
        // DontDestroyOnLoad(gameObject);
    }

    /// <summary>
    /// Charge les donn�es sauvegard�es lorsque le jeu commence.
    /// </summary>
    void Start()
    {
        // Affiche le chemin du fichier de sauvegarde pour le d�bogage.
        // C:/Users/Chris/AppData/LocalLow/DefaultCompany/Spirit\saveData.json
        Debug.Log("Chemin du fichier de sauvegarde : " + filePath);

        // Charge les donn�es sauvegard�es.
        LoadData();
    }

    /// <summary>
    /// Sauvegarde les donn�es actuelles du jeu dans un fichier.
    /// </summary>
    public void SaveData()
    {
        // Cr�e un objet SaveData contenant les donn�es � sauvegarder.
        var saveData = new SaveData
        {
            CoinsCount = Inventory.instance.coinsCount,
            LevelReached = CurrentSceneManager.instance.levelToUnlock,
            InventoryItems = Inventory.instance.contentItems.ConvertAll(item => item.id)
        };

        try
        {
            // Convertit les donn�es en JSON.
            string json = JsonConvert.SerializeObject(saveData);

            // Chiffre le JSON.
            string encryptedJson = EncryptString(json, encryptionKey);

            // �crit le JSON chiffr� dans le fichier.
            File.WriteAllText(filePath, encryptedJson);
        }
        catch (Exception ex)
        {
            // Affiche une erreur si la sauvegarde �choue.
            Debug.LogError($"Erreur lors de la sauvegarde des donn�es : {ex.Message}");
        }
    }

    /// <summary>
    /// Charge les donn�es sauvegard�es � partir du fichier.
    /// </summary>
    private void LoadData()
    {
        // V�rifie si le fichier de sauvegarde existe.
        if (File.Exists(filePath))
        {
            try
            {
                // Lit le contenu chiffr� du fichier.
                string encryptedJson = File.ReadAllText(filePath);

                // D�chiffre le contenu.
                string json = DecryptString(encryptedJson, encryptionKey);

                // D�s�rialise le JSON en un objet SaveData.
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                // Si les donn�es sont valides, met � jour les composants du jeu.
                if (saveData != null)
                {
                    Inventory.instance.coinsCount = saveData.CoinsCount;
                    Inventory.instance.UpdateTextUI();

                    // Ajoute les �l�ments d'inventaire sauvegard�s.
                    foreach (int id in saveData.InventoryItems)
                    {
                        Item currentItem = ItemsDataBase.instance.allItems.FirstOrDefault(x => x.id == id);
                        if (currentItem != null)
                        {
                            Inventory.instance.contentItems.Add(currentItem);
                        }
                    }

                    Inventory.instance.UpdateInventoryUI();
                    CurrentSceneManager.instance.levelToUnlock = saveData.LevelReached;
                }
                else
                {
                    // Affiche un avertissement si les donn�es sont corrompues.
                    Debug.LogWarning("Les donn�es sauvegard�es sont corrompues ou invalides.");
                }
            }
            catch (Exception ex)
            {
                // Affiche une erreur si le chargement �choue.
                Debug.LogError($"Erreur lors du chargement des donn�es : {ex.Message}");
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


//*********************
//Aide m�moire
//*********************

// PlayerPrefs sauvegarde les donn�es dans un fichier de registre sur Windows ou un fichier .plist sur macOS
// Ces fichiers sont situ�s dans un r�pertoire sp�cifique � l'application sur le disque de l'utilisateur

//        Fonctionnement de PlayerPrefs:
//        Stockage Local : PlayerPrefs stocke les donn�es en local sur le disque de l'utilisateur.
//                          Sur Windows, les donn�es sont stock�es dans le registre,
//                              cmd :Windows + r ==>> taper " regedit "
//                          tandis que sur macOS, elles sont stock�es dans un fichier .plist.

//        Persistance: Les donn�es sauvegard�es avec PlayerPrefs persistent entre les sessions de jeu,
//                      ce qui signifie qu'elles ne sont pas perdues lorsque le jeu est ferm�.

//        Types de Donn�es: PlayerPrefs peut stocker des types de donn�es simples comme des entiers,
//                          des flottants et des cha�nes de caract�res.

//        Limites : PlayerPrefs n'est pas con�u pour stocker de grandes quantit�s de donn�es ou des types de donn�es complexes.
//                  Pour des besoins plus avanc�s, il est recommand� d'utiliser des solutions de s�rialisation ou des bases de donn�es.


//**************************************************************************************************
//En cours de r�flexion ==>> Solution a pr�voir pour optimiser les sauvegardes des donn�es en local
//**************************************************************************************************

// 1. un fichier JSON crypt�. Utiliser des biblioth�ques comme Newtonsoft.Json pour la gestion JSON et System.Security.Cryptography
// pour le cryptage en C#.

// 2. SQLite : Une base de donn�es l�g�re qui peut �tre int�gr�e directement dans votre application. Elle est id�ale pour des
// donn�es relationnelles et offre des fonctionnalit�s de s�curit� comme le cryptage.
