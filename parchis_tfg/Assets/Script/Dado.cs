using UnityEngine;
using System.Collections;

public class Dado : MonoBehaviour
{
    private Sprite[] carasDado;
    private SpriteRenderer rend;
    private int turnoJugador = 1;
    private bool coroutineAllowed = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();

        // Carga las imágenes del dado desde Resources/Dados/
        carasDado = Resources.LoadAll<Sprite>("Dados");

        if (carasDado.Length == 0)
            Debug.LogError("No se encontraron sprites del dado en Resources/Dados");

        // Poner cara inicial
        rend.sprite = carasDado[5]; 
    }

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
        {
            StartCoroutine(TirarElDado());
        }
    }

    private IEnumerator TirarElDado()
    {
        coroutineAllowed = false;

        int numeroDadoRandom = 0;

        for (int i = 0; i <= 20; i++)
        {
            numeroDadoRandom = Random.Range(0, 6);
            rend.sprite = carasDado[numeroDadoRandom];
            yield return new WaitForSeconds(0.05f);
        }

        GameControl.diceSideThrown = numeroDadoRandom + 1;

        if (turnoJugador == 1)
            GameControl.MoverJugador(1);
        else
            GameControl.MoverJugador(2);

        turnoJugador *= -1;
        coroutineAllowed = true;
    }
}
