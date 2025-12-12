using UnityEngine;
using Photon.Pun;

public class MovimientoFichaMultiplayer : MonoBehaviourPun
{
    [SerializeField] private float velocidadMovimiento = 2f;
    public Transform casillasPadre;

    private Transform[] casillas;
    private int indiceCasillaActual = 0;
    private int casillasRestantes = 0;
    private bool moviendo = false;

    private void Start()
    {
        if (casillasPadre != null)
        {
            int cantidad = casillasPadre.childCount;
            casillas = new Transform[cantidad];
            for (int i = 0; i < cantidad; i++)
                casillas[i] = casillasPadre.GetChild(i);
        }

        if (casillas.Length > 0)
            transform.position = casillas[indiceCasillaActual].position;
    }

    private void Update()
    {
        if (!photonView.IsMine) return; // Solo mueve el propietario

        if (moviendo) MoverAutomatico();
        if (Input.GetKeyDown(KeyCode.Space)) Mover(3);
    }

    public void Mover(int numeroCasillas)
    {
        if (!moviendo && casillas.Length > 0)
        {
            casillasRestantes = numeroCasillas;
            moviendo = true;
            photonView.RPC("RPC_SetCasillasRestantes", RpcTarget.OthersBuffered, numeroCasillas);
        }
    }

    [PunRPC]
    void RPC_SetCasillasRestantes(int numero)
    {
        casillasRestantes = numero;
        moviendo = true;
    }

    private void MoverAutomatico()
    {
        if (casillasRestantes > 0 && indiceCasillaActual < casillas.Length - 1)
        {
            Vector3 destino = casillas[indiceCasillaActual + 1].position;
            transform.position = Vector3.MoveTowards(transform.position, destino, velocidadMovimiento * Time.deltaTime);

            if (Vector3.Distance(transform.position, destino) < 0.2f)
            {
                indiceCasillaActual++;
                casillasRestantes--;
                if (casillasRestantes == 0) moviendo = false;
            }
        }
        else moviendo = false;
    }
}
