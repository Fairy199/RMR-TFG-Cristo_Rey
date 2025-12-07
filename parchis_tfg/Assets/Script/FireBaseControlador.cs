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

    // Inicializar Firebase automáticamente usando la configuración de Unity
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

    // Registrar nuevo usuario
    public async Task RegisterNewUserAsync(string email, string password)
    {
        try
        {
            var result = await auth.CreateUserWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"Usuario registrado: {user.Email}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al registrar usuario: {GetLastInnerException(ex).Message}");
            throw GetLastInnerException(ex);
        }
    }

    // Iniciar sesión
    public async Task SignInAsync(string email, string password)
    {
        try
        {
            var result = await auth.SignInWithEmailAndPasswordAsync(email, password);
            user = result.User;
            Debug.Log($"Usuario logueado: {user.Email}");
        }
        catch (Exception ex)
        {
            Debug.LogError($"Error al iniciar sesión: {GetLastInnerException(ex).Message}");
            throw GetLastInnerException(ex);
        }
    }

    // Cerrar sesión
    public void SignOut()
    {
        auth.SignOut();
        user = null;
        Debug.Log("Usuario desconectado.");
    }

    // Obtener la excepción más interna
    internal Exception GetLastInnerException(Exception ex)
    {
        if (ex.InnerException != null) return GetLastInnerException(ex.InnerException);
        return ex;
    }

    // Métodos para base de datos
    internal async Task AddBuyToListAsync(int id)
    {
        if (database == null || user == null) return;
        var updates = new Dictionary<string, object> { [$"/users/{user.UserId}/shopping-car/{id}"] = true };
        await database.UpdateChildrenAsync(updates);
    }

    internal async Task AddEquipmentToListAsync(int id, string type)
    {
        if (database == null || user == null) return;
        var updates = new Dictionary<string, object> { [$"/users/{user.UserId}/equipment/{type}"] = id };
        await database.UpdateChildrenAsync(updates);
    }

    internal async Task AddEquipmentToListMultipleAsync(int shield, int core, int trail)
    {
        if (database == null || user == null) return;
        var updates = new Dictionary<string, object>
        {
            [$"/users/{user.UserId}/equipment/shield"] = shield,
            [$"/users/{user.UserId}/equipment/core"] = core,
            [$"/users/{user.UserId}/equipment/trail"] = trail
        };
        await database.UpdateChildrenAsync(updates);
    }

    public async Task AddDataToRankingAsync(RankingData data)
    {
        if (database == null || user == null || data == null) return;
        string key = database.Child("ranking").Push().Key;
        var updates = new Dictionary<string, object>
        {
            [$"/ranking/{key}"] = JsonUtility.ToJson(data),
            [$"/user-scores/{user.UserId}/{key}"] = JsonUtility.ToJson(data)
        };
        await database.UpdateChildrenAsync(updates);
    }

    public async Task AddMultipleDataToRankingAsync(RankingData[] datas)
    {
        if (database == null || user == null || datas == null) return;
        var updates = new Dictionary<string, object>();
        foreach (var data in datas)
        {
            string key = database.Child("ranking").Push().Key;
            updates[$"/ranking/{key}"] = JsonUtility.ToJson(data);
            updates[$"/user-scores/{user.UserId}/{key}"] = JsonUtility.ToJson(data);
        }
        await database.UpdateChildrenAsync(updates);
    }

    public Task<DataSnapshot> GetDataOfRankingFromDatabaseAsync() => database?.Child("ranking").GetValueAsync();
    public Task<DataSnapshot> GetDataOfUserScoreFromDatabaseAsync() => database?.Child("user-scores")?.Child(user.UserId).GetValueAsync();
    public Task<DataSnapshot> GetDataOfEquipmentFromDatabaseAsync() => database?.Child($"users/{user.UserId}/equipment").GetValueAsync();
    public Task<DataSnapshot> GetDataOfShoppingCarFromDatabaseAsync() => database?.Child($"users/{user.UserId}/shopping-car").GetValueAsync();
}
