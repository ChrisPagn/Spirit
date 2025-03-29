using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;

/// <summary>
/// Classe responsable de la gestion de l'authentification par email via Firebase depuis Unity.
/// </summary>
public class FirebaseEmailAuthentication : MonoBehaviour
{
    //L'attribut [Header("Game Configuration")] est utilisé dans Unity pour organiser l'inspecteur
    //de l'éditeur. Il ajoute une section intitulée "Game Configuration" dans l'inspecteur, ce qui
    //permet de regrouper visuellement les variables qui suivent cet attribut
    [Header("Authentication Inputs")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public TMP_InputField displayNameInputField;
    public Button connectAndStartGameButton;
    public Button registerButton;
    public TextMeshProUGUI feedbackText;
    public string levelToLoad;
    public string idUser;

    //L'attribut [Header("Authentication Inputs")] fonctionne de manière similaire à
    //[Header("Game Configuration")]. Il crée une section intitulée
    //"Authentication Inputs" dans l'inspecteur Unity
    [Header("Game Configuration")]

    private FirebaseAuth auth;

    /// <summary>
    /// Instance unique de la classe FirebaseEmailAuthentication.
    /// </summary>
    public static FirebaseEmailAuthentication instance;

    /// <summary>
    /// Initialise l'instance unique de FirebaseEmailAuthentication.
    /// </summary>
    private void Awake()
    {
        if (instance != null)
        {
            Debug.LogWarning("Il y a plus d'une instance FirebaseEmailAuthentication dans la scène");
            return;
        }

        instance = this;
    }

    /// <summary>
    /// Initialise les composants et les écouteurs d'événements au démarrage.
    /// </summary>
    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (connectAndStartGameButton != null)
        {
            connectAndStartGameButton.onClick.AddListener(HandleAuthentication);
        }
        else
        {
            Debug.LogError("Connect and Start Game Button is missing!");
        }

        if (registerButton != null)
        {
            registerButton.onClick.AddListener(CreateUserWithEmailAndPassword);
        }
        else
        {
            Debug.LogError("Register Button is missing!");
        }
    }

    /// <summary>
    /// Tente de connecter un utilisateur à Firebase avec email et mot de passe.
    /// S'assure que l'utilisateur est bien authentifié avant de charger la scène.
    /// </summary>
    public async void ConnectToFirebase()
    {
        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowFeedback("Veuillez saisir un email et un mot de passe", Color.red);
            return;
        }

        SetButtonInteractable(false);
        ShowFeedback("Connexion en cours...", Color.yellow);

        try
        {
            // Vérifier si un utilisateur anonyme est connecté et le supprimer
            FirebaseUser currentUser = auth.CurrentUser;
            if (currentUser != null && currentUser.IsAnonymous)
            {
                await currentUser.DeleteAsync();
                Debug.Log("Utilisateur anonyme supprimé.");
            }

            // Connexion avec email et mot de passe
            FirebaseUser user = (await auth.SignInWithEmailAndPasswordAsync(
                emailInputField.text,
                passwordInputField.text
            )).User;

            if (user != null)
            {
                ShowFeedback($"Connexion reussie : {user.Email}, {user.UserId}, {displayNameInputField.text}", Color.black);

                // Chargement des données
                await DataOrchestrator.instance.LoadData();

                SceneManager.LoadScene(levelToLoad); // Charger la scène de jeu
            }
        }
        catch (FirebaseException ex)
        {
            ShowFeedback($"Erreur : {ex.Message}", Color.red);
        }
        finally
        {
            SetButtonInteractable(true);
        }
    }

    /// <summary>
    /// Crée un nouveau compte utilisateur sur Firebase.
    /// </summary>
    public async void CreateUserWithEmailAndPassword()
    {
        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowFeedback("Veuillez saisir un email et un mot de passe", Color.red);
            return;
        }

        try
        {
            FirebaseUser user = (await auth.CreateUserWithEmailAndPasswordAsync(
                emailInputField.text,
                passwordInputField.text
            )).User;

            ShowFeedback($"Compte cree ! ID : {user.Email}, {user.UserId}", Color.black);
        }
        catch (FirebaseException ex)
        {
            ShowFeedback($"Erreur : {ex.Message}", Color.red);
        }
    }

    /// <summary>
    /// Gère l'authentification de l'utilisateur, soit en se connectant, soit en créant un nouveau compte.
    /// </summary>
    public async void HandleAuthentication()
    {
        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowFeedback("Veuillez saisir un email et un mot de passe", Color.red);
            return;
        }

        SetButtonInteractable(false);
        ShowFeedback("Connexion en cours...", Color.yellow);

        try
        {
            // Vérifier si un utilisateur anonyme est connecté et le supprimer
            FirebaseUser currentUser = auth.CurrentUser;
            if (currentUser != null && currentUser.IsAnonymous)
            {
                await currentUser.DeleteAsync();
                Debug.Log("Utilisateur anonyme supprimé.");
            }

            // Tenter de se connecter avec email et mot de passe
            FirebaseUser user = (await auth.SignInWithEmailAndPasswordAsync(
                emailInputField.text,
                passwordInputField.text
            )).User;

            if (user != null)
            {
                ShowFeedback($"Connexion reussie : {user.Email}, {user.UserId}", Color.black);
                // Charger les données après authentification
                await DataOrchestrator.instance.LoadData();
                SceneManager.LoadScene(levelToLoad); // Charger la scène de jeu
            }
            else
            {
                // Si la connexion échoue, proposer la création de compte
                ShowFeedback("Compte non trouve. Creation de compte...", Color.yellow);
                user = (await auth.CreateUserWithEmailAndPasswordAsync(
                    emailInputField.text,
                    passwordInputField.text
                )).User;

                if (user != null)
                {
                    ShowFeedback($"Compte cree ! ID : {user.Email}, {user.UserId}", Color.black);
                    // Sauvegarder les données locales pour un nouveau compte
                    await DataOrchestrator.instance.SaveData();
                    SceneManager.LoadScene(levelToLoad); // Charger la scène de jeu
                }
                else
                {
                    ShowFeedback("Erreur lors de la creation du compte.", Color.red);
                }
            }
        }
        catch (FirebaseException ex)
        {
            HandleAuthenticationError(ex);
        }
        finally
        {
            SetButtonInteractable(true);
        }
    }

    /// <summary>
    /// Gère les erreurs d'authentification et affiche un message approprié à l'utilisateur.
    /// </summary>
    private async void HandleAuthenticationError(FirebaseException ex)
    {
        string message = "Erreur inconnue";

        Debug.LogError($"HandleAuthenticationError appelé avec : {ex.ErrorCode} - {ex.Message}"); // Vérification

        switch ((AuthError)ex.ErrorCode)
        {
            case AuthError.MissingEmail:
                message = "L'email est requis.";
                break;
            case AuthError.WrongPassword:
                message = "Mot de passe incorrect.";
                break;
            case AuthError.UserNotFound:
                message = "Aucun compte trouvé avec cet email.";
                break;
            case AuthError.EmailAlreadyInUse:
                message = "Cet email est déjà utilisé.";
                break;
            case AuthError.InvalidEmail:
                message = "Email invalide.";
                break;
            default:
                message = ex.Message;
                break;
        }

        ShowFeedback($"Erreur : {message}", Color.red);
        feedbackText.gameObject.SetActive(true);

        // Attendre 3 secondes pour s'assurer que le message est affiché
        await Task.Delay(3000);
    }

    /// <summary>
    /// Active ou désactive le bouton de connexion.
    /// </summary>
    private void SetButtonInteractable(bool interactable)
    {
        if (connectAndStartGameButton != null)
        {
            connectAndStartGameButton.interactable = interactable;
        }
    }

    /// <summary>
    /// Affiche un message à l'utilisateur.
    /// </summary>
    private void ShowFeedback(string message, Color color)
    {
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }
}
