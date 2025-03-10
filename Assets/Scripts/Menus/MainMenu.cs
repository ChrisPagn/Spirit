using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère le menu principal du jeu, permettant de lancer le jeu, d'accéder aux paramètres, de quitter le jeu et de charger la scène des crédits.
/// </summary>
public class MainMenu : MonoBehaviour
{
    /// <summary>
    /// Nom de la scène à charger lorsque le jeu commence.
    /// </summary>
    public string levelToLoad;

    /// <summary>
    /// Fenêtre des paramètres du jeu.
    /// </summary>
    public GameObject settingsWindow;

    /// <summary>
    /// Lance le jeu en chargeant la scène spécifiée par levelToLoad.
    /// </summary>
    public void StartGameButton()
    {
        SceneManager.LoadScene(levelToLoad);
    }

    /// <summary>
    /// Ouvre la fenêtre des paramètres du jeu.
    /// </summary>
    public void SettingsButton()
    {
        settingsWindow.SetActive(true);
    }

    /// <summary>
    /// Ferme la fenêtre des paramètres du jeu.
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
    /// Charge la scène des crédits.
    /// </summary>
    public void LoadSceneCredits()
    {
        SceneManager.LoadScene("Credits");
    }
}
