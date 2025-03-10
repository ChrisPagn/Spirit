using UnityEngine;

/// <summary>
/// G�re les informations sp�cifiques � la sc�ne actuelle, comme le nombre de pi�ces ramass�es, 
/// le point de r�apparition du joueur, et le niveau � d�bloquer.
/// </summary>
public class CurrentSceneManager : MonoBehaviour
{
    /// <summary>
    /// Nombre de pi�ces ramass�es dans la sc�ne actuelle.
    /// </summary>
    public int coinsPickedUpInThisSceneCount;

    /// <summary>
    /// Point de r�apparition du joueur dans la sc�ne actuelle.
    /// </summary>
    public Vector3 respawnPoint;

    /// <summary>
    /// Niveau � d�bloquer apr�s avoir compl�t� cette sc�ne.
    /// </summary>
    public int levelToUnlock;

    /// <summary>
    /// Instance unique de CurrentSceneManager (Singleton).
    /// </summary>
    public static CurrentSceneManager instance;

    /// <summary>
    /// Initialise l'instance unique de CurrentSceneManager et d�finit le point de r�apparition du joueur.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance CurrentSceneManager dans la sc�ne");
            return;
        }

        instance = this;

        // D�finir le point de r�apparition du joueur dans la sc�ne actuelle
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            respawnPoint = player.transform.position;
        }
        else
        {
            Debug.LogWarning("Le joueur est introuvable ! Le point de r�apparition ne peut pas �tre d�fini.");
        }
    }
}
