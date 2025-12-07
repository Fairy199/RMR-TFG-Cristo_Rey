using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public Transform contentPanel;      // El Content del Scroll
    public GameObject entryPrefab;      // Prefab con el script LeaderboardEntry

    private FirebaseControlador firebase;

    private async void Start()
    {
        // Inicializar Firebase
        firebase = new FirebaseControlador();
        await firebase.InitializeFirebaseAsync();

        // Cargar leaderboard
        await LoadLeaderboard();
    }

    public async Task LoadLeaderboard()
    {
        if (firebase == null || firebase.database == null)
        {
            Debug.LogError("Firebase no está inicializado correctamente.");
            return;
        }

        var snapshot = await firebase.GetRankingAsync();

        if (snapshot == null || !snapshot.Exists)
        {
            Debug.Log("No hay datos de ranking");
            return;
        }

        // Limpiar contenido previo
        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        // Recorrer cada usuario en LeaderBoard
        foreach (var child in snapshot.Children)
        {
            var usernameValue = child.Child("username")?.Value;
            var scoreValue = child.Child("score")?.Value;

            if (usernameValue == null || scoreValue == null)
            {
                Debug.LogWarning($"Usuario {child.Key} tiene datos inválidos.");
                continue;
            }

            string username = usernameValue.ToString();
            string score = scoreValue.ToString();

            // Instanciar prefab
            GameObject newEntry = Instantiate(entryPrefab, contentPanel);
            newEntry.transform.localScale = Vector3.one;

            // Asignar valores usando el script LeaderboardEntry
            var entryScript = newEntry.GetComponent<LeaderBoardEntry>();
            if (entryScript != null)
            {
                entryScript.usernameText.text = username;
                entryScript.scoreText.text = score;
            }
            else
            {
                Debug.LogError("El prefab necesita el script LeaderboardEntry con referencias asignadas.");
                Destroy(newEntry);
            }
        }
    }
}
