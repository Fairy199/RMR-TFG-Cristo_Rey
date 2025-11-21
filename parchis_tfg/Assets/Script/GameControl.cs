using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static bool gameOver = false;

    private static FollowThePath jugador1Path;
    private static FollowThePath jugador2Path;

    private static GameObject quienGanaTexto;
    private static GameObject jugador1MueveTexto;
    private static GameObject jugador2MueveTexto;

    private static int turno = 1;

    void Start()
    {
        quienGanaTexto = GameObject.Find("quienGanaTexto");
        jugador1MueveTexto = GameObject.Find("jugador1MueveTexto");
        jugador2MueveTexto = GameObject.Find("jugador2MueveTexto");

        jugador1Path = GameObject.Find("Jugador1").GetComponent<FollowThePath>();
        jugador2Path = GameObject.Find("Jugador2").GetComponent<FollowThePath>();

        quienGanaTexto.SetActive(false);
        jugador1MueveTexto.SetActive(true);
        jugador2MueveTexto.SetActive(false);
    }

    public static void MoverJugador(int jugador)
    {
        if (jugador == 1)
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
}
