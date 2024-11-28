using UnityEngine;
using TMPro;

public class GameOverManager : MonoBehaviour
{
    public TMP_Text scoreText;  // Referencia al TextMeshPro donde se mostrar� la puntuaci�n final

    void Start()
    {
        if (scoreText == null)
        {
            Debug.LogError("Assegura't que el TextMeshPro est� assignat a l'Inspector!");
            return;
        }

        // Recuperar la puntuaci� del jugador i mostrar-la
        int finalScore = PlayerPrefs.GetInt("FinalScore", 0);  // Obtiene la puntuaci�n (por defecto 0 si no existe)
        scoreText.text = "Puntuaci� Final: " + finalScore.ToString();
    }
}
