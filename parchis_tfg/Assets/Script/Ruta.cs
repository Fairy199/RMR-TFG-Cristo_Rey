using UnityEngine;
using System.Collections.Generic;

public class Ruta : MonoBehaviour
{
    public Transform[] PuntoDeCamino;

    [SerializeField]
    private float moveSpeed = 1f;

    [HideInInspector]
    public int PuntoDeCaminoIndex = 0;

    public bool puedesMoverte = false;

    private void Start()
    {
        transform.position = PuntoDeCamino[PuntoDeCaminoIndex].position;
    }

    private void Update()
    {
        if (puedesMoverte)
            Mover();
    }

    private void Mover()
    {
        if (PuntoDeCaminoIndex < PuntoDeCamino.Length)
        {
            Vector3 destino = PuntoDeCamino[PuntoDeCaminoIndex].position;

            // Mover
            transform.position = Vector3.MoveTowards(
                transform.position,
                destino,
                moveSpeed * Time.deltaTime
            );

            // Comprobamos si hemos llegado
            if (Vector3.Distance(transform.position, destino) < 0.05f)
            {
                PuntoDeCaminoIndex++;

                // Si llegó al final, se detiene
                if (PuntoDeCaminoIndex >= PuntoDeCamino.Length)
                    puedesMoverte = false;
            }
        }
    }
}
