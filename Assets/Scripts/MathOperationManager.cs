using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathOperationManager : MonoBehaviour
{
    public TMP_Text operationText;  // Referència al TextMeshPro UI on es mostrarà l'operació
    public TMP_InputField answerInputField;  // Referència al InputField on el jugador escriurà la seva resposta
    public Button submitButton;  // Referència al botó per enviar la resposta

    private int correctAnswer;  // La resposta correcta de l'operació
    private GameObject currentCube;  // Referència al cub que ha col·lisionat
    private int score = 0;  // Puntuació del jugador
    private int pointsForCurrentOperation = 0;  // Punts per l'operació actual

    void Start()
    {
        if (operationText == null || answerInputField == null || submitButton == null)
        {
            Debug.LogError("Assegura't que tots els components estan assignats a l'Inspector!");
            return;
        }

        // Amagar l'operació i el camp d'entrada al començar
        operationText.text = "";
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

        // Afegir listener al botó d'enviar resposta
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void Update()
    {
        // Comprovar si s'ha premut Enter (només si el camp d'entrada està actiu)
        if (answerInputField.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();  // Cridar la funció per verificar la resposta quan es prem Enter
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D triggered with: " + collision.gameObject.name);
        // Comprovar si la col·lisió és amb un cub
        if (collision.gameObject.CompareTag("Cube"))
        {
            Debug.Log("Col·lisió amb un cub detectada.");
            currentCube = collision.gameObject;  // Guardar referència al cub que ha col·lisionat
            GenerateAndShowOperation();
        }
    }

    public void GenerateAndShowOperation()
    {
        // Aturar el temps del joc
        Time.timeScale = 0;

        // Generar dos nombres aleatoris
        int number1 = Random.Range(1, 100);
        int number2 = Random.Range(1, 10);

        // Generar un nombre aleatori per seleccionar l'operació
        int operationType = Random.Range(0, 4);

        string operation = "";

        switch (operationType)
        {
            case 0:
                operation = number1 + " + " + number2;
                correctAnswer = number1 + number2;
                pointsForCurrentOperation = 1;  // Punts per suma
                break;
            case 1:
                // Assegurar-se que el resultat no és negatiu
                if (number1 < number2)
                {
                    int temp = number1;
                    number1 = number2;
                    number2 = temp;
                }
                operation = number1 + " - " + number2;
                correctAnswer = number1 - number2;
                pointsForCurrentOperation = 2;  // Punts per resta
                break;
            case 2:
                operation = number1 + " * " + number2;
                correctAnswer = number1 * number2;
                pointsForCurrentOperation = 5;  // Punts per multiplicació
                break;
            case 3:
                // Assegurar-se que la divisió no té residu
                while (number2 == 0 || number1 % number2 != 0)
                {
                    number1 = Random.Range(1, 100);
                    number2 = Random.Range(1, 10);
                }
                operation = number1 + " / " + number2;
                correctAnswer = number1 / number2;
                pointsForCurrentOperation = 10;  // Punts per divisió
                break;
        }

        // Mostrar l'operació al TextMeshPro UI
        Debug.Log("Operació generada: " + operation + " = " + correctAnswer);
        operationText.text = operation + " =";
        answerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);

        // Seleccionar automàticament l'InputField perquè el jugador comenci a escriure
        answerInputField.Select();
        answerInputField.ActivateInputField();
    }

    public void CheckAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                Debug.Log("Resposta correcta!");
                score += pointsForCurrentOperation;  // Incrementar la puntuació segons el tipus d'operació

                // Destruir el cub després de resoldre correctament l'operació
                Destroy(currentCube);

                // Reiniciar el camp d'entrada i amagar-lo
                answerInputField.text = "";
                answerInputField.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);

                // Reprendre el temps del joc
                Time.timeScale = 1;

                // Amagar l'operació
                operationText.text = "";
            }
            else
            {
                Debug.Log("Resposta incorrecta! Torna a intentar-ho.");
                // Aquí es canvia a la escena de Game Over
                PlayerPrefs.SetInt("FinalScore", score);  // Guarda la puntuació final
                SceneManager.LoadScene("GameOver");  // Asegúrate de que la escena "Game Over" esté en los Build Settings
            }
        }
        else
        {
            Debug.Log("Introdueix un nombre vàlid.");
            answerInputField.text = "";
            answerInputField.Select();
            answerInputField.ActivateInputField();
        }
    }
}
