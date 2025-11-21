using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] PuntoDeCamino;
    public float moveSpeed = 2f;

    public int PuntoDeCaminoIndex = 0;
    public bool puedesMoverte = false;

    private int pasosRestantes = 0;

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
                    puedesMoverte = false;
            }
        }
    }
}
