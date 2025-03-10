using System.Linq;
using System.Net.Sockets;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// G�re le chargement et la sauvegarde des donn�es de jeu en local.
/// </summary>
public class LoadAndSaveData : MonoBehaviour
{
    /// <summary>
    /// Instance unique de la classe LoadAndSaveData.
    /// </summary>
    public static LoadAndSaveData instance;

    /// <summary>
    /// Initialise l'instance unique de LoadAndSaveData.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance LoadAndSaveData dans la sc�ne");
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Charge les donn�es sauvegard�es au d�marrage du jeu.
    /// </summary>
    void Start()
    {
        // Charge le nombre de pi�ces depuis PlayerPrefs
        // PlayerPrefs stocke les donn�es en local sur le disque de l'utilisateur
        // Les donn�es sont stock�es dans un fichier de registre sur Windows ou un fichier .plist sur macOS
        Inventory.instance.coinsCount = PlayerPrefs.GetInt("coinsCount", 0);
        Inventory.instance.UpdateTextUI();

        // Charge les objets sauvegard�s dans l'inventaire
        string[] itemsSaved = PlayerPrefs.GetString("inventoryItems", "").Split(',');

        for (int i = 0; i < itemsSaved.Length; i++)
        {
            if (itemsSaved[i] != "")
            {
                int id = int.Parse(itemsSaved[i]);
                Item currentItem = ItemsDataBase.instance.allItems.Single(x => x.id == id);
                Inventory.instance.contentItems.Add(currentItem);
            }
        }

        Inventory.instance.UpdateInventoryUI();
    }

    /// <summary>
    /// Sauvegarde les donn�es actuelles du jeu.
    /// </summary>
    public void SaveData()
    {
        // Sauvegarde le nombre de pi�ces dans PlayerPrefs
        // Les donn�es sont sauvegard�es en local sur le disque de l'utilisateur
        PlayerPrefs.SetInt("coinsCount", Inventory.instance.coinsCount);

        // Sauvegarde le niveau atteint
        if (CurrentSceneManager.instance.levelToUnlock > PlayerPrefs.GetInt("levelReached", 1))
        {
            PlayerPrefs.SetInt("levelReached", CurrentSceneManager.instance.levelToUnlock);
        }

        // Sauvegarde les objets dans l'inventaire
        // Les donn�es sont converties en une cha�ne de caract�res s�par�e par des virgules
        string itemsInInventory = string.Join(",", Inventory.instance.contentItems.Select(x => x.id));
        PlayerPrefs.SetString("inventoryItems", itemsInInventory);
    }
}

//*********************
//Aide m�moire
//*********************

// PlayerPrefs sauvegarde les donn�es dans un fichier de registre sur Windows ou un fichier .plist sur macOS
// Ces fichiers sont situ�s dans un r�pertoire sp�cifique � l'application sur le disque de l'utilisateur

//        Fonctionnement de PlayerPrefs:
//        Stockage Local : PlayerPrefs stocke les donn�es en local sur le disque de l'utilisateur.
//                          Sur Windows, les donn�es sont stock�es dans le registre, tandis que sur macOS,
//                          elles sont stock�es dans un fichier .plist.

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
