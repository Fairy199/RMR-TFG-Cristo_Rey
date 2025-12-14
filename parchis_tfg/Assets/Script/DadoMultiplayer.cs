using System.Collections;
using UnityEngine;
using Photon.Pun;
using UnityEngine.InputSystem;

public class DadoMultiplayer : MonoBehaviour
{
    private Sprite[] carasDado;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        carasDado = Resources.LoadAll<Sprite>("Dados");

        if (carasDado.Length == 0)
            Debug.LogError("No se encontraron sprites en Resources/Dados");

        rend.sprite = carasDado[5]; // cara inicial
    }

    private void Update()
    {
        // Solo permitir tirar si es mi turno, no hay movimiento en curso y el juego no terminó
        if (!coroutineAllowed || GameControlMultiplayer.gameOver)
            return;

        if (GameControlMultiplayer.TurnoJugadorSoyYo())
        {
            if (Keyboard.current.spaceKey.wasPressedThisFrame)
                StartCoroutine(TirarDado());
        }
    }

    private IEnumerator TirarDado()
    {
        coroutineAllowed = false;

        int numero = 0;

        // Animación del dado
        for (int i = 0; i < 20; i++)
        {
            numero = Random.Range(0, 6);
            if (carasDado != null && carasDado.Length > 0)
                rend.sprite = carasDado[numero];
            yield return new WaitForSeconds(0.05f);
        }

        int resultado = numero + 1;

        // Guardamos resultado global
        GameControlMultiplayer.diceSideThrown = resultado;
        Debug.Log("DADO → " + resultado);

        // Mover al jugador correspondiente
        GameControlMultiplayer.TestTurn();

        coroutineAllowed = true;
    }
}
