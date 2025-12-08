using System;
using System.Collections.Generic;
using System.Threading.Tasks;   // <- Para Task
using UnityEngine;
using Firebase;
using Firebase.Auth;           // <- Para FirebaseAuth y FirebaseUser
using Firebase.Database;       // <- Para DatabaseReference y DataSnapshot

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

            if (user != null)
                await AddAuthenticatedUserToDatabaseAsync();
        }
        else
        {
            Debug.LogError($"No se pudo inicializar Firebase: {dependencyStatus}");
        }
    }

    public async Task AddAuthenticatedUserToDatabaseAsync()
    {
        if (user == null)
        {
            Debug.LogWarning("No hay usuario autenticado.");
            return;
        }

        string uid = user.UserId;
        string nickname = user.DisplayName ?? "Jugador";

        var updates = new Dictionary<string, object>
        {
            [$"/LeaderBoard/{uid}/username"] = nickname,
            [$"/LeaderBoard/{uid}/score"] = 0
        };

        await database.UpdateChildrenAsync(updates);
        Debug.Log($"Usuario {nickname} añadido al LeaderBoard con score 0.");
    }

    public Task<DataSnapshot> GetRankingAsync()
    {
        return database.Child("LeaderBoard").GetValueAsync();
    }
}
