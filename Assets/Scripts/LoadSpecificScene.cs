using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadSpecificScene : MonoBehaviour
{
    public string sceneName;
    public Animator fadeSysteme;

    private void Awake()
    {
        // Utilisation de FindWithTag pour obtenir un seul GameObject
        fadeSysteme = GameObject.FindWithTag("FadeSystem").GetComponent<Animator>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            StartCoroutine(loadNextScene());
        }
    }

    public IEnumerator loadNextScene()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        Canvas canvasUI = GameObject.FindObjectOfType<Canvas>();

        // Empêcher la destruction du joueur
        if (player != null)
        {
            DontDestroyOnLoad(player);
        }

        // Empêcher la destruction du canvas UI, mais seulement s'il n'existe pas déjà une instance persistante
        if (canvasUI != null && !canvasUI.gameObject.scene.name.Equals("DontDestroyOnLoad"))
        {
            DontDestroyOnLoad(canvasUI.gameObject);
        }

        // Lancer l'animation de fondu
        fadeSysteme.SetTrigger("FadeIn");

        // Attendre la fin de l'animation
        yield return new WaitForSeconds(1f);

        // Charger la nouvelle scène
        SceneManager.LoadScene(sceneName);
    }
}

