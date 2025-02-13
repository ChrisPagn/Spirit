using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script permet de charger une sc�ne sp�cifique lors de l'entr�e d'un joueur dans une zone.
/// Un syst�me de fondu est utilis� pour effectuer une transition visuelle entre les sc�nes.
/// </summary>
public class LoadSpecificScene : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip sound;

    // Nom de la sc�ne � charger
    public string sceneName;

    // R�f�rence � l'Animator pour la transition de fondu
    public Animator fadeSysteme;

    /// <summary>
    /// M�thode appel�e lors de l'initialisation du script. Trouve et assigne le syst�me de fondu via son tag.
    /// </summary>
    private void Awake()
    {
        GameObject fadeObject = GameObject.FindWithTag("FadeSystem");

        if (fadeObject == null)
        {
            Debug.LogError(" Aucun GameObject avec le tag 'FadeSystem' trouv� !");
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
        // Si l'objet entrant dans le trigger est le joueur, lancer la coroutine pour charger la sc�ne
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            StartCoroutine(loadNextScene());
        }
    }

    /// <summary>
    /// Coroutine qui g�re le chargement de la nouvelle sc�ne avec un fondu.
    /// </summary>
    /// <returns>Un IEnumerator n�cessaire pour le fonctionnement d'une coroutine</returns>
    public IEnumerator loadNextScene()
    {
        // Trouver le joueur et le Canvas UI
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Canvas canvasUI = GameObject.FindObjectOfType<Canvas>();

        // Emp�cher la destruction du joueur lors du chargement de la nouvelle sc�ne
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }

        // Emp�cher la destruction du Canvas UI, mais seulement s'il n'est pas d�j� persistant
        if (canvasUI != null && !canvasUI.gameObject.scene.name.Equals("DontDestroyOnLoad"))
        {
            DontDestroyOnLoad(canvasUI.gameObject);
        }

        // D�clencher l'animation de fondu via l'Animator
        fadeSysteme.SetTrigger("FadeIn");

        // Attendre que l'animation se termine avant de charger la sc�ne (1 seconde ici)
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle sc�ne sp�cifi�e dans "sceneName"
        SceneManager.LoadScene(sceneName);
    }
}
