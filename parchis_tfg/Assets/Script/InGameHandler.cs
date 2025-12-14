using UnityEngine;
using Photon.Pun;

public class InGameHandler : MonoBehaviour
{
    public Transform[] spawnPositions;
    public Transform[] pathPoints;
    public string playerPrefabName = "Jugador1";

    void Start()
    {
        int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        index = Mathf.Clamp(index, 0, spawnPositions.Length - 1);

        GameObject jugador = PhotonNetwork.Instantiate(playerPrefabName, spawnPositions[index].position, Quaternion.identity);
        jugador.tag = "Player";

        var ftp = jugador.GetComponent<FollowThePathMultiplayer>();
        if (ftp != null)
            ftp.PuntoDeCamino = pathPoints;

        Debug.Log($"Jugador instanciado: {jugador.name}, ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");
    }
}
