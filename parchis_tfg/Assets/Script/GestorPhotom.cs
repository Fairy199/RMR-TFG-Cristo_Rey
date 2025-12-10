using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class GestorPhotom : MonoBehaviourPunCallbacks
{
    public TMP_Text textIndicator;
    void Start()
    {
        if (!PhotonNetwork.IsConnected)
            PhotonNetwork.ConnectUsingSettings();
    }

    public void CreatePlayer(string namePlayer)
    {
        PhotonNetwork.NickName = namePlayer;
    }

    public override void OnConnected()
    {
        //PhotonNetwork.JoinLobby();
        base.OnConnected();
        Debug.Log("Conectado a photon");
        textIndicator.text = "Conectado correctamente";
    }

    public override void OnConnectedToMaster()
    {
        base.OnConnectedToMaster();
        textIndicator.text = "Bienvendio "+ PhotonNetwork.NickName;
    }

    /*public override void OnDisconnected(DisconectCause cause)
    {
        base.OnDisconnected(cause);
    }*/
/*
    public override void OnJoinedLobby()
    {
        //PhotonNetwork.JoinOrCreateRoom("Cuatro", new RoomOptions { MaxPlayers = 4 }, TypedLobby.Default);
    }

    public override void OnJoinedRoom()
    {
       // PhotonNetwork.Instantiate("JAmarillo", new Vector3(Random.Range(-1, 1), 2), Quaternion.identity);
    }*/
}
