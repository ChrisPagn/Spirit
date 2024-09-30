using UnityEngine;

/// <summary>
/// Classe qui gère le mouvement du joueur dans un jeu 2D.
/// Utilise un Rigidbody2D pour déplacer le joueur en fonction des inputs.
/// </summary>
public class PlayerMovement : MonoBehaviour
{
    /// <summary>
    /// Vitesse de déplacement du joueur.
    /// </summary>
    public float moveSpeed;

    /// <summary>
    /// Vitesse de déplacement du joueur.
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
    /// Rayon de vérification du sol.
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
    /// Transform de vérification du sol.
    /// </summary>
    public Transform groundCheck;

    /// <summary>
    /// Vecteur pour stocker la vélocité de l'objet.
    /// </summary>
    private Vector2 velocity = Vector2.zero; // Utilisation de Vector2 pour les mouvements en 2D

    /// <summary>
    /// Méthode appelée au démarrage du script.
    /// Initialise le Rigidbody2D si non assigné dans l'éditeur.
    /// </summary>
    void Start()
    {
        // Vérifie si le Rigidbody2D est assigné, sinon essaie de le récupérer automatiquement
        if (rigidbody2D == null)
        {
            rigidbody2D = GetComponent<Rigidbody2D>();
        }
    }

    /// <summary>
    /// Méthode appelée à chaque frame, utilisée pour gérer les inputs du joueur.
    /// </summary>
    void Update()
    {
        // Récupère l'input horizontal ("Horizontal" fait référence aux touches fléchées ou aux sticks analogiques)
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        
        // Récupère l'input vertical ("Vertical" fait référence aux touches fléchées ou aux sticks analogiques)
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;
        
        // Vérifie si le joueur est en train de sauter (barre espace)
        if (Input.GetButtonDown("Jump") && isGrounded)
        {
            isJumping = true;
        }

        // Appelle de la méthode pour l'inversion du sens du personnage lors des déplacements
        Flip(rigidbody2D.velocity.x);

        // Calcul de la vitesse du personnage (Player) et renvoie tjs une valeur positive (Problème du gauche - droite ou arrière - avant)
        float characterVelocity = Mathf.Abs(velocity.x);
        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    /// <summary>
    /// Méthode appelée à intervalles fixes, utilisée pour appliquer la physique.
    /// Gère les déplacements du joueur basés sur les inputs.
    /// </summary>
    void FixedUpdate()
    {
        // Vérifier si le joueur est au sol
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
    /// Vitesse horizontale à appliquer au joueur
    /// </summary>
    void MovePlayer(float _horizontalMovement, float _verticalMovement)
    {
        if (!isClimbing)
        {
            // Crée un vecteur avec la vitesse horizontale et la vitesse verticale actuelle
            Vector2 targetVelocity = new Vector2(_horizontalMovement, rigidbody2D.velocity.y);
            // Applique un lissage à la vélocité du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la vélocité actuelle et la cible
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
            // Crée un vecteur avec la vitesse vertical et la vitesse verticale actuelle (0 pour que le personnage s'arrete lorsque qu'il attrape l'echelle)
            Vector2 targetVelocity = new Vector2(0, _verticalMovement);

            // Applique un lissage à la vélocité du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la vélocité actuelle et la cible
            rigidbody2D.velocity = Vector2.SmoothDamp(rigidbody2D.velocity, targetVelocity, ref velocity, .05f);

        }

        
    }

    /// <summary>
    /// Inverse le sens du personnage lors des déplacements.
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
    /// Méthode appelée pour dessiner les gizmos.
    /// </summary>
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }

    /// <summary>
    /// Méthode appelée au démarrage du script.
    /// Empêche la destruction du joueur lors du changement de scène.
    /// </summary>
    void Awake()
    {
        DontDestroyOnLoad(gameObject);
        Debug.LogWarning("Player Not Destroy change scene!");
    }
}