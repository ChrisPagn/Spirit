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
    /// M�thode qui initialise l'activation des boutons de s�l�ctions
    /// par default sur false sauf pour les scenes termin�es ainsi
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
    /// Methode qui charge la scene selectionn�e
    /// </summary>
    /// <param name="LevelName"></param>
    public void LoadLevelPassed(string LevelName)
    {
          
       SceneManager.LoadScene(LevelName);
    }
}
