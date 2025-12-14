using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    [Header("Leaderboard UI")]
    public Transform contentPanel;       
    public GameObject entryPrefab;      

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

        foreach (Transform child in contentPanel)
            Destroy(child.gameObject);

        List<RankingData> rankingList = new List<RankingData>();

        foreach (var child in snapshot.Children)
        {
            var usernameValue = child.Child("username")?.Value;
            var scoreValue = child.Child("score")?.Value;

            if (usernameValue == null || scoreValue == null)
                continue;

            rankingList.Add(new RankingData(
                usernameValue.ToString(),
                int.Parse(scoreValue.ToString())
            ));
        }

        rankingList.Sort((a, b) => b.score.CompareTo(a.score));

        for (int i = 0; i < rankingList.Count; i++)
        {
            GameObject newEntry = Instantiate(entryPrefab, contentPanel);
            newEntry.transform.localScale = Vector3.one;

            var entryScript = newEntry.GetComponent<LeaderBoardEntry>();
            if (entryScript != null)
            {
                entryScript.SetData(
                    i + 1,                            
                    rankingList[i].playerName,       
                    rankingList[i].score             
                );
            }
        }
    }
}
