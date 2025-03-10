using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// G�re le retour au menu principal depuis l'�cran des cr�dits.
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
    /// Met � jour le comportement � chaque frame pour d�tecter l'appui sur la touche �chap.
    /// </summary>
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            LoadMainMenu();
        }
    }
}
