using UnityEngine;

public class MenuLeaderboard : MonoBehaviour
{
    [Header("Leaderboard")]
    public GameObject leaderboardCanvas; // Arrastra aquí el Canvas del leaderboard

    private void Start()
    {
        if (leaderboardCanvas == null)
        {
            Debug.LogWarning("Leaderboard Canvas no asignado en el Inspector.");
        }
    }

    // Función que se asigna al botón de Leaderboard
    public void MostrarLeaderboard()
    {
        if (leaderboardCanvas != null)
        {
            // Poner el Canvas al frente
            leaderboardCanvas.transform.SetAsLastSibling();
        }
    }

    // Función opcional para cerrar el leaderboard
    public void CerrarLeaderboard()
    {
        if (leaderboardCanvas != null)
        {
            // Opcional: volverlo atrás si quieres
            leaderboardCanvas.transform.SetAsFirstSibling();
        }
    }
}
