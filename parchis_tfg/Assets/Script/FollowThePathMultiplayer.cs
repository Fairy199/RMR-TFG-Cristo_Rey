using UnityEngine;
using Photon.Pun;

public class FollowThePathMultiplayer : MonoBehaviourPun
{
    public Transform[] PuntoDeCamino;
    public float moveSpeed = 2f;

    [HideInInspector] public int PuntoDeCaminoIndex = 0;

    private int pasosRestantes = 0;
    private bool puedesMoverte = false;

    public CasillaEspecial[] casillasEspeciales;

    public void Mover(int pasos)
    {
        if (photonView.IsMine)
        {
            pasosRestantes = pasos;
            puedesMoverte = true;
            photonView.RPC("RPC_SetPasosRestantes", RpcTarget.OthersBuffered, pasos);
        }
    }

    [PunRPC]
    void RPC_SetPasosRestantes(int pasos)
    {
        pasosRestantes = pasos;
        puedesMoverte = true;
    }

    private void Start()
    {
        if (PuntoDeCamino.Length > 0)
            transform.position = PuntoDeCamino[0].position;
    }

    private void Update()
    {
        if (!puedesMoverte) return;

        if (PuntoDeCaminoIndex < PuntoDeCamino.Length)
        {
            Vector3 destino = PuntoDeCamino[PuntoDeCaminoIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, destino, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destino) < 0.05f)
            {
                PuntoDeCaminoIndex++;
                pasosRestantes--;

                if (pasosRestantes <= 0)
                {
                    puedesMoverte = false;
                    RevisarCasillaEspecial();

                    // Si NO repite turno, pasar turno
                    if (photonView.IsMine)
                        GameControlMultiplayer.AvanzarTurno();
                }
            }
        }
    }

    private void RevisarCasillaEspecial()
    {
        int casillaActual = PuntoDeCaminoIndex - 1;

        foreach (var casilla in casillasEspeciales)
        {
            if (casillaActual == casilla.casillaOrigen)
            {
                PuntoDeCaminoIndex = casilla.casillaDestino;
                transform.position = PuntoDeCamino[PuntoDeCaminoIndex].position;

                if (casilla.repetirTurno)
                {
                    if (photonView.IsMine)
                        GameControlMultiplayer.RepetirTurno();
                }
            }
        }
    }
}
