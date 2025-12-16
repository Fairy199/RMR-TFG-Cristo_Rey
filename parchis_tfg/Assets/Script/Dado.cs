using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections;

public class Dado : MonoBehaviour
{
    private Sprite[] carasDado;
    private SpriteRenderer rend;
    private bool coroutineAllowed = true;

    private void Start()
    {
        rend = GetComponent<SpriteRenderer>();
        carasDado = Resources.LoadAll<Sprite>("Dados");

        if (carasDado.Length == 0)
        {
            Debug.LogError("No se encontraron sprites del dado en Resources/Dados");
        }
        else
        {
            rend.sprite = carasDado[5];
        }
    }

    private void Update()
    {
        if (!GameControl.gameOver)
        {
            if (GameControl.puedeTirar)
            {
                if (coroutineAllowed)
                {
                    if (GameControl.turno == 1)
                    {
                        if (Keyboard.current.spaceKey.wasPressedThisFrame)
                        {
                            StartCoroutine(TirarElDado());
                        }
                    }
                    else if (GameControl.turno == 2)
                    {
                        StartCoroutine(TirarElDado());
                    }
                }
            }
        }
    }

    private void OnMouseDown()
    {
        if (!GameControl.gameOver)
        {
            if (GameControl.puedeTirar)
            {
                if (coroutineAllowed)
                {
                    if (GameControl.turno == 1)
                    {
                        StartCoroutine(TirarElDado());
                    }
                }
            }
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
