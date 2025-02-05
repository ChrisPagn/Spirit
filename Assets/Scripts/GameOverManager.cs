using UnityEngine;
using UnityEngine.SceneManagement;
public class GameOverManager : MonoBehaviour
{

    public GameObject gameOverUI;

    public static GameOverManager instance;

    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance GameOverManager dans la sc�ne");
            return;
        }

        instance = this;
    }

    public void OnPlayerDeath()
    {
        if (CurrentSceneManager.instance.isPlayerPresentByDefault) 
        {
            DontDestroyLoadScene.instance.RemoveFromDontDestroyOnLoad();
        }

        
        gameOverUI.SetActive(true);


    }

    /// <summary>
    /// Methode pour recommencer le niveau
    /// </summary>
    public void RetryButton()
    {
        Inventory.Instance.RemoveCoins(CurrentSceneManager.instance.coinsPickedUpInThisSceneCount);
        //recharge la scene
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        //replace le joueur au spawn
        PlayerHealth.instance.Respawn();
        //reactive les mouvements du joueur + lui rendre sa vie
        gameOverUI.SetActive(false);
    }
    /// <summary>
    /// Methode pour retourner au menu principal
    /// </summary>
    public void MainMenuButton()
    {

    }
    /// <summary>
    /// Methode pour fermer le jeu
    /// </summary>
    public void QuitButton()
    {
        Application.Quit();
    }
}
