using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using ExitGames.Client.Photon;

public class GameControlMultiplayer : MonoBehaviourPunCallbacks
{
    public static int diceSideThrown = 0;
    public static bool gameOver = false;

    private static List<FollowThePathMultiplayer> players = new List<FollowThePathMultiplayer>();
    private static TMP_Text quienGanaTexto;
    private static TMP_Text jugadorMueveTexto;


    public static int GetTurno()
    {
        if (PhotonNetwork.CurrentRoom.CustomProperties.ContainsKey("turno"))
        {
            return (int)PhotonNetwork.CurrentRoom.CustomProperties["turno"];
        }
        return 0;
    }

    public static void SetTurno(int nuevoTurno)
    {
        ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
        h["turno"] = nuevoTurno;
        PhotonNetwork.CurrentRoom.SetCustomProperties(h);
    }


    void Start()
    {
        quienGanaTexto = GameObject.Find("quienGanaTexto").GetComponent<TMP_Text>();
        jugadorMueveTexto = GameObject.Find("jugador1MueveTexto").GetComponent<TMP_Text>();

        quienGanaTexto.gameObject.SetActive(false);
        jugadorMueveTexto.gameObject.SetActive(false);

        gameOver = false;

        if (PhotonNetwork.IsMasterClient)
        {
            SetTurno(1);
        }

        RegisterLocalPlayer();
        ActualizarTurnoUI();
    }

    private void RegisterLocalPlayer()
    {
        GameObject[] objetos = GameObject.FindGameObjectsWithTag("Player");

        for (int i = 0; i < objetos.Length; i++)
        {
            FollowThePathMultiplayer ftp = objetos[i].GetComponent<FollowThePathMultiplayer>();

            if (ftp != null && ftp.photonView.IsMine)
            {
                RegisterPlayer(ftp);
            }
        }
    }

    private void RegisterPlayer(FollowThePathMultiplayer ftp)
    {
        if (!players.Contains(ftp))
        {
            players.Add(ftp);
            Debug.Log(">> Jugador registrado: " + ftp.name);
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(WaitAndRegister(newPlayer.ActorNumber));
    }

    private IEnumerator WaitAndRegister(int actorNumber)
    {
        PhotonView encontrado = null;

        while (encontrado == null)
        {
            PhotonView[] vistas = FindObjectsOfType<PhotonView>();

            for (int i = 0; i < vistas.Length; i++)
            {
                if (vistas[i].Owner != null &&
                    vistas[i].Owner.ActorNumber == actorNumber)
                {
                    encontrado = vistas[i];
                }
            }

            yield return null;
        }

        FollowThePathMultiplayer ftp = encontrado.GetComponent<FollowThePathMultiplayer>();

        if (ftp != null)
        {
            RegisterPlayer(ftp);
        }
    }

    void Update()
    {
        if (!gameOver && players.Count > 0)
        {
            for (int i = 0; i < players.Count; i++)
            {
                if (players[i].PuntoDeCaminoIndex >= players[i].PuntoDeCamino.Length)
                {
                    quienGanaTexto.gameObject.SetActive(true);
                    quienGanaTexto.text = "Jugador " + (i + 1) + " gana";
                    gameOver = true;
                }
            }
        }
    }

    public static void AvanzarTurno()
    {
        int turnoActual = GetTurno();
        int nuevoTurno = turnoActual + 1;

        if (nuevoTurno > players.Count)
        {
            nuevoTurno = 1;
        }

        SetTurno(nuevoTurno);
    }

    public static void RepetirTurno()
    {
        int turnoActual = GetTurno();
        int nuevoTurno = turnoActual - 1;

        if (nuevoTurno < 1)
        {
            nuevoTurno = players.Count;
        }

        SetTurno(nuevoTurno);
    }

    private static void ActualizarTurnoUI()
    {
        if (jugadorMueveTexto != null)
        {
            jugadorMueveTexto.text = "Turno jugador " + GetTurno();
        }
    }

    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changed)
    {
        if (changed.ContainsKey("turno"))
        {
            ActualizarTurnoUI();
        }
    }


    public static void TestTurn()
    {
        int turnoActual = GetTurno();
        FollowThePathMultiplayer jugadorActual = players[turnoActual - 1];

        if (jugadorActual.photonView.IsMine)
        {
            jugadorActual.Mover(diceSideThrown);
            AvanzarTurno();
        }
        else
        {
            Debug.Log(">> No es tu turno.");
        }
    }

    public static void TestMovimiento()
    {
        int turnoActual = GetTurno();
        FollowThePathMultiplayer jugadorActual = players[turnoActual - 1];

        if (jugadorActual.photonView.IsMine)
        {
            jugadorActual.Mover(3);
        }
        else
        {
            Debug.Log(">> No es tu turno.");
        }
    }

    public static bool TurnoJugadorSoyYo()
    {
        if (players.Count == 0)
            return false;

        int turnoActual = GetTurno();
        return players[turnoActual - 1].photonView.IsMine;
    }
}
