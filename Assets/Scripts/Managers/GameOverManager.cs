using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère l'affichage et les actions du menu Game Over lorsque le joueur meurt.
/// </summary>
public class GameOverManager : MonoBehaviour
{
    /// <summary>
    /// Interface utilisateur du menu Game Over.
    /// </summary>
    public GameObject gameOverUI;

    /// <summary>
    /// Instance unique de GameOverManager (Singleton).
    /// </summary>
    public static GameOverManager instance;

    /// <summary>
    /// Initialise l'instance unique de GameOverManager.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance GameOverManager dans la scène");
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Affiche le menu Game Over lorsque le joueur meurt.
    /// </summary>
    public void OnPlayerDeath()
    {
        gameOverUI.SetActive(true);
    }

    /// <summary>
    /// Recommence le niveau actuel en réinitialisant les paramètres du joueur.
    /// </summary>
    public void RetryButton()
    {
        Inventory.instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
        // Recharge la scène
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        // Replace le joueur au point de réapparition
        PlayerHealth.instance.Respawn();
        // Réactive les mouvements du joueur et restaure sa vie
        gameOverUI.SetActive(false);
    }

    /// <summary>
    /// Retourne au menu principal.
    /// </summary>
    public void MainMenuButton()
    {
        SceneManager.LoadScene("MainMenu");
    }

    /// <summary>
    /// Ferme le jeu.
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}
