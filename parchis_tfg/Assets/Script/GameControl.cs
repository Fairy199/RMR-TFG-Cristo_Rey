using UnityEngine;
using TMPro;

public class GameControl : MonoBehaviour
{
    public static int diceSideThrown = 0;
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

    public static void JugarTurno()
    {
        if (gameOver) return;

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
                quienGanaTexto.GetComponent<TMP_Text>().text = "Jugador 1 gana";
                gameOver = true;
            }

            else if (jugador2Path.PuntoDeCaminoIndex >= jugador2Path.PuntoDeCamino.Length)
            {
                quienGanaTexto.SetActive(true);
                quienGanaTexto.GetComponent<TMP_Text>().text = "Jugador 2 gana";
                gameOver = true;
            }
        }
    }
}
