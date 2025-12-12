using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GameControlMultiplayer : MonoBehaviourPunCallbacks
{
    public static int diceSideThrown = 0;
    public static bool gameOver = false;

    private static List<FollowThePathMultiplayer> players = new List<FollowThePathMultiplayer>();
    private static TMP_Text quienGanaTexto;
    private static TMP_Text jugadorMueveTexto;

    private static int turno = 1;

    void Start()
    {
        // Referencias UI
        quienGanaTexto = GameObject.Find("quienGanaTexto").GetComponent<TMP_Text>();
        jugadorMueveTexto = GameObject.Find("jugador1MueveTexto").GetComponent<TMP_Text>();
        quienGanaTexto.gameObject.SetActive(false);
        jugadorMueveTexto.gameObject.SetActive(false);

        gameOver = false;
        turno = 1;

        // Registrar el jugador local
        RegisterLocalPlayer();
        ActualizarTurnoUI();
    }

    private void RegisterLocalPlayer()
    {
        foreach (var go in GameObject.FindGameObjectsWithTag("Player"))
        {
            var ftp = go.GetComponent<FollowThePathMultiplayer>();
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
            Debug.Log($">> Jugador registrado: {ftp.name}, ActorNumber: {ftp.photonView.Owner.ActorNumber}");
        }
    }

    // Cuando entra un jugador nuevo a la sala
    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        StartCoroutine(WaitAndRegister(newPlayer.ActorNumber));
    }

    private System.Collections.IEnumerator WaitAndRegister(int actorNumber)
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
        if (ftp != null)
            RegisterPlayer(ftp);
    }

    void Update()
    {
        if (gameOver || players.Count == 0) return;

        // Revisar victoria
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

    // -----------------------------
    // MÉTODOS DE TURNOS
    // -----------------------------
    public static void JugarTurno()
    {
        if (gameOver || players.Count == 0) return;

        FollowThePathMultiplayer jugadorActual = players[turno - 1];
        if (jugadorActual.photonView.IsMine)
        {
            jugadorActual.Mover(diceSideThrown);
        }

        turno++;
        if (turno > players.Count) turno = 1;

        ActualizarTurnoUI();
    }

    public static void RepetirTurno()
    {
        if (players.Count == 0) return;

        turno--;
        if (turno < 1) turno = players.Count;

        ActualizarTurnoUI();
    }

    private static void ActualizarTurnoUI()
    {
        if (jugadorMueveTexto != null)
            jugadorMueveTexto.text = $"Turno jugador {turno}";
    }

    // -----------------------------
    // MÉTODOS DE TEST
    // -----------------------------
    public static void TestTurn()
    {
        Debug.Log($">> Turno testeado por: {PhotonNetwork.LocalPlayer.NickName}");
        JugarTurno();
    }

    public static void TestMovimiento()
    {
        if (players.Count == 0 || gameOver)
        {
            Debug.Log("Mov test cancelado: juego no iniciado.");
            return;
        }

        FollowThePathMultiplayer jugadorActual = players[turno - 1];

        if (jugadorActual.photonView.IsMine)
        {
            Debug.Log($">> TEST MOVIMIENTO ejecutado por {PhotonNetwork.LocalPlayer.NickName}");
            jugadorActual.Mover(3);
        }
        else
        {
            Debug.Log(">> NO ES TU TURNO, no puedes mover.");
        }
    }
}
