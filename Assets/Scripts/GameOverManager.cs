using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text scoreText;  // Referencia al TextMeshPro donde se mostrará la puntuación final

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Assegura't que el TextMeshPro està assignat a l'Inspector!");
            return;
        }

        // Recuperar la puntuació del jugador i mostrar-la
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);  // Obtiene la puntuación (por defecto 0 si no existe)
        scoreText.text = "Puntuació Final: " + finalScore.ToString();
    }
}
