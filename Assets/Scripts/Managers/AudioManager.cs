using UnityEngine;

/// <summary>
/// G�re la lecture d'une playlist de musiques en boucle.
/// </summary>
public class AudioManager : MonoBehaviour
{
    /// <summary>
    /// Liste des morceaux de musique � jouer.
    /// </summary>
    public AudioClip[] playlist;

    /// <summary>
    /// Source audio utilis�e pour jouer les morceaux de musique.
    /// </summary>
    public AudioSource audioSource;

    /// <summary>
    /// Index de la musique actuellement jou�e dans la playlist.
    /// </summary>
    private int musicIndex = 0;

    /// <summary>
    /// Initialise la lecture de la premi�re musique de la playlist au d�marrage.
    /// </summary>
    void Start()
    {
        audioSource.clip = playlist[0];
        audioSource.Play();
    }

    /// <summary>
    /// Met � jour la lecture de la musique � chaque frame.
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
