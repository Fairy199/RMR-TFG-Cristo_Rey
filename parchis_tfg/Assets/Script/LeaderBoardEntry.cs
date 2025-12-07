using UnityEngine;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        // Buscar automáticamente los hijos correctos según tu descripción
        usernameText = transform.Find("Text (TMP) (1)")?.GetComponent<TextMeshProUGUI>();
        scoreText = transform.Find("Text (TMP) (2)")?.GetComponent<TextMeshProUGUI>();

        if (usernameText == null || scoreText == null)
            Debug.LogError("No se pudieron encontrar los TextMeshProUGUI en el prefab");
    }
}
