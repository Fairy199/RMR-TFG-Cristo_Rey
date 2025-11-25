using UnityEngine;

[System.Serializable]
public class CasillaEspecial
{
    public int casillaOrigen;   
    public int casillaDestino;  
    public bool repetirTurno;   
}

public class FollowThePath : MonoBehaviour
{
    public Transform[] PuntoDeCamino;
    public float moveSpeed = 2f;

    public int PuntoDeCaminoIndex = 0;
    public bool puedesMoverte = false;

    private int pasosRestantes = 0;

    public CasillaEspecial[] casillasEspeciales;

    public void Mover(int pasos)
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
        if (puedesMoverte && PuntoDeCaminoIndex < PuntoDeCamino.Length)
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
                    GameControl.RepetirTurno();
                }
            }
        }
    }
}
