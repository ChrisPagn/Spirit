using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script gère le chargement d'une scène spécifique lorsque le joueur entre dans une zone définie.
/// Il utilise un système de fondu pour effectuer une transition visuelle entre les scènes.
/// </summary>
public class LoadSpecificScene : MonoBehaviour
{
    /// <summary>
    /// Source audio utilisée pour jouer un son lors du changement de scène.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio à jouer lors du déclenchement du changement de scène.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Nom de la scène à charger lorsque le joueur entre dans la zone.
    /// </summary>
    public string sceneName;

    /// <summary>
    /// Référence à l'Animator utilisé pour la transition de fondu.
    /// </summary>
    private Animator fadeSysteme;

    /// <summary>
    /// Méthode appelée lors de l'initialisation du script.
    /// Trouve et assigne le système de fondu via son tag.
    /// </summary>
    private void Awake()
    {
        // Recherche de l'objet avec le tag "FadeSystem"
        GameObject fadeObject = GameObject.FindWithTag("FadeSystem");

        if (fadeObject == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'FadeSystem' trouvé !");
            return;
        }

        fadeSysteme = fadeObject.GetComponent<Animator>();

        if (fadeSysteme == null)
        {
            Debug.LogError($"L'objet {fadeObject.name} a été trouvé mais il ne contient pas d'Animator !");
        }
    }

    /// <summary>
    /// Méthode appelée lorsqu'un autre objet entre en collision avec le trigger (collider).
    /// Si c'est le joueur, démarre la coroutine pour charger la nouvelle scène.
    /// </summary>
    /// <param name="collision">Le Collider de l'objet entrant dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Vérifie si l'objet entrant est le joueur
        if (collision.CompareTag("Player"))
        {
            // Joue un son pour indiquer le changement de scène
            audioSource.PlayOneShot(sound);
            // Démarre la coroutine pour charger la scène
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Coroutine qui gère le chargement de la nouvelle scène avec un fondu,
    /// ainsi que la sauvegarde et le chargement des données.
    /// </summary>
    /// <returns>Un IEnumerator nécessaire pour le fonctionnement d'une coroutine</returns>
    public IEnumerator LoadNextScene()
    {
        // Vérifie si DataOrchestrator est initialisé
        if (DataOrchestrator.instance == null)
        {
            Debug.LogError("DataOrchestrator.instance est null.");
        }
        else
        {
            // Sauvegarde des données avant de changer de scène
            System.Threading.Tasks.Task saveDataTask = DataOrchestrator.instance.SaveData();
            Debug.LogWarning("Données sauvegardées au passage de niveau");

            // Attendre la fin de la tâche de sauvegarde
            yield return new WaitUntil(() => saveDataTask.IsCompleted);
        }

        // Vérifie si fadeSysteme est initialisé
        if (fadeSysteme == null)
        {
            Debug.LogError("fadeSysteme est null.");
        }
        else
        {
            // Déclenche l'animation de fondu via l'Animator
            fadeSysteme.SetTrigger("FadeIn");
        }

        // Attendre que l'animation de fondu se termine avant de charger la scène (1 seconde ici)
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle scène spécifiée dans "sceneName"
        SceneManager.LoadScene(sceneName);

        // Vérifie si DataOrchestrator est initialisé après le chargement de la scène
        if (DataOrchestrator.instance == null)
        {
            Debug.LogError("DataOrchestrator.instance est null après le chargement de la scène.");
        }
        else
        {
            // Recharger les données après le chargement de la scène
            System.Threading.Tasks.Task loadDataTask = DataOrchestrator.instance.LoadData();

            // Attendre la fin de la tâche de chargement
            yield return new WaitUntil(() => loadDataTask.IsCompleted);
        }
    }

}
