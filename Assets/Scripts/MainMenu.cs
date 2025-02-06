using UnityEngine;
using UnityEngine.SceneManagement;
public class MainMenu : MonoBehaviour
{
    public string levelToLoad;

    public GameObject settingsWindow;

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
        settingsWindow.SetActive(true);
    }

    /// <summary>
    /// Methode pour fermer la fenetre settingswindow
    /// </summary>
    public void CloseSettingsWindows()
    {
        settingsWindow.SetActive(false);
    }

    /// <summary>
    /// Methode pour fermer le jeu
    /// </summary>
    public void QuitGameButton()
    {
        Application.Quit();
    }

    
}
