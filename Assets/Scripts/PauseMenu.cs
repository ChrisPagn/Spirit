using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false; // Indique si le jeu est en pause
    public GameObject pauseMenuUI; // Référence à l'interface utilisateur du menu pause
    public GameObject settingsMenuUI;

    /// <summary>
    /// Met à jour le comportement du menu pause à chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // Vérifie si la touche Échap est pressée
        {
            if (gameIsPaused)
            {
                Resume(); // Reprendre le jeu
            }
            else
            {
                Paused(); // Mettre le jeu en pause
            }
        }
    }

    /// <summary>
    /// Met le jeu en pause.
    /// </summary>
    public void Paused()
    {
        // Empêche les mouvements du joueur
        PlayerMovement.instance.enabled = false;
        // Active l'interface utilisateur du menu pause
        pauseMenuUI.SetActive(true);
        // Arrête le temps dans le jeu
        Time.timeScale = 0;
        // Change l'état du jeu à "en pause"
        gameIsPaused = true;
    }

    /// <summary>
    /// Reprend le jeu après une pause.
    /// </summary>
    public void Resume()
    {
        // Active les mouvements du joueur
        PlayerMovement.instance.enabled = true;
        // Désactive l'interface utilisateur du menu pause
        pauseMenuUI.SetActive(false);
        // Redémarre le temps dans le jeu
        Time.timeScale = 1;
        // Change l'état du jeu à "en cours"
        gameIsPaused = false;
    }

    /// <summary>
    /// Charge le menu principal.
    /// </summary>
    public void LoadMainMenu()
    {
        // Reprend le jeu avant de charger le menu principal
        Resume();
        // Charge la scène du menu principal
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Ouvre le menu settings.
    /// </summary>
    public void OpenSettingsMenu()
    {
        settingsMenuUI.SetActive(true);
    }

    /// <summary>
    /// Ferme le menu settings.
    /// </summary>
    public void CloseSettingsMenu()
    {
        settingsMenuUI.SetActive(false);
    }

   
}
