using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

/// <summary>
/// Gestion de la scene LevelSelect
/// </summary>
public class LevelSelector : MonoBehaviour
{

    public Button[] levelbuttons;
    

    /// <summary>
    /// Méthode qui initialise l'activation des boutons de séléctions
    /// par default sur false sauf pour les scenes terminées ainsi
    /// que la suivante
    /// </summary>
    private void Start()
    {
        int levelReached = PlayerPrefs.GetInt("levelReached", 1);

        for (int i = 0; i < levelbuttons.Length; i++)
        {
            if (i + 1 > levelReached) 
            {
                levelbuttons[i].interactable = false;
            }
        }      
     }

    /// <summary>
    /// Methode qui charge la scene selectionnée
    /// </summary>
    /// <param name="LevelName"></param>
    public void LoadLevelPassed(string LevelName)
    {
          
       SceneManager.LoadScene(LevelName);
    }
}
