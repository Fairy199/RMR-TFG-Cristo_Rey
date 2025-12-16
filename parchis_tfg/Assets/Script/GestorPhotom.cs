using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;
using System.Collections; 

public class GestorPhotom : MonoBehaviourPunCallbacks
{
    [Header("Instancias de scripts")]
    public WindowsHandler windowsHandler;

    [Header("Indicadores")]
    public TMP_Text textIndicator;
    public TMP_Text textNameSala;
    public Transform contentPlayers;

    [Header("Prefabs")]
    public GameObject nickNamePlayer;

    [Header("Botones Menu")]
    public GameObject btnConect;

    private int countPlayer = 0;
    void Start()
    {
        if (btnConect != null) btnConect.SetActive(false);
        PhotonNetwork.AutomaticallySyncScene = true;
    }

    public void CreatePlayer(string namePlayer)
    {
        PhotonNetwork.NickName = namePlayer;
    }

    public override void OnConnected()
    {
        base.OnConnected();
        Debug.Log("Conectado a Photon");
        if (textIndicator != null) textIndicator.text = "Conectado correctamente";
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        if (textIndicator != null) textIndicator.text = "Bienvenido " + PhotonNetwork.NickName;
        if (btnConect != null) btnConect.SetActive(true);
    }

    public void CreateRoom()
    {
        string nameRoom = "Sala";

        RoomOptions optionsRoom = new RoomOptions
        {
            IsVisible = true,
            MaxPlayers = 2,
            PublishUserId = true
        };

        PhotonNetwork.JoinOrCreateRoom(nameRoom, optionsRoom, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
        base.OnJoinedRoom();
        windowsHandler.EnableWindow(0);
        StartCoroutine(UpdateTextSala());
        Debug.Log("Estamos conectados a la sala " + PhotonNetwork.CurrentRoom.Name + " Bienvenido " + PhotonNetwork.NickName);
    }

    public void StartScene()
    {
        if (PhotonNetwork.IsMasterClient)
        {
            PhotonNetwork.LoadLevel("TableroOnline");
        }
    }

    IEnumerator UpdateTextSala()
    {
        while (true)
        {
            textNameSala.text = $"Nombre sala: {PhotonNetwork.CurrentRoom.Name} - #Jugadores: {PhotonNetwork.CurrentRoom.PlayerCount}";
            yield return new WaitForSeconds(0.2f);

            if (PhotonNetwork.CurrentRoom.Players.Count > countPlayer)
            {
                countPlayer = PhotonNetwork.CurrentRoom.Players.Count;
                foreach (var item in PhotonNetwork.CurrentRoom.Players)
                {
                    GameObject nickName = Instantiate(nickNamePlayer, contentPlayers);
                    nickName.GetComponent<TMP_Text>().text = item.Value.NickName;
                }
            }
        }
    }

    /*
    public override void OnDisconnected(DisconnectCause cause)
    {
        base.OnDisconnected(cause);
    }
    */
}
