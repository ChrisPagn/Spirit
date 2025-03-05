using UnityEngine;

public class CurrentSceneManager : MonoBehaviour
{
    public int coinsPickedUpInThisSceneCount;

    public Vector3 respawnPoint;

    public int levelToUnlock;

    /// <summary>
    /// singleton de l'instance
    /// </summary>
    public static CurrentSceneManager instance;

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
