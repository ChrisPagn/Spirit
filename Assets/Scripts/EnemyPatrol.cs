using UnityEngine;

public class EnemyPatrol : MonoBehaviour
{
    public float speed;
    public Transform[] waypoints;
    public int damageOnCollision = 10;


    private Transform target;
    private int destPoint = 0;

    public SpriteRenderer orientationPersonnage;
    
    // Start is called before the first frame update
    void Start()
    {
        target = waypoints[0];
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 direction = target.position - transform.position;
        transform.Translate(direction.normalized * speed * Time.deltaTime, Space.World);

        // Si l'ennemi est quasiment arrivé a destination
        if (Vector3.Distance(transform.position, target.position) < 0.3f)
        {
            // utilisation du modulo afin que quand j'arrive au dernier point il revienne a 0 
            destPoint = (destPoint + 1) % waypoints.Length;
            target = waypoints[destPoint];
            orientationPersonnage.flipX = !orientationPersonnage.flipX;
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("Player"))
        {
            PlayerHealth playerHealth = collision.transform.GetComponent<PlayerHealth>();
            playerHealth.TakeDamage(damageOnCollision);        
        }
    }

}
