using System;
using System.Collections.Generic;
using System.Threading.Tasks;  
using UnityEngine;
using Firebase;
using Firebase.Auth;           
using Firebase.Database;       

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

    /// <summary>
    /// Registrar un usuario nuevo en Firebase Realtime Database
    /// Esto solo se llama en el registro, no en login.
    /// </summary>
    public async Task RegisterNewUserAsync(FirebaseUser newUser)
    {
        if (newUser == null)
        {
            Debug.LogWarning("No hay usuario a registrar.");
            return;
        }

        string uid = newUser.UserId;
        string nickname = newUser.DisplayName ?? "Jugador";

        // Comprobar si el usuario ya existe
        var snapshot = await database.Child("LeaderBoard").Child(uid).GetValueAsync();
        if (!snapshot.Exists)
        {
            // Crear usuario con score = 0 solo si no existe
            var updates = new Dictionary<string, object>
            {
                ["username"] = nickname,
                ["score"] = 0
            };

            await database.Child("LeaderBoard").Child(uid).UpdateChildrenAsync(updates);
            Debug.Log($"Usuario {nickname} añadido al LeaderBoard con score 0.");
        }
        else
        {
            Debug.Log($"Usuario {nickname} ya existe, no se modifica score.");
        }
    }

    /// <summary>
    /// Llamar en login para obtener usuario autenticado sin tocar score
    /// </summary>
    public async Task SetLoggedInUserAsync(FirebaseUser loggedInUser)
    {
        if (loggedInUser == null)
        {
            Debug.LogWarning("No hay usuario autenticado.");
            return;
        }

        user = loggedInUser;

        // Actualizar solo el nombre del usuario en la base de datos
        string nickname = user.DisplayName ?? "Jugador";
        await database.Child("LeaderBoard").Child(user.UserId).Child("username").SetValueAsync(nickname);
        Debug.Log($"Usuario {nickname} ha iniciado sesión. Score no modificado.");
    }

    public Task<DataSnapshot> GetRankingAsync()
    {
        return database.Child("LeaderBoard").GetValueAsync();
    }
}
