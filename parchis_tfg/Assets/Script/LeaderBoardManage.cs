using UnityEngine;
using Firebase.Database;
using TMPro;
using System.Collections.Generic;
using System.Threading.Tasks;

public class LeaderboardManager : MonoBehaviour
{
    public Transform contentPanel;
    public GameObject entryPrefab;

    private FirebaseControlador firebase;

    private async void Start()
    {
        firebase = new FirebaseControlador();
        await firebase.InitializeFirebaseAsync();

        await LoadLeaderboard();
    }

    public async Task LoadLeaderboard()
    {
        var snapshot = await firebase.GetDataOfRankingFromDatabaseAsync();

        if (snapshot == null || !snapshot.Exists)
        {
            Debug.Log("No hay datos de ranking");
            return;
        }

        List<RankingData> entries = new List<RankingData>();

        foreach (var child in snapshot.Children)
        {
            string json = child.Value.ToString();
            RankingData data = JsonUtility.FromJson<RankingData>(json);
            entries.Add(data);
        }

        // Ordenar por puntuación descendente
        entries.Sort((a, b) => b.score.CompareTo(a.score));

        // Pintar en UI
        foreach (var data in entries)
        {
            GameObject newEntry = Instantiate(entryPrefab, contentPanel);
            newEntry.transform.Find("NameText").GetComponent<TextMeshProUGUI>().text = data.playerName;
            newEntry.transform.Find("ScoreText").GetComponent<TextMeshProUGUI>().text = data.score.ToString();
        }
    }
}
