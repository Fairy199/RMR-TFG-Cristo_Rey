using UnityEngine;

public class FollowThePath : MonoBehaviour
{
    public Transform[] PuntoDeCamino;
    public float moveSpeed = 2f;
    public int PuntoDeCaminoIndex = 0;
    public bool puedesMoverte = false;

    private void Start()
    {
        if (PuntoDeCamino.Length > 0)
            transform.position = PuntoDeCamino[PuntoDeCaminoIndex].position;
        else
            Debug.LogError("No hay casillas asignadas en FollowThePath");
    }

    private void Update()
    {
        if (puedesMoverte && PuntoDeCaminoIndex < PuntoDeCamino.Length)
        {
            Vector3 destino = PuntoDeCamino[PuntoDeCaminoIndex].position;
            transform.position = Vector3.MoveTowards(transform.position, destino, moveSpeed * Time.deltaTime);

            if (Vector3.Distance(transform.position, destino) < 0.05f)
                PuntoDeCaminoIndex++;
        }
    }
}
