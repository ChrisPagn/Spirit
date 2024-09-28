using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script permet de charger une scène spécifique lors de l'entrée d'un joueur dans une zone.
/// Un système de fondu est utilisé pour effectuer une transition visuelle entre les scènes.
/// </summary>
public class LoadSpecificScene : MonoBehaviour
{
    // Nom de la scène à charger
    public string sceneName;

    // Référence à l'Animator pour la transition de fondu
    public Animator fadeSysteme;

    /// <summary>
    /// Méthode appelée lors de l'initialisation du script. Trouve et assigne le système de fondu via son tag.
    /// </summary>
    private void Awake()
    {
        // Trouver l'Animator du système de fondu dans la scène via son tag "FadeSystem"
        fadeSysteme = GameObject.FindWithTag("FadeSystem").GetComponent<Animator>();
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre en collision avec le trigger (collider).
    /// Si c'est le joueur, démarre la coroutine pour charger la nouvelle scène.
    /// </summary>
    /// <param name="collision">Le Collider de l'objet entrant dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Si l'objet entrant dans le trigger est le joueur, lancer la coroutine pour charger la scène
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(loadNextScene());
        }
    }

    /// <summary>
    /// Coroutine qui gère le chargement de la nouvelle scène avec un fondu.
    /// </summary>
    /// <returns>Un IEnumerator nécessaire pour le fonctionnement d'une coroutine</returns>
    public IEnumerator loadNextScene()
    {
        // Trouver le joueur et le Canvas UI
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Canvas canvasUI = GameObject.FindObjectOfType<Canvas>();

        // Empêcher la destruction du joueur lors du chargement de la nouvelle scène
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }

        // Empêcher la destruction du Canvas UI, mais seulement s'il n'est pas déjà persistant
        if (canvasUI != null && !canvasUI.gameObject.scene.name.Equals("DontDestroyOnLoad"))
        {
            DontDestroyOnLoad(canvasUI.gameObject);
        }

        // Déclencher l'animation de fondu via l'Animator
        fadeSysteme.SetTrigger("FadeIn");

        // Attendre que l'animation se termine avant de charger la scène (1 seconde ici)
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle scène spécifiée dans "sceneName"
        SceneManager.LoadScene(sceneName);
    }
}
