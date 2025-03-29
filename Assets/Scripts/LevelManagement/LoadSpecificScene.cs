using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script g�re le chargement d'une sc�ne sp�cifique lorsque le joueur entre dans une zone d�finie.
/// Il utilise un syst�me de fondu pour effectuer une transition visuelle entre les sc�nes.
/// </summary>
public class LoadSpecificScene : MonoBehaviour
{
    /// <summary>
    /// Source audio utilis�e pour jouer un son lors du changement de sc�ne.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Clip audio � jouer lors du d�clenchement du changement de sc�ne.
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Nom de la sc�ne � charger lorsque le joueur entre dans la zone.
    /// </summary>
    public string sceneName;

    /// <summary>
    /// R�f�rence � l'Animator utilis� pour la transition de fondu.
    /// </summary>
    private Animator fadeSysteme;

    /// <summary>
    /// M�thode appel�e lors de l'initialisation du script.
    /// Trouve et assigne le syst�me de fondu via son tag.
    /// </summary>
    private void Awake()
    {
        // Recherche de l'objet avec le tag "FadeSystem"
        GameObject fadeObject = GameObject.FindWithTag("FadeSystem");

        if (fadeObject == null)
        {
            Debug.LogError("Aucun GameObject avec le tag 'FadeSystem' trouv� !");
            return;
        }

        fadeSysteme = fadeObject.GetComponent<Animator>();

        if (fadeSysteme == null)
        {
            Debug.LogError($"L'objet {fadeObject.name} a �t� trouv� mais il ne contient pas d'Animator !");
        }
    }

    /// <summary>
    /// M�thode appel�e lorsqu'un autre objet entre en collision avec le trigger (collider).
    /// Si c'est le joueur, d�marre la coroutine pour charger la nouvelle sc�ne.
    /// </summary>
    /// <param name="collision">Le Collider de l'objet entrant dans la zone de trigger</param>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // V�rifie si l'objet entrant est le joueur
        if (collision.CompareTag("Player"))
        {
            // Joue un son pour indiquer le changement de sc�ne
            audioSource.PlayOneShot(sound);
            // D�marre la coroutine pour charger la sc�ne
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Coroutine qui g�re le chargement de la nouvelle sc�ne avec un fondu,
    /// ainsi que la sauvegarde et le chargement des donn�es.
    /// </summary>
    /// <returns>Un IEnumerator n�cessaire pour le fonctionnement d'une coroutine</returns>
    public IEnumerator LoadNextScene()
    {
        // V�rifie si DataOrchestrator est initialis�
        if (DataOrchestrator.instance == null)
        {
            Debug.LogError("DataOrchestrator.instance est null.");
        }
        else
        {
            // Sauvegarde des donn�es avant de changer de sc�ne
            System.Threading.Tasks.Task saveDataTask = DataOrchestrator.instance.SaveData();
            Debug.LogWarning("Donn�es sauvegard�es au passage de niveau");

            // Attendre la fin de la t�che de sauvegarde
            yield return new WaitUntil(() => saveDataTask.IsCompleted);
        }

        // V�rifie si fadeSysteme est initialis�
        if (fadeSysteme == null)
        {
            Debug.LogError("fadeSysteme est null.");
        }
        else
        {
            // D�clenche l'animation de fondu via l'Animator
            fadeSysteme.SetTrigger("FadeIn");
        }

        // Attendre que l'animation de fondu se termine avant de charger la sc�ne (1 seconde ici)
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle sc�ne sp�cifi�e dans "sceneName"
        SceneManager.LoadScene(sceneName);

        // V�rifie si DataOrchestrator est initialis� apr�s le chargement de la sc�ne
        if (DataOrchestrator.instance == null)
        {
            Debug.LogError("DataOrchestrator.instance est null apr�s le chargement de la sc�ne.");
        }
        else
        {
            // Recharger les donn�es apr�s le chargement de la sc�ne
            System.Threading.Tasks.Task loadDataTask = DataOrchestrator.instance.LoadData();

            // Attendre la fin de la t�che de chargement
            yield return new WaitUntil(() => loadDataTask.IsCompleted);
        }
    }

}
