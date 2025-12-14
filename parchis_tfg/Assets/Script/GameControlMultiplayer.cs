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

    // -------------------------------
    // TURNO GLOBAL SINCRONIZADO
    // -------------------------------
    public static int Turno
    {
        get
        {
            if (PhotonNetwork.CurrentRoom.CustomProperties.TryGetValue("turno", out object value))
                return (int)value;
            return 1;
        }
        set
        {
            ExitGames.Client.Photon.Hashtable h = new ExitGames.Client.Photon.Hashtable();
            h["turno"] = value;
            PhotonNetwork.CurrentRoom.SetCustomProperties(h);
        }
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
            Turno = 1; // SOLO EL MASTER MARCA EL TURNO INICIAL
        }

        RegisterLocalPlayer();
        ActualizarTurnoUI();
    }

    private void RegisterLocalPlayer()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            var ftp = go.GetComponent<FollowThePathMultiplayer>();
            if (ftp != null && ftp.photonView.IsMine)
                RegisterPlayer(ftp);
        }
    }

    private void RegisterPlayer(FollowThePathMultiplayer ftp)
    {
        if (!players.Contains(ftp))
        {
            players.Add(ftp);
            Debug.Log($">> Jugador registrado: {ftp.name}");
        }
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(WaitAndRegister(newPlayer.ActorNumber));
    }

    private IEnumerator WaitAndRegister(int actorNumber)
    {
        PhotonView pv = null;
        while (pv == null)
        {
            foreach (var view in FindObjectsOfType<PhotonView>())
            {
                if (view.Owner != null && view.Owner.ActorNumber == actorNumber)
                {
                    pv = view;
                    break;
                }
            }
            yield return null;
        }

        var ftp = pv.GetComponent<FollowThePathMultiplayer>();
        if (ftp != null) RegisterPlayer(ftp);
    }

    void Update()
    {
        if (gameOver || players.Count == 0) return;

        foreach (var player in players)
        {
            if (player.PuntoDeCaminoIndex >= player.PuntoDeCamino.Length)
            {
                quienGanaTexto.gameObject.SetActive(true);
                int jugadorNum = players.IndexOf(player) + 1;
                quienGanaTexto.text = $"Jugador {jugadorNum} gana";
                gameOver = true;
            }
        }
    }

    // ------------------------------------
    //       MÉTODOS DE TURNOS
    // ------------------------------------

    public static void AvanzarTurno()
    {
        int nuevo = Turno + 1;
        if (nuevo > players.Count) nuevo = 1;
        Turno = nuevo;
    }

    public static void RepetirTurno()
    {
        int nuevo = Turno - 1;
        if (nuevo < 1) nuevo = players.Count;
        Turno = nuevo;
    }

    private static void ActualizarTurnoUI()
    {
        if (jugadorMueveTexto != null)
            jugadorMueveTexto.text = $"Turno jugador {Turno}";
    }

    // 👇 AQUÍ ESTABA EL ERROR (ya corregido)
    public override void OnRoomPropertiesUpdate(ExitGames.Client.Photon.Hashtable changed)
    {
        if (changed.ContainsKey("turno"))
            ActualizarTurnoUI();
    }

    // ------------------------------------
    //      MÉTODOS DE TEST Y MOVIMIENTO
    // ------------------------------------

    public static void TestTurn()
    {
        FollowThePathMultiplayer jugadorActual = players[Turno - 1];

        if (!jugadorActual.photonView.IsMine)
        {
            Debug.Log(">> No es tu turno.");
            return;
        }

        jugadorActual.Mover(diceSideThrown);
        AvanzarTurno();
    }

    public static void TestMovimiento()
    {
        FollowThePathMultiplayer jugadorActual = players[Turno - 1];

        if (!jugadorActual.photonView.IsMine)
        {
            Debug.Log(">> No es tu turno.");
            return;
        }

        jugadorActual.Mover(3);
    }

    public static bool TurnoJugadorSoyYo()
    {
        if (players.Count == 0) return false;

        return players[Turno - 1].photonView.IsMine;
    }

}
