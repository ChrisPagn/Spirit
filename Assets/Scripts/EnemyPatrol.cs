using UnityEngine;

/// <summary>
/// Ce script permet � un ennemi de patrouiller entre plusieurs points (waypoints) et de causer des d�g�ts au joueur en cas de collision.
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
    // Vitesse de d�placement de l'ennemi
    public float speed;

    // Tableau des positions que l'ennemi va suivre
    public Transform[] waypoints;

    // D�g�ts inflig�s au joueur lors d'une collision
    public int damageOnCollision = 10;

    // Cible actuelle de l'ennemi (le waypoint vers lequel il se d�place)
    private Transform target;

    // Index du prochain waypoint dans le tableau
    private int destPoint = 0;

    // R�f�rence au SpriteRenderer pour changer l'orientation de l'ennemi
    public SpriteRenderer orientationPersonnage;

    /// <summary>
    /// Initialisation au d�marrage. D�finit la premi�re cible de l'ennemi.
    /// </summary>
    void Start()
    {
        target = waypoints[0]; // L'ennemi se d�place vers le premier waypoint au d�but
    }

    /// <summary>
    /// Mise � jour � chaque frame. L'ennemi se d�place vers la cible actuelle et v�rifie s'il doit changer de direction.
    /// </summary>
    void Update()
    {
        // Calcul de la direction vers le waypoint cible
        Vector3 direction = target.position - transform.position;

        // D�placement de l'ennemi vers la cible avec normalisation de la direction
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Si l'ennemi est proche de sa cible (moins de 0.3 unit�s de distance)
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            // Passer au waypoint suivant en utilisant un modulo pour boucler apr�s le dernier waypoint
            destPoint = (destPoint + 1) % waypoints.Length;

            // Changer la cible pour le prochain waypoint
            target = waypoints[destPoint];

            // Inverser l'orientation du sprite de l'ennemi pour le faire se retourner
            orientationPersonnage.flipX = !orientationPersonnage.flipX;
        }
    }

    /// <summary>
    /// D�tecte la collision avec un autre objet, en particulier avec le joueur.
    /// Si l'ennemi entre en collision avec le joueur, des d�g�ts lui sont inflig�s.
    /// </summary>
    /// <param name="collision">Objet avec lequel l'ennemi est entr� en collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // V�rifie si l'objet avec lequel il entre en collision est le joueur
        if (collision.transform.CompareTag("Player"))
        {
            // R�cup�re le script PlayerHealth sur le joueur pour appliquer des d�g�ts
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();

            // Si le joueur a un composant PlayerHealth, on lui inflige des d�g�ts
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageOnCollision);
            }
        }
    }
}
