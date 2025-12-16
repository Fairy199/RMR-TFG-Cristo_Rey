using UnityEngine;
using TMPro;

public class LeaderBoardEntry : MonoBehaviour
{
    public TextMeshProUGUI puestoText;
    public TextMeshProUGUI usernameText;
    public TextMeshProUGUI scoreText;

    private void Awake()
    {
        puestoText   = transform.Find("Text (TMP)")?.GetComponent<TextMeshProUGUI>();     // puesto
        usernameText = transform.Find("Text (TMP) (1)")?.GetComponent<TextMeshProUGUI>(); // username
        scoreText    = transform.Find("Text (TMP) (2)")?.GetComponent<TextMeshProUGUI>(); // score

        if (puestoText == null || usernameText == null || scoreText == null)
            Debug.LogError("No se pudieron encontrar los TextMeshProUGUI en el prefab");
    }

    public void SetData(int puesto, string username, int score)
    {
        puestoText.text = puesto.ToString();
        usernameText.text = username;
        scoreText.text = score.ToString();
    }
}