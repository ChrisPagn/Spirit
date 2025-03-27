using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;
using Firebase;

public class FirebaseEmailAuthentication : MonoBehaviour
{
    [Header("Authentication Inputs")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button connectAndStartGameButton;
    public Button registerButton;
    public TextMeshProUGUI feedbackText;
    /// <summary>
    /// Nom de la scène à charger lorsque le jeu commence.
    /// </summary>
    public string levelToLoad;

    [Header("Game Configuration")]

    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        if (connectAndStartGameButton != null)
        {
            connectAndStartGameButton.onClick.AddListener(ConnectToFirebase);
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
                ShowFeedback($"Connexion reussie : {user.Email}", Color.green);
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

            ShowFeedback($"Compte créé ! ID : {user.UserId}", Color.green);
        }
        catch (FirebaseException ex)
        {
            ShowFeedback($"Erreur : {ex.Message}", Color.red);
        }
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
