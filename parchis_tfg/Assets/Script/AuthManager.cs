using UnityEngine;
using Firebase;
using Firebase.Auth;
using TMPro;
using System.Collections;
using UnityEngine.SceneManagement;

public class AuthManager : MonoBehaviour
{
    [Header("Firebase")]
    public DependencyStatus dependencyStatus;
    public FirebaseAuth auth;
    public FirebaseUser user;

    [Header("Login")]
    public TMP_InputField emailLoginField;
    public TMP_InputField passwordLoginField;
    public TMP_Text warningLoginText;
    public TMP_Text confirmLoginText;

    [Header("Register")]
    public TMP_InputField usernameRegisterField;
    public TMP_InputField emailRegisterField;
    public TMP_InputField passwordRegisterField;
    public TMP_InputField passwordConfirmField;
    public TMP_Text warningRegisterText;
    public TMP_Text confirmRegisterText;

    [Header("Panels")]                     
    public GameObject PanelLogin;
    public GameObject PanelRegister;

    [Header("Buttons")]                    
    public GameObject loginButtons;
    public GameObject registerButtons;

    private void Start()
    {
        FirebaseApp.CheckAndFixDependenciesAsync().ContinueWith(task =>
        {
            dependencyStatus = task.Result;
            if (dependencyStatus == DependencyStatus.Available)
                auth = FirebaseAuth.DefaultInstance;
            else
                Debug.LogError("Could not resolve all Firebase dependencies: " + dependencyStatus);
        });
    }

    // ---------------- PANEL SWITCH -----------  <<< NUEVO
    public void OpenRegisterPanel()
    {
        PanelLogin.SetActive(false);
        PanelRegister.SetActive(true);
    }

    public void OpenLoginPanel()
    {
        PanelRegister.SetActive(false);
        PanelLogin.SetActive(true);
    }

    // ---------------- LOGIN ---------------------
    public void LoginUser()
    {
        StartCoroutine(Login(emailLoginField.text, passwordLoginField.text));
    }

    private IEnumerator Login(string email, string password)
    {
        var loginTask = auth.SignInWithEmailAndPasswordAsync(email, password);

        yield return new WaitUntil(() => loginTask.IsCompleted);

        if (loginTask.Exception != null)
        {
            warningLoginText.text = "Login failed";
        }
        else
        {
            user = loginTask.Result.User;
            confirmLoginText.text = "Login successful!";
            SceneManager.LoadScene("Menu");
        }
    }

    // ---------------- REGISTER ---------------------
    public void RegisterUser()
    {
        StartCoroutine(Register(
            usernameRegisterField.text,
            emailRegisterField.text,
            passwordRegisterField.text,
            passwordConfirmField.text));
    }

    private IEnumerator Register(string username, string email, string password, string confirmPassword)
    {
        // Validaciones
        if (username.Length < 3)
        {
            warningRegisterText.text = "Username too short";
            yield break;
        }

        if (password != confirmPassword)
        {
            warningRegisterText.text = "Passwords do not match";
            yield break;
        }

        var registerTask = auth.CreateUserWithEmailAndPasswordAsync(email, password);
        yield return new WaitUntil(() => registerTask.IsCompleted);

        if (registerTask.Exception != null)
        {
            warningRegisterText.text = "Register failed";
        }
        else
        {
            user = registerTask.Result.User;

            // Actualizar nombre visible
            UserProfile profile = new UserProfile { DisplayName = username };
            var profileTask = user.UpdateUserProfileAsync(profile);

            yield return new WaitUntil(() => profileTask.IsCompleted);

            if (profileTask.Exception != null)
            {
                warningRegisterText.text = "User created, but name couldn't be set";
            }
            else
            {
                confirmRegisterText.text = "Register successful!";

                // <<< DESACTIVAR BOTONES
                registerButtons.SetActive(false);

                // <<< ESPERAR 2 SEGUNDOS Y VOLVER
                StartCoroutine(ReturnToLoginAfterDelay());
            }
        }
    }

    // <<< NUEVO
    private IEnumerator ReturnToLoginAfterDelay()
    {
        yield return new WaitForSeconds(2f);

        // Volver al Login
        PanelRegister.SetActive(false);
        PanelLogin.SetActive(true);

        // Reactivar los botones del registro para la próxima vez
        registerButtons.SetActive(true);

        // Limpiar campos y textos
        confirmRegisterText.text = "";
        warningRegisterText.text = "";
        usernameRegisterField.text = "";
        emailRegisterField.text = "";
        passwordRegisterField.text = "";
        passwordConfirmField.text = "";
    }
}
