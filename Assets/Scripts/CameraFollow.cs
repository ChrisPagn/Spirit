using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CameraFollow : MonoBehaviour
{
    public string playerTag = "Player";
    public float timeOffset;
    public Vector2 posOffset;

    private Vector2 velocity;
    private GameObject player;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        StartCoroutine(FindPlayerWithDelay());
    }

    private IEnumerator FindPlayerWithDelay()
    {
        yield return null; // Attendre une frame pour s'assurer que tous les objets sont initialisés
        FindPlayer();
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag(playerTag);
        if (player == null)
        {
            Debug.LogWarning("Player not found in the scene dans le script camerafollow!");
        }
    }

    void Update()
    {
        if (player == null)
        {
            FindPlayer();
            return;
        }

        Vector3 startPosition = transform.position;
        Vector2 targetPosition = new Vector2(player.transform.position.x, player.transform.position.y) + posOffset;
        Vector2 smoothedPosition = Vector2.SmoothDamp(new Vector2(startPosition.x, startPosition.y), targetPosition, ref velocity, timeOffset);
        transform.position = new Vector3(smoothedPosition.x, smoothedPosition.y, startPosition.z);
    }
}
