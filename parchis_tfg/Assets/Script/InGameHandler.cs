using UnityEngine;
using Photon.Pun;

public class InGameHandler : MonoBehaviour
{
    public Transform[] spawnPositions;
    public Transform[] pathPoints; // ruta de camino
    public string playerPrefabName = "Jugador1";

    void Start()
    {
        int index = PhotonNetwork.LocalPlayer.ActorNumber - 1;
        index = Mathf.Clamp(index, 0, spawnPositions.Length - 1);

        // Instanciar jugador usando Photon
        GameObject jugador = PhotonNetwork.Instantiate(playerPrefabName, spawnPositions[index].position, Quaternion.identity);

        // Asignar tag Player y ruta de camino
        jugador.tag = "Player";

        var ftp = jugador.GetComponent<FollowThePathMultiplayer>();
        if (ftp != null)
        {
            ftp.PuntoDeCamino = pathPoints;
        }

        Debug.Log($"Jugador instanciado: {jugador.name}, ActorNumber: {PhotonNetwork.LocalPlayer.ActorNumber}");
    }
}
