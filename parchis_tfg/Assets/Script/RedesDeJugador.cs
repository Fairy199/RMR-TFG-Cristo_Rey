using UnityEngine;
using Photon.Pun;
using System.Collections; 
using System.Collections.Generic;
public class RedesDeJugador : MonoBehaviour
{
    public MonoBehaviour[] codigosQueIgnorar;

    private PhotonView photonView;

    void Start()
    {
        photonView = GetComponent<PhotonView>();
        if (!photonView.IsMine)
        {
            foreach (var codigo in codigosQueIgnorar)
            {
                codigo.enabled = false;
            }
        }
    }

    void Update()
    {
        
    }
}
