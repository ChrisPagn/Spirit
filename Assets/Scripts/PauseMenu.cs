using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    public static bool gameIsPaused = false; // Indique si le jeu est en pause
    public GameObject pauseMenuUI; // R�f�rence � l'interface utilisateur du menu pause
    public GameObject settingsMenuUI;

    /// <summary>
    /// Met � jour le comportement du menu pause � chaque frame.
    /// </summary>
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)) // V�rifie si la touche �chap est press�e
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
        // Emp�che les mouvements du joueur
        PlayerMovement.instance.enabled = false;
        // Active l'interface utilisateur du menu pause
        pauseMenuUI.SetActive(true);
        // Arr�te le temps dans le jeu
        Time.timeScale = 0;
        // Change l'�tat du jeu � "en pause"
        gameIsPaused = true;
    }

    /// <summary>
    /// Reprend le jeu apr�s une pause.
    /// </summary>
    public void Resume()
    {
        // Active les mouvements du joueur
        PlayerMovement.instance.enabled = true;
        // D�sactive l'interface utilisateur du menu pause
        pauseMenuUI.SetActive(false);
        // Red�marre le temps dans le jeu
        Time.timeScale = 1;
        // Change l'�tat du jeu � "en cours"
        gameIsPaused = false;
    }

    /// <summary>
    /// Charge le menu principal.
    /// </summary>
    public void LoadMainMenu()
    {
        // Reprend le jeu avant de charger le menu principal
        Resume();
        // Charge la sc�ne du menu principal
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
