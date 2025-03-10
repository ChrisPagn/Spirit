using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère le retour au menu principal depuis l'écran des crédits.
/// </summary>
public class Credits : MonoBehaviour
{
    /// <summary>
    /// Charge le menu principal.
    /// </summary>
    public void LoadMainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Met à jour le comportement à chaque frame pour détecter l'appui sur la touche Échap.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }
}
