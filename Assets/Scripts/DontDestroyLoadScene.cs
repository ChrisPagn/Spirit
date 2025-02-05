using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script marque les objets sp�cifi�s pour qu'ils ne soient pas d�truits lors du chargement d'une nouvelle sc�ne.
/// </summary>
public class DontDestroyLoadScene : MonoBehaviour
{
    // Tableau des objets qui ne doivent pas �tre d�truits lors du changement de sc�ne
    public GameObject[] gameObjects;

    public static DontDestroyLoadScene instance;
    /// <summary>
    /// M�thode appel�e � l'initialisation de l'objet. Elle v�rifie chaque �l�ment du tableau et applique la m�thode DontDestroyOnLoad si n�cessaire.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance DontDestroyLoadScene dans la sc�ne");
            return;
        }

        instance = this;
        // Parcourt chaque �l�ment du tableau de GameObjects
        foreach (var element in gameObjects)
        {
            // V�rifie que l'�l�ment n'est pas nul et qu'il n'est pas d�j� marqu� comme "DontDestroyOnLoad"
            if (element != null && element.scene.name != "DontDestroyOnLoad")
            {
                // Marque cet objet pour qu'il ne soit pas d�truit lors du chargement d'une nouvelle sc�ne
                DontDestroyOnLoad(element);
            }
        }
    }

    /// <summary>
    /// M�thode qui retire les objets sp�cifi�s de la liste des objets � ne pas d�truire lors du changement de sc�ne.
    /// Elle v�rifie chaque �l�ment du tableau gameObjects et les d�place vers la sc�ne active.
    /// Si un �l�ment est null, un message d'erreur est affich�.
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
                Debug.LogError("Un �l�ment du tableau gameObjects est null.");
            }
        }
    }

}
