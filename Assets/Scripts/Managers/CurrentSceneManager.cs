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
