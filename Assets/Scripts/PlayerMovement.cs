using UnityEngine;

/// <summary>
/// Classe qui g�re le mouvement du joueur dans un jeu 2D.
/// Utilise un Rigidbody2D pour d�placer le joueur en fonction des inputs.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Vitesse de d�placement du joueur.
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Vitesse de d�placement du joueur.
    /// </summary>
    public float climbSpeed;

    /// <summary>
    /// Force de saut du joueur.
    /// </summary>
    public float jumpForce;

    /// <summary>
    /// Mouvement horizontal du joueur.
    /// </summary>
    private float horizontalMovement;

    /// <summary>
    /// Mouvement vertical du joueur.
    /// </summary>
    private float verticalMovement;

    /// <summary>
    /// Rayon de v�rification du sol.
    /// </summary>
    public float groundCheckRadius;

    /// <summary>
    /// Indicateur si le joueur est en train de sauter.
    /// </summary>
    public bool isJumping = false;

    /// <summary>
    /// Indicateur si le joueur est au sol.
    /// </summary>
    public bool isGrounded = false;

    /// <summary>
    /// Indicateur si le joueur en train de grimper.
    /// </summary>
    [HideInInspector]  
    public bool isClimbing = false;

    /// <summary>
    /// Rigidbody2D du joueur.
    /// </summary>
    public Rigidbody2D rigidbody2D;

    /// <summary>
    /// Animator du joueur.
    /// </summary>
    public Animator animator;

    /// <summary>
    /// SpriteRenderer du joueur.
    /// </summary>
    public SpriteRenderer spriteRenderer;

    /// <summary>
    /// LayerMask des couches de collision.
    /// </summary>
    public LayerMask collinsionLayers;

    /// <summary>
    /// Transform de v�rification du sol.
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Vecteur pour stocker la v�locit� de l'objet.
    /// </summary>
    private Vector2 velocity = Vector2.zero; // Utilisation de Vector2 pour les mouvements en 2D

    /// <summary>
    /// M�thode appel�e au d�marrage du script.
    /// Initialise le Rigidbody2D si non assign� dans l'�diteur.
    /// </summary>
    void Start()
    {
        // V�rifie si le Rigidbody2D est assign�, sinon essaie de le r�cup�rer automatiquement
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    /// <summary>
    /// M�thode appel�e � chaque frame, utilis�e pour g�rer les inputs du joueur.
    /// </summary>
    void Update()
    {
        // R�cup�re l'input horizontal ("Horizontal" fait r�f�rence aux touches fl�ch�es ou aux sticks analogiques)
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        
        // R�cup�re l'input vertical ("Vertical" fait r�f�rence aux touches fl�ch�es ou aux sticks analogiques)
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
        
        // V�rifie si le joueur est en train de sauter (barre espace)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        // Appelle de la m�thode pour l'inversion du sens du personnage lors des d�placements
        Flip(rigidbody2D.velocity.x);

        // Calcul de la vitesse du personnage (Player) et renvoie tjs une valeur positive (Probl�me du gauche - droite ou arri�re - avant)
        float characterVelocity = Mathf.Abs(velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    /// <summary>
    /// M�thode appel�e � intervalles fixes, utilis�e pour appliquer la physique.
    /// G�re les d�placements du joueur bas�s sur les inputs.
    /// </summary>
    void FixedUpdate()
    {
        // V�rifier si le joueur est au sol
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collinsionLayers);

        // Si le joueur est en train de grimper, ne pas autoriser les sauts et mouvements horizontaux classiques
        if (!isClimbing)
        {
            MovePlayer(horizontalMovement, 0); // Pas de mouvement vertical si on ne grimpe pas
        }
        else
        {
            MovePlayer(0, verticalMovement); // Mouvement vertical uniquement lors de l'escalade
        }
    }


    /// <summary>
    /// Applique le mouvement au joueur en lissant la transition vers la vitesse cible.
    /// Vitesse horizontale � appliquer au joueur
    /// </summary>
    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            // Cr�e un vecteur avec la vitesse horizontale et la vitesse verticale actuelle
            Vector2 targetVelocity = new Vector2(_horizontalMovement, rigidbody2D.velocity.y);
            // Applique un lissage � la v�locit� du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la v�locit� actuelle et la cible
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, .05f);
            if (isJumping == true)
            {
                rigidbody2D.AddForce(new Vector2(0f, jumpForce));
                isJumping = false;
            }
        }
        else
        {
            // Deplacement vertical
            // Cr�e un vecteur avec la vitesse vertical et la vitesse verticale actuelle (0 pour que le personnage s'arrete lorsque qu'il attrape l'echelle)
            Vector2 targetVelocity = new Vector2(0, _verticalMovement);

            // Applique un lissage � la v�locit� du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la v�locit� actuelle et la cible
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, .05f);

        }

        
    }

    /// <summary>
    /// Inverse le sens du personnage lors des d�placements.
    /// </summary>
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

    /// <summary>
    /// M�thode appel�e pour dessiner les gizmos.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    /// <summary>
    /// M�thode appel�e au d�marrage du script.
    /// Emp�che la destruction du joueur lors du changement de sc�ne.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.LogWarning("Player Not Destroy change scene!");
    }
}