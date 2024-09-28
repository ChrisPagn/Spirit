using UnityEngine;

/// <summary>
/// Ce script permet à un ennemi de patrouiller entre plusieurs points (waypoints) et de causer des dégâts au joueur en cas de collision.
/// </summary>
public class EnemyPatrol : MonoBehaviour
{
    // Vitesse de déplacement de l'ennemi
    public float speed;

    // Tableau des positions que l'ennemi va suivre
    public Transform[] waypoints;

    // Dégâts infligés au joueur lors d'une collision
    public int damageOnCollision = 10;

    // Cible actuelle de l'ennemi (le waypoint vers lequel il se déplace)
    private Transform target;

    // Index du prochain waypoint dans le tableau
    private int destPoint = 0;

    // Référence au SpriteRenderer pour changer l'orientation de l'ennemi
    public SpriteRenderer orientationPersonnage;

    /// <summary>
    /// Initialisation au démarrage. Définit la première cible de l'ennemi.
    /// </summary>
    void Start()
    {
        target = waypoints[0]; // L'ennemi se déplace vers le premier waypoint au début
    }

    /// <summary>
    /// Mise à jour à chaque frame. L'ennemi se déplace vers la cible actuelle et vérifie s'il doit changer de direction.
    /// </summary>
    void Update()
    {
        // Calcul de la direction vers le waypoint cible
        Vector3 direction = target.position - transform.position;

        // Déplacement de l'ennemi vers la cible avec normalisation de la direction
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Si l'ennemi est proche de sa cible (moins de 0.3 unités de distance)
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            // Passer au waypoint suivant en utilisant un modulo pour boucler après le dernier waypoint
            destPoint = (destPoint + 1) % waypoints.Length;

            // Changer la cible pour le prochain waypoint
            target = waypoints[destPoint];

            // Inverser l'orientation du sprite de l'ennemi pour le faire se retourner
            orientationPersonnage.flipX = !orientationPersonnage.flipX;
        }
    }

    /// <summary>
    /// Détecte la collision avec un autre objet, en particulier avec le joueur.
    /// Si l'ennemi entre en collision avec le joueur, des dégâts lui sont infligés.
    /// </summary>
    /// <param name="collision">Objet avec lequel l'ennemi est entré en collision</param>
    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Vérifie si l'objet avec lequel il entre en collision est le joueur
        if (collision.transform.CompareTag("Player"))
        {
            // Récupère le script PlayerHealth sur le joueur pour appliquer des dégâts
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();

            // Si le joueur a un composant PlayerHealth, on lui inflige des dégâts
            if (playerHealth != null)
            {
                playerHealth.TakeDamage(damageOnCollision);
            }
        }
    }
}
