using System;
using System.Collections.Generic;
using UnityEngine;
using Firebase;
using Firebase.Database;
using Firebase.Auth;
using System.Threading.Tasks;

[Serializable]
public class RankingData
{
    public string playerName;
    public int score;
    public string date;

    public RankingData(string playerName, int score)
    {
        this.playerName = playerName;
        this.score = score;
    }
}

public class FirebaseControlador
{
    internal FirebaseAuth auth = null;
    internal FirebaseUser user = null;
    internal DatabaseReference database = null;

    public async Task InitializeFirebaseAsync()
    {
        var dependencyStatus = await FirebaseApp.CheckAndFixDependenciesAsync();
        if (dependencyStatus == DependencyStatus.Available)
        {
            auth = FirebaseAuth.DefaultInstance;
            user = auth.CurrentUser;
            database = FirebaseDatabase.GetInstance("https://ocaunity-default-rtdb.europe-west1.firebasedatabase.app/").RootReference;
            Debug.Log("Firebase inicializado correctamente.");
        }
        else
        {
            Debug.LogError($"No se pudo inicializar Firebase: {dependencyStatus}");
        }
    }


    // Añadir jugador al ranking, usando la posición como clave
    public async Task AddPlayerToRankingAsync(string playerName, int position)
    {
        if (database == null) return;

        var updates = new Dictionary<string, object>
        {
            [$"/LeaderBoard/User_{position}/username"] = playerName,
            [$"/LeaderBoard/User_{position}/score"] = position // o el score real
        };

        await database.UpdateChildrenAsync(updates);
    }


    // Obtener todos los jugadores del ranking
    public Task<DataSnapshot> GetRankingAsync()
    {
        return database.Child("LeaderBoard").GetValueAsync();
    }
}
