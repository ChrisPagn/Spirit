using UnityEngine;

/// <summary>
/// Gère la lecture d'une playlist de musiques en boucle.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Liste des morceaux de musique à jouer.
    /// </summary>
    public AudioClip[] playlist;

    /// <summary>
    /// Source audio utilisée pour jouer les morceaux de musique.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Index de la musique actuellement jouée dans la playlist.
    /// </summary>
    private int musicIndex = 0;

    /// <summary>
    /// Initialise la lecture de la première musique de la playlist au démarrage.
    /// </summary>
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    /// <summary>
    /// Met à jour la lecture de la musique à chaque frame.
    /// </summary>
    void Update()
    {
        if (!audioSource.isPlaying)
        {
            PlayNextSong();
        }
    }

    /// <summary>
    /// Joue le morceau suivant dans la playlist.
    /// </summary>
    public void PlayNextSong()
    {
        musicIndex = (musicIndex + 1) % playlist.Length;
        audioSource.clip = playlist[musicIndex];
        audioSource.Play();
    }
}
