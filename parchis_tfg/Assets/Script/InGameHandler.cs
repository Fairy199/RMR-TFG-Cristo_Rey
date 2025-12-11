using UnityEngine;
using Photon.Pun;

public class InGameHandler : MonoBehaviour
{
    public Transform spawnPosition;
    public void Start()
    {
        PhotonNetwork.Instantiate("Jugador1", spawnPosition.position, Quaternion.identity);
    }
    
}
