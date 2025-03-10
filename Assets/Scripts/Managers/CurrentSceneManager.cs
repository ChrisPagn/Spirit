using UnityEngine;

/// <summary>
/// Gère les informations spécifiques à la scène actuelle, comme le nombre de pièces ramassées, 
/// le point de réapparition du joueur, et le niveau à débloquer.
/// </summary>
public class CurrentSceneManager : MonoBehaviour
{
    /// <summary>
    /// Nombre de pièces ramassées dans la scène actuelle.
    /// </summary>
    public int coinsPickedUpInThisSceneCount;

    /// <summary>
    /// Point de réapparition du joueur dans la scène actuelle.
    /// </summary>
    public Vector3 respawnPoint;

    /// <summary>
    /// Niveau à débloquer après avoir complété cette scène.
    /// </summary>
    public int levelToUnlock;

    /// <summary>
    /// Instance unique de CurrentSceneManager (Singleton).
    /// </summary>
    public static CurrentSceneManager instance;

    /// <summary>
    /// Initialise l'instance unique de CurrentSceneManager et définit le point de réapparition du joueur.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance CurrentSceneManager dans la scène");
            return;
        }

        instance = this;

        // Définir le point de réapparition du joueur dans la scène actuelle
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            respawnPoint = player.transform.position;
        }
        else
        {
            Debug.LogWarning("Le joueur est introuvable ! Le point de réapparition ne peut pas être défini.");
        }
    }
}
