using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    /// <summary>
    /// Methode pour lancer le jeu
    /// </summary>
    public void StartGameButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    /// <summary>
    /// Methode pour aller aux parametres de jeu
    /// </summary>
    public void SettingsButton()
    {

    }

    /// <summary>
    /// Methode pour fermer le jeu
    /// </summary>
    public void QuitGameButton()
    {
        Application.Quit();
    }

    
}
