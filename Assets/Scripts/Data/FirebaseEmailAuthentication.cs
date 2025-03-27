using UnityEngine;
using Firebase.Auth;
using System.Threading.Tasks;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

/// <summary>
/// Gère l'authentification par email et mot de passe avec Firebase
/// Handles email and password authentication with Firebase
/// </summary>
public class FirebaseEmailAuthentication : MonoBehaviour
{
    [Header("Authentication Inputs")]
    public TMP_InputField emailInputField;
    public TMP_InputField passwordInputField;
    public Button connectAndStartGameButton;
    public Button registerButton;
    public TextMeshProUGUI feedbackText;

    [Header("Game Configuration")]
    public string gameSceneName = "MainGameScene";

    private FirebaseAuth auth;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;

        // Vérification null sécurisée
        if (connectAndStartGameButton != null)
        {
            connectAndStartGameButton.onClick.AddListener(ConnectAndStartGame);
        }
        else
        {
            Debug.LogError("Connect and Start Game Button is missing!");
        }

        // Ajoutez également un listener pour le bouton d'inscription
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
    /// Connecte l'utilisateur et démarre le jeu
    /// Connect user and start the game
    /// </summary>
    public async void ConnectAndStartGame()
    {
        // Vérification null supplémentaire
        if (connectAndStartGameButton == null)
        {
            Debug.LogError("Bouton de connexion détruit. Impossible de continuer.");
            return;
        }

        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowFeedback("Veuillez saisir un email et un mot de passe", Color.red);
            return;
        }

        // Méthode sécurisée pour désactiver le bouton
        SetButtonInteractable(false);
        ShowFeedback("Connexion en cours...", Color.yellow);

        try
        {
            FirebaseUser user = await SignInWithEmailAndPasswordAsync(
                emailInputField.text,
                passwordInputField.text
            );

            if (user != null)
            {
                ShowFeedback("Connexion réussie !", Color.green);
                await DataOrchestrator.instance.LoadData();
                SceneManager.LoadScene(gameSceneName);
            }
        }
        catch (System.Exception ex)
        {
            ShowFeedback($"Erreur de connexion : {ex.Message}", Color.red);
        }
        finally
        {
            // Réactiver le bouton de manière sécurisée
            SetButtonInteractable(true);
        }
    }

    /// <summary>
    /// Méthode asynchrone pour la connexion Firebase
    /// Asynchronous method for Firebase sign-in
    /// </summary>
    private Task<FirebaseUser> SignInWithEmailAndPasswordAsync(string email, string password)
    {
        // Utiliser TaskCompletionSource pour gérer l'authentification asynchrone
        // Use TaskCompletionSource to manage asynchronous authentication
        var tcs = new TaskCompletionSource<FirebaseUser>();
        auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled)
            {
                tcs.SetCanceled();
                return;
            }
            if (task.IsFaulted)
            {
                tcs.SetException(task.Exception);
                return;
            }
            // Connexion réussie
            // Successful sign-in
            tcs.SetResult(task.Result.User);
        });
        return tcs.Task;
    }

    /// <summary>
    /// Méthode sécurisée pour définir l'interactivité du bouton
    /// </summary>
    private void SetButtonInteractable(bool interactable)
    {
        if (connectAndStartGameButton != null)
        {
            connectAndStartGameButton.interactable = interactable;
        }
    }

    /// <summary>
    /// Crée un nouveau compte utilisateur
    /// </summary>
    public void CreateUserWithEmailAndPassword()
    {
        if (string.IsNullOrEmpty(emailInputField.text) || string.IsNullOrEmpty(passwordInputField.text))
        {
            ShowFeedback("Veuillez saisir un email et un mot de passe", Color.red);
            return;
        }

        auth.CreateUserWithEmailAndPasswordAsync(emailInputField.text, passwordInputField.text)
            .ContinueWith(task =>
            {
                if (task.IsCanceled)
                {
                    ShowFeedback("Création de compte annulée", Color.yellow);
                    return;
                }

                if (task.IsFaulted)
                {
                    ShowFeedback("Erreur de création de compte : " + task.Exception.ToString(), Color.red);
                    return;
                }

                AuthResult result = task.Result;
                FirebaseUser user = result.User;
                ShowFeedback("Compte créé ! ID : " + user.UserId, Color.green);
            });
    }

    /// <summary>
    /// Affiche un message de feedback à l'utilisateur
    /// Display feedback message to user
    /// </summary>
    private void ShowFeedback(string message, Color color)
    {
        // Assurer l'exécution sur le thread principal
        // Ensure execution on the main thread
        if (feedbackText != null)
        {
            feedbackText.text = message;
            feedbackText.color = color;
        }
    }
}