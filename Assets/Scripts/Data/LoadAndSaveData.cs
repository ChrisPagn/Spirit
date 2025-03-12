using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;
using Newtonsoft.Json;
using UnityEngine;

public class LoadAndSaveData : MonoBehaviour
{
    public static LoadAndSaveData instance;

    private string filePath;
    private string encryptionKey = "YourEncryptionKeyHere"; // Remplacez par une cl� s�curis�e

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
        filePath = Path.Combine(Application.persistentDataPath, "saveData.json");

        // Si on veut que l'objet persiste entre les sc�nes :
        DontDestroyOnLoad(gameObject);
    }

    void Start()
    {
        LoadData();
    }

    public void SaveData()
    {
        var saveData = new SaveData
        {
            CoinsCount = Inventory.instance.coinsCount,
            LevelReached = CurrentSceneManager.instance.levelToUnlock,
            InventoryItems = Inventory.instance.contentItems.ConvertAll(item => item.id)
        };

        try
        {
            string json = JsonConvert.SerializeObject(saveData);
            string encryptedJson = EncryptString(json, encryptionKey);
            File.WriteAllText(filePath, encryptedJson);
        }
        catch (Exception ex)
        {
            Debug.LogError($"Erreur lors de la sauvegarde des donn�es : {ex.Message}");
        }
    }

    private void LoadData()
    {
        if (File.Exists(filePath))
        {
            try
            {
                string encryptedJson = File.ReadAllText(filePath);
                string json = DecryptString(encryptedJson, encryptionKey);
                SaveData saveData = JsonConvert.DeserializeObject<SaveData>(json);

                if (saveData != null)
                {
                    Inventory.instance.coinsCount = saveData.CoinsCount;
                    Inventory.instance.UpdateTextUI();

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
                    Debug.LogWarning("Les donn�es sauvegard�es sont corrompues ou invalides.");
                }
            }
            catch (Exception ex)
            {
                Debug.LogError($"Erreur lors du chargement des donn�es : {ex.Message}");
            }
        }
    }

    private string EncryptString(string plainText, string key)
    {
        using (Aes aes = Aes.Create())
        {
            // G�n�ration d'une cl� s�curis�e avec un sel pour �viter les attaques par dictionnaire
            byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
            aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);
            aes.GenerateIV(); // IV al�atoire

            using (MemoryStream memoryStream = new MemoryStream())
            {
                memoryStream.Write(aes.IV, 0, aes.IV.Length);

                using (CryptoStream cryptoStream = new CryptoStream(memoryStream, aes.CreateEncryptor(), CryptoStreamMode.Write))
                using (StreamWriter streamWriter = new StreamWriter(cryptoStream))
                {
                    streamWriter.Write(plainText);
                }

                return Convert.ToBase64String(memoryStream.ToArray());
            }
        }
    }

    private string DecryptString(string cipherText, string key)
    {
        try
        {
            byte[] fullCipher = Convert.FromBase64String(cipherText);

            using (Aes aes = Aes.Create())
            {
                byte[] salt = Encoding.UTF8.GetBytes("SaltValue1234");
                aes.Key = new Rfc2898DeriveBytes(key, salt, 10000).GetBytes(32);

                byte[] iv = new byte[aes.BlockSize / 8];
                byte[] cipher = new byte[fullCipher.Length - iv.Length];

                Array.Copy(fullCipher, iv, iv.Length);
                Array.Copy(fullCipher, iv.Length, cipher, 0, cipher.Length);

                aes.IV = iv;

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
