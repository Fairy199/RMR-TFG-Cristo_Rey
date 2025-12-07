using UnityEngine;
using TMPro;
using Firebase.Auth;

public class MenuManager : MonoBehaviour
{
    public TMP_Text welcomeText; // Arrastra tu TextMeshPro al inspector
    private FirebaseAuth auth;
    private FirebaseUser user;

    private void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = auth.CurrentUser;

        if (user != null)
        {
            string nickname = user.DisplayName; // Esto es el nickname que pusiste al registrar
            welcomeText.text = $"Bienvenido {nickname}";
        }
        else
        {
            welcomeText.text = "Bienvenido jugador";
        }
    }
}
