using UnityEngine;

/// Classe qui g�re le mouvement du joueur dans un jeu 2D.
/// Utilise un Rigidbody2D pour d�placer le joueur en fonction des inputs.
public class PlayerMovement : MonoBehaviour
{
    public float moveSpeed;
    public float jumpForce;
    private float horizontalMovement;
    public float groundCheckRadius;
    public bool isJumping = false;
    public bool isGrounded = false;

    public new Rigidbody2D rigidbody2D;
    public Animator animator;
    public SpriteRenderer spriteRenderer;
    public LayerMask collinsionLayers;
    public Transform groundCheck;
    


    // Vecteur pour stocker la v�locit� de l'objet
    private Vector2 velocity = Vector2.zero; // Utilisation de Vector2 pour les mouvements en 2D

    /// M�thode appel�e au d�marrage du script.
    /// Initialise le Rigidbody2D si non assign� dans l'�diteur.
    void Start()
    {
        // V�rifie si le Rigidbody2D est assign�, sinon essaie de le r�cup�rer automatiquement
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    /// M�thode appel�e � chaque frame, utilis�e pour g�rer les inputs du joueur.
    
    void Update()
    {
        


        // R�cup�re l'input horizontal ("Horizontal" fait r�f�rence aux touches fl�ch�es ou aux sticks analogiques)
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;

        //verifie si le joueur est entrain de sauter (barre espace)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        

        //Appelle de la methode pour l'inversion du sens du personnage lors des d�placements
        Flip(rigidbody2D.velocity.x);

        // Calcul de la vitesse du personnage (Player) et renvoie tjs une valeur possitive (Probleme du gauche - droite ou arriere - avant)
        float characterVelocity = Mathf.Abs(velocity.x);
        animator.SetFloat("Speed", characterVelocity);
    }

    // M�thode appel�e � intervalles fixes, utilis�e pour appliquer la physique.
    // G�re les d�placements du joueur bas�s sur les inputs.
    void FixedUpdate()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collinsionLayers);

        // Appelle la fonction pour d�placer le joueur avec l'input r�cup�r�
        MovePlayer(horizontalMovement);
    }

    // Applique le mouvement au joueur en lissant la transition vers la vitesse cible.
    // <param name="_horizontalMovement">Vitesse horizontale � appliquer au joueur</param>
    void MovePlayer(float _horizontalMovement)
    {
        // Cr�e un vecteur avec la vitesse horizontale et la vitesse verticale actuelle
        Vector2 targetVelocity = new Vector2(_horizontalMovement, rigidbody2D.velocity.y);

        // Applique un lissage � la v�locit� du joueur pour un mouvement plus fluide
        // SmoothDamp lisse la transition entre la v�locit� actuelle et la cible
        rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, .05f);


        if (isJumping == true)
        {
            rigidbody2D.AddForce(new Vector2(0f, jumpForce));
            isJumping=false;
        }
    }

    void Flip(float _velocity)
    {
        if (_velocity > 0.1f)
        {
            spriteRenderer.flipX = false;
        }
        else if (_velocity < -0.1f)
        {
            spriteRenderer.flipX = true;
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    // Dans le script du joueur
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.LogWarning("Player Not Destroy change scene!");
    }
}
