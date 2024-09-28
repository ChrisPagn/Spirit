using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Script de suivi de la caméra pour suivre un joueur dans une scène Unity.
/// Ce script ajuste la position de la caméra pour qu'elle suive l'objet joueur.
/// </summary>
public class CameraFollow : MonoBehaviour
{
    // Le tag qui permet d'identifier le joueur dans la scène.
    public string playerTag = "Player";

    // Temps de décalage pour adoucir le mouvement de la caméra (plus le temps est élevé, plus la caméra se déplacera lentement).
    public float timeOffset;

    // Décalage de la position de la caméra par rapport à celle du joueur.
    public Vector2 posOffset;

    // Vitesse du lissage lors du mouvement de la caméra.
    private Vector2 velocity;

    // Référence à l'objet joueur dans la scène.
    private GameObject player;

    /// <summary>
    /// Appelé lors de l'initialisation de l'objet.
    /// Utilisé pour empêcher la destruction de la caméra lors des changements de scène.
    /// </summary>
    private void Awake()
    {
        DontDestroyOnLoad(gameObject); // Garde la caméra en vie à travers les changements de scènes.
    }

    /// <summary>
    /// Abonnement à l'événement de chargement de scène.
    /// </summary>
    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded; // Abonne la fonction OnSceneLoaded à l'événement de chargement de scène.
    }

    /// <summary>
    /// Désabonnement de l'événement de chargement de scène.
    /// </summary>
    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Désabonne la fonction lorsque l'objet est désactivé ou détruit.
    }

    /// <summary>
    /// Appelé quand une nouvelle scène est chargée.
    /// Lance une coroutine pour trouver le joueur dans la nouvelle scène après un court délai.
    /// </summary>
    /// <param name="scene">La scène chargée.</param>
    /// <param name="mode">Le mode de chargement de la scène.</param>
    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindPlayerWithDelay()); // Commence la recherche du joueur après un petit délai.
    }

    /// <summary>
    /// Coroutine pour rechercher le joueur après un délai d'une frame.
    /// </summary>
    private IEnumerator FindPlayerWithDelay()
    {
        yield return null; // Attendre une frame pour s'assurer que tous les objets de la scène sont bien initialisés.
        FindPlayer(); // Appelle la méthode pour rechercher le joueur dans la scène.
    }

    /// <summary>
    /// Recherche l'objet joueur dans la scène en utilisant le tag défini.
    /// </summary>
    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag(playerTag); // Recherche l'objet avec le tag "Player".
        if (player == null)
        {
            Debug.LogWarning("Player non trouvé dans la scène dans le script CameraFollow !");
        }
    }

    /// <summary>
    /// Appelé à chaque frame pour mettre à jour la position de la caméra.
    /// </summary>
    private void Update()
    {
        if (player == null)
        {
            FindPlayer(); // Si le joueur n'a pas encore été trouvé, on tente de le retrouver.
            return;
        }

        // Position actuelle de la caméra
        Vector3 startPosition = transform.position;

        // Position cible de la caméra (celle du joueur avec un décalage)
        Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y) + posOffset;

        // Calcule une transition adoucie entre la position actuelle et la position cible
        Vector2 smoothedPosition = Vector2.SmoothDamp(
            new Vector2(startPosition.x, startPosition.y), // Position actuelle de la caméra
            targetPosition, // Position cible (position du joueur avec un décalage)
            ref velocity, // Référence à la vitesse, utilisée par le SmoothDamp
            timeOffset // Temps de lissage
        );

        // Mise à jour de la position de la caméra
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, startPosition.z);
    }
}
