using System.Collections;
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
    public bool IsOnGround = false;

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
    public new Rigidbody2D rigidbody;
 
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
    /// Sphere de collision autour du joueur
    /// </summary>
    public CapsuleCollider2D playerCollider;

    /// <summary>
    /// singleton de l'instance
    /// </summary>
    public static PlayerMovement instance;

    /// <summary>
    /// M�thode appel�e au d�marrage du script.
    /// Emp�che la destruction du joueur lors du changement de sc�ne.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance PlayerMovement dans la sc�ne");
            return;
        }

        instance = this;

        // V�rifie si le Rigidbody2D est assign�, sinon essaie de le r�cup�rer automatiquement
        if (rigidbody == null)
        {
            rigidbody = GetComponent<Rigidbody2D>();
        }

        Debug.LogWarning("Player Not Destroy change scene!");
    }

    /// <summary>
    /// M�thode appel�e � chaque frame, utilis�e pour g�rer les inputs du joueur.
    /// </summary>
    void Update()
    {
        // R�cup�re l'input horizontal ("Horizontal" fait r�f�rence aux touches fl�ch�es ou aux sticks analogiques)
        horizontalMovement = Input.GetAxis("Horizontal") * moveSpeed * Time.fixedDeltaTime;
        
        // R�cup�re l'input vertical ("Vertical"DDD fait r�f�rence aux touches fl�ch�es ou aux sticks analogiques)
        verticalMovement = Input.GetAxis("Vertical") * climbSpeed * Time.fixedDeltaTime;

        // V�rifie si le joueur est en train de sauter (barre espace)
        if (Input.GetButtonDown("Jump") && IsOnGround && !isClimbing)
        {
            IsOnGround = false;
            rigidbody.AddForce( transform.up * jumpForce);
            Debug.Log($"Update(): isJumping: {IsOnGround}, isGrounded: {isGrounded}, isClimbing: {isClimbing}");
        }

        // Appelle de la m�thode pour l'inversion du sens du personnage lors des d�placements
        Flip(rigidbody.velocity.x);

        // Calcul de la vitesse du personnage (Player) et renvoie tjs une valeur positive (Probl�me du gauche - droite ou arri�re - avant)
        float characterVelocity = Mathf.Abs(velocity.x);

        animator.SetFloat("Speed", characterVelocity);
        animator.SetBool("isClimbing", isClimbing);
    }

    /// <summary>
    /// M�thode appel�e � intervalles fixes, V�rifie si le joueur est au sol en utilisant un cercle de d�tection autour de groundCheck
    // groundCheck.position : position du point de contr�le au sol
    // groundCheckRadius : rayon du cercle de d�tection
    // collisionLayers : couche(s) � consid�rer comme sol
    /// </summary>
    void FixedUpdate()
    {
        IsOnGround = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, collinsionLayers);
      
        // Si le joueur n'est pas en train de grimper, on applique un mouvement horizontal classique
        if (!isClimbing)
        {
            // Le joueur peut se d�placer horizontalement, mais pas verticalement
            MovePlayer(horizontalMovement, 0);
        }
        else
        {
            // Si le joueur est en train de grimper, il ne peut se d�placer que verticalement
            MovePlayer(0, verticalMovement);
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
            Vector2 targetVelocity = new Vector2(_horizontalMovement, rigidbody.velocity.y);
            // Applique un lissage � la v�locit� du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la v�locit� actuelle et la cible
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref velocity, .05f);
        }
        else
        {
            // Deplacement vertical
            // Cr�e un vecteur avec la vitesse vertical et la vitesse verticale actuelle (0 pour que le personnage s'arrete lorsque qu'il attrape l'echelle)
            Vector2 targetVelocity = new Vector2(0, _verticalMovement);

            // Applique un lissage � la v�locit� du joueur pour un mouvement plus fluide
            // SmoothDamp lisse la transition entre la v�locit� actuelle et la cible
            rigidbody.velocity = Vector2.SmoothDamp(rigidbody.velocity, targetVelocity, ref velocity, .05f);
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

}