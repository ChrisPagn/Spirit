using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// G�re le menu principal du jeu, permettant de lancer le jeu, d'acc�der aux param�tres, de quitter le jeu et de charger la sc�ne des cr�dits.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Nom de la sc�ne � charger lorsque le jeu commence.
    /// </summary>
    public string levelToLoad;

    /// <summary>
    /// Fen�tre des param�tres du jeu.
    /// </summary>
    public GameObject settingsWindow;

    /// <summary>
    /// Lance le jeu en chargeant la sc�ne sp�cifi�e par levelToLoad.
    /// </summary>
    public void StartGameButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    /// <summary>
    /// Ouvre la fen�tre des param�tres du jeu.
    /// </summary>
    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    /// <summary>
    /// Ferme la fen�tre des param�tres du jeu.
    /// </summary>
    public void CloseSettingsWindows()
    {
        settingsWindow.SetActive(false);
    }

    /// <summary>
    /// Ferme le jeu.
    /// </summary>
    public void QuitGameButton()
    {
        Application.Quit();
    }

    /// <summary>
    /// Charge la sc�ne des cr�dits.
    /// </summary>
    public void LoadSceneCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
