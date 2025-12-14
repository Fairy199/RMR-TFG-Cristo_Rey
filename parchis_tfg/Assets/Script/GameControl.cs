using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static int diceSideThrown = 0;
    public static bool gameOver = false;
    public static bool puedeTirar = true;

    private static FollowThePath jugador1Path;
    private static FollowThePath jugador2Path;

    private static GameObject quienGanaTexto;
    private static GameObject jugador1MueveTexto;
    private static GameObject jugador2MueveTexto;

    public static int turno = 1;

    // Nombres de jugadores
    public static string jugador1Nombre = "Jugador 1";
    public static string jugador2Nombre = "Jugador 2";

    // Singleton para acceder desde FirebaseControlador
    public static GameControl instance;

    void Awake()
    {
        instance = this;
    }

    void Start()
    {
        quienGanaTexto = GameObject.Find("quienGanaTexto");
        jugador1MueveTexto = GameObject.Find("jugador1MueveTexto");
        jugador2MueveTexto = GameObject.Find("jugador2MueveTexto");

        jugador1Path = GameObject.Find("Jugador1").GetComponent<FollowThePath>();
        jugador2Path = GameObject.Find("Jugador2").GetComponent<FollowThePath>();

        quienGanaTexto.SetActive(false);

        // Actualiza los textos con los nombres actuales (por defecto o asignados desde Firebase)
        ActualizarTextoTurno();
    }

    // Método para actualizar nombre de jugador 1 dinámicamente
    public void ActualizarNombreJugador1(string nombre)
    {
        jugador1Nombre = nombre;
        ActualizarTextoTurno();
    }

    private void ActualizarTextoTurno()
    {
        if (jugador1MueveTexto != null)
            jugador1MueveTexto.GetComponent<TMP_Text>().text = jugador1Nombre + " mueve";
        if (jugador2MueveTexto != null)
            jugador2MueveTexto.GetComponent<TMP_Text>().text = jugador2Nombre + " mueve";
    }

    public static void JugarTurno()
    {
        if (gameOver || !puedeTirar) return;

        puedeTirar = false;

        if (turno == 1)
        {
            jugador1Path.Mover(diceSideThrown);
            jugador1MueveTexto.SetActive(false);
            jugador2MueveTexto.SetActive(true);
            turno = 2;
        }
        else
        {
            jugador2Path.Mover(diceSideThrown);
            jugador2MueveTexto.SetActive(false);
            jugador1MueveTexto.SetActive(true);
            turno = 1;
        }
    }

    public static void AvisarMovimientoTerminado()
    {
        puedeTirar = true;
    }

    public static void RepetirTurno()
    {
        if (turno == 1)
        {
            turno = 2;
            jugador2MueveTexto.SetActive(false);
            jugador1MueveTexto.SetActive(true);
        }
        else
        {
            turno = 1;
            jugador1MueveTexto.SetActive(false);
            jugador2MueveTexto.SetActive(true);
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            if (jugador1Path.PuntoDeCaminoIndex >= jugador1Path.PuntoDeCamino.Length)
            {
                quienGanaTexto.SetActive(true);
                quienGanaTexto.GetComponent<TMP_Text>().text = jugador1Nombre + " gana";
                gameOver = true;
            }
            else if (jugador2Path.PuntoDeCaminoIndex >= jugador2Path.PuntoDeCamino.Length)
            {
                quienGanaTexto.SetActive(true);
                quienGanaTexto.GetComponent<TMP_Text>().text = jugador2Nombre + " gana";
                gameOver = true;
            }
        }
    }
}
