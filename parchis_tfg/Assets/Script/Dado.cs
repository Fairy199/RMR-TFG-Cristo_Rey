using UnityEngine;
using UnityEngine.InputSystem; // Necesario para el nuevo Input System
using System.Collections;

public class Dado : MonoBehaviour
{
    private Sprite[] carasDado;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    private void Start()
    {
        Debug.Log("INICIÓ EL SCRIPT DADO");
        rend = GetComponent<SpriteRenderer>();

        // Carga las imágenes del dado desde Resources/Dados
        carasDado = Resources.LoadAll<Sprite>("Dados");

        if (carasDado.Length == 0)
            Debug.LogError("No se encontraron sprites del dado en Resources/Dados");

        rend.sprite = carasDado[5];
    }

    private void Update()
    {
        // Tirar dado con barra espacio usando nuevo Input System
        if (!GameControl.gameOver && coroutineAllowed && Keyboard.current.spaceKey.wasPressedThisFrame)
        {
            Debug.Log("TECLA ESPACIO: Tirando dado");
            StartCoroutine(TirarElDado());
        }
    }

    private void OnMouseDown()
    {
        if (!GameControl.gameOver && coroutineAllowed)
        {
            Debug.Log("CLICK DETECTADO EN EL DADO");
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
        GameControl.JugarTurno();

        coroutineAllowed = true;
    }
}
