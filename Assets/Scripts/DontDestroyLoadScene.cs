using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script marque les objets spécifiés pour qu'ils ne soient pas détruits lors du chargement d'une nouvelle scène.
/// </summary>
public class DontDestroyLoadScene : MonoBehaviour
{
    // Tableau des objets qui ne doivent pas être détruits lors du changement de scène
    public GameObject[] gameObjects;

    public static DontDestroyLoadScene instance;
    /// <summary>
    /// Méthode appelée à l'initialisation de l'objet. Elle vérifie chaque élément du tableau et applique la méthode DontDestroyOnLoad si nécessaire.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance DontDestroyLoadScene dans la scène");
            return;
        }

        instance = this;
        // Parcourt chaque élément du tableau de GameObjects
        foreach (var element in gameObjects)
        {
            // Vérifie que l'élément n'est pas nul et qu'il n'est pas déjà marqué comme "DontDestroyOnLoad"
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                // Marque cet objet pour qu'il ne soit pas détruit lors du chargement d'une nouvelle scène
                DontDestroyOnLoad(element);
            }
        }
    }

    /// <summary>
    /// Méthode qui retire les objets spécifiés de la liste des objets à ne pas détruire lors du changement de scène.
    /// Elle vérifie chaque élément du tableau gameObjects et les déplace vers la scène active.
    /// Si un élément est null, un message d'erreur est affiché.
    /// </summary>
    public void RemoveFromDontDestroyOnLoad()
    {
        if (gameObjects == null)
        {
            Debug.LogError("Le tableau gameObjects est null.");
            return;
        }

        foreach (var element in gameObjects)
        {
            if (element != null)
            {
                SceneManager.MoveGameObjectToScene(element, SceneManager.GetActiveScene());
            }
            else
            {
                Debug.LogError("Un élément du tableau gameObjects est null.");
            }
        }
    }

}
