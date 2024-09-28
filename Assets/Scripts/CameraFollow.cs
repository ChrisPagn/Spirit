using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script de suivi de la cam�ra pour suivre un joueur dans une sc�ne Unity.
/// Ce script ajuste la position de la cam�ra pour qu'elle suive l'objet joueur.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // Le tag qui permet d'identifier le joueur dans la sc�ne.
    public string playerTag = "Player";

    // Temps de d�calage pour adoucir le mouvement de la cam�ra (plus le temps est �lev�, plus la cam�ra se d�placera lentement).
    public float timeOffset;

    // D�calage de la position de la cam�ra par rapport � celle du joueur.
    public Vector2 posOffset;

    // Vitesse du lissage lors du mouvement de la cam�ra.
    private Vector2 velocity;

    // R�f�rence � l'objet joueur dans la sc�ne.
    private GameObject player;

    /// <summary>
    /// Appel� lors de l'initialisation de l'objet.
    /// Utilis� pour emp�cher la destruction de la cam�ra lors des changements de sc�ne.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Garde la cam�ra en vie � travers les changements de sc�nes.
    }

    /// <summary>
    /// Abonnement � l'�v�nement de chargement de sc�ne.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Abonne la fonction OnSceneLoaded � l'�v�nement de chargement de sc�ne.
    }

    /// <summary>
    /// D�sabonnement de l'�v�nement de chargement de sc�ne.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // D�sabonne la fonction lorsque l'objet est d�sactiv� ou d�truit.
    }

    /// <summary>
    /// Appel� quand une nouvelle sc�ne est charg�e.
    /// Lance une coroutine pour trouver le joueur dans la nouvelle sc�ne apr�s un court d�lai.
    /// </summary>
    /// <param name="scene">La sc�ne charg�e.</param>
    /// <param name="mode">Le mode de chargement de la sc�ne.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindPlayerWithDelay()); // Commence la recherche du joueur apr�s un petit d�lai.
    }

    /// <summary>
    /// Coroutine pour rechercher le joueur apr�s un d�lai d'une frame.
    /// </summary>
    private IEnumerator FindPlayerWithDelay()
    {
        yield return null; // Attendre une frame pour s'assurer que tous les objets de la sc�ne sont bien initialis�s.
        FindPlayer(); // Appelle la m�thode pour rechercher le joueur dans la sc�ne.
    }

    /// <summary>
    /// Recherche l'objet joueur dans la sc�ne en utilisant le tag d�fini.
    /// </summary>
    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag(playerTag); // Recherche l'objet avec le tag "Player".
        if (player == null)
        {
            Debug.LogWarning("Player non trouv� dans la sc�ne dans le script CameraFollow !");
        }
    }

    /// <summary>
    /// Appel� � chaque frame pour mettre � jour la position de la cam�ra.
    /// </summary>
    private void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Si le joueur n'a pas encore �t� trouv�, on tente de le retrouver.
            return;
        }

        // Position actuelle de la cam�ra
        Vector3 startPosition = transform.position;

        // Position cible de la cam�ra (celle du joueur avec un d�calage)
        Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y) + posOffset;

        // Calcule une transition adoucie entre la position actuelle et la position cible
        Vector2 smoothedPosition = Vector2.SmoothDamp(
            new Vector2(startPosition.x, startPosition.y), // Position actuelle de la cam�ra
            targetPosition, // Position cible (position du joueur avec un d�calage)
            ref velocity, // R�f�rence � la vitesse, utilis�e par le SmoothDamp
            timeOffset // Temps de lissage
        );

        // Mise � jour de la position de la cam�ra
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, startPosition.z);
    }
}
