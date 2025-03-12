using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

/// <summary>
/// Ce script permet de charger une scène spécifique lors de l'entrée d'un joueur dans une zone.
/// Un système de fondu est utilisé pour effectuer une transition visuelle entre les scènes.
/// </summary>
public class LoadSpecificScene : MonoBehaviour
{
    /// <summary>
    /// playlist disponible dans l'audioManager
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// clip ou son 
    /// </summary>
    public AudioClip sound;

    /// <summary>
    /// Nom de la scène à charger
    /// </summary>
    public string sceneName;

    /// <summary>
    /// Référence à l'Animator pour la transition de fondu
    /// </summary>
    private Animator fadeSysteme;

    /// <summary>
    /// Méthode appelée lors de l'initialisation du script. Trouve et assigne le système de fondu via son tag.
    /// </summary>
    private void Awake()
    {
        GameObject fadeObject = GameObject.FindWithTag("FadeSystem");

        if (fadeObject == null)
        {
            Debug.LogError(" Aucun GameObject avec le tag 'FadeSystem' trouvé !");
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
        // Si l'objet entrant dans le trigger est le joueur, lancer la coroutine pour charger la scène
        if (collision.CompareTag("Player"))
        {
            audioSource.PlayOneShot(sound);
            StartCoroutine(LoadNextScene());
        }
    }

    /// <summary>
    /// Coroutine qui gère le chargement de la nouvelle scène avec un fondu.
    /// </summary>
    /// <returns>Un IEnumerator nécessaire pour le fonctionnement d'une coroutine</returns>
    public IEnumerator LoadNextScene()
    {
        // Trouver le joueur et le Canvas UI
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        //sauvegarde des données
        LoadAndSaveData.instance.SaveData();
        Debug.LogWarning("Données sauvegardées au passage de niveau");


        // Déclencher l'animation de fondu via l'Animator
        fadeSysteme.SetTrigger("FadeIn");

        // Attendre que l'animation se termine avant de charger la scène (1 seconde ici)
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle scène spécifiée dans "sceneName"
        SceneManager.LoadScene(sceneName);
    }
}
