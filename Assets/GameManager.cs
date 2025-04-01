using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    private SaveData LastedData;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
            SceneManager.sceneLoaded += OnSceneLoaded; // 🔥 S'abonner à l'événement de chargement de scène
        }
        else
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// Charge la scène et met à jour les données après le chargement complet.
    /// </summary>
    /// <param name="lastLevelPlayed"></param>
    /// <param name="lastData"></param>
    public void OnLoadLevel(string lastLevelPlayed, SaveData lastData)
    {
        LastedData = lastData; // 🔥 Sauvegarde temporairement les données
        string sceneToLoad = string.IsNullOrEmpty(lastLevelPlayed) ? "level01" : lastLevelPlayed;
        SceneManager.LoadScene(sceneToLoad);
    }

    /// <summary>
    /// Callback appelé automatiquement après qu'une nouvelle scène est complètement chargée.
    /// </summary>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        Debug.Log($"Scène {scene.name} chargée.");

        if (Inventory.instance != null && LastedData != null)
        {
            DataOrchestrator.instance.ApplyDataOnUI(LastedData);
        }
        else
        {
            Debug.LogWarning("Inventory non trouvé ou données absentes.");
        }
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // 🔥 Désinscription pour éviter les fuites de mémoire
    }
}
