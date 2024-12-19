using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Collections;

public class MathOperationManager : MonoBehaviour
{
    public TMP_Text operationText;  // Refer�ncia al TextMeshPro UI on es mostrar� l'operaci�
    public TMP_InputField answerInputField;  // Refer�ncia al InputField on el jugador escriur� la seva resposta
    public Button submitButton;  // Refer�ncia al bot� per enviar la resposta
    public TMP_Text timerText;  // Refer�ncia al TextMeshPro UI per mostrar el temps restant

    private int correctAnswer;  // La resposta correcta de l'operaci�
    private GameObject currentCube;  // Refer�ncia al cub que ha col�lisionat
    private int score = 0;  // Puntuaci� del jugador
    private int pointsForCurrentOperation = 0;  // Punts per l'operaci� actual

    private zombi jugador;  // Referencia al script del jugador
    private Coroutine answerTimerCoroutine;  // Referencia a la coroutine del temporizador
    private float timeRemaining;  // Temps restant per respondre

    void Start()
    {
        jugador = FindObjectOfType<zombi>();  // Buscar el script del jugador en la escena

        if (operationText == null || answerInputField == null || submitButton == null || timerText == null)
        {
            Debug.LogError("Assegura't que tots els components estan assignats a l'Inspector!");
            return;
        }

        // Amagar l'operaci�, el camp d'entrada i el temporitzador al comen�ar
        operationText.text = "";
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Afegir listener al bot� d'enviar resposta
        submitButton.onClick.AddListener(CheckAnswer);
    }

    void Update()
    {
        // Comprovar si s'ha premut Enter (nom�s si el camp d'entrada est� actiu)
        if (answerInputField.gameObject.activeSelf && Input.GetKeyDown(KeyCode.Return))
        {
            CheckAnswer();  // Cridar la funci� per verificar la resposta quan es prem Enter
        }

        // Actualitzar el temporitzador
        if (timerText.gameObject.activeSelf)
        {
            timerText.text = "TEMPS RESTANT: " + Mathf.Ceil(timeRemaining).ToString();
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        Debug.Log("OnCollisionEnter2D triggered with: " + collision.gameObject.name);
        // Comprovar si la col�lisi� �s amb un cub
        if (collision.gameObject.CompareTag("Cube"))
        {
            Debug.Log("Col�lisi� amb un cub detectada.");
            currentCube = collision.gameObject;  // Guardar refer�ncia al cub que ha col�lisionat
            GenerateAndShowOperation();
        }
    }

    public void GenerateAndShowOperation()
    {
        // Aturar el temps del joc
        Time.timeScale = 0;

        // Generar dos nombres aleatoris
        int number1 = Random.Range(1, 11);
        int number2 = Random.Range(1, 11);

        // Generar un nombre aleatori per seleccionar l'operaci�
        int operationType = Random.Range(0, 4);

        string operation = "";

        switch (operationType)
        {
            case 0:
                // Assegurar que la suma no excedeixi 100
                while (number1 + number2 > 100)
                {
                    number1 = Random.Range(1, 11);
                    number2 = Random.Range(1, 11);
                }
                operation = number1 + " + " + number2;
                correctAnswer = number1 + number2;
                pointsForCurrentOperation = 1;  // Punts per suma
                break;
            case 1:
                // Assegurar-se que el resultat no �s negatiu
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
                number1 = Random.Range(1, 11);
                number2 = Random.Range(1, 11);
                operation = number1 + " * " + number2;
                correctAnswer = number1 * number2;
                pointsForCurrentOperation = 5;  // Punts per multiplicaci�
                break;
            case 3:
                // Assegurar-se que la divisi� no t� residu
                while (number2 == 0 || number1 % number2 != 0)
                {
                    number1 = Random.Range(1, 11);
                    number2 = Random.Range(1, 11);
                }
                operation = number1 + " / " + number2;
                correctAnswer = number1 / number2;
                pointsForCurrentOperation = 10;  // Punts per divisi�
                break;
        }

        // Mostrar l'operaci� al TextMeshPro UI
        Debug.Log("Operaci� generada: " + operation + " = " + correctAnswer);
        operationText.text = operation + " =";
        answerInputField.gameObject.SetActive(true);
        submitButton.gameObject.SetActive(true);
        timerText.gameObject.SetActive(true);

        // Seleccionar autom�ticament l'InputField perqu� el jugador comenci a escriure
        answerInputField.Select();
        answerInputField.ActivateInputField();

        // Iniciar el temporitzador de resposta
        if (answerTimerCoroutine != null)
        {
            StopCoroutine(answerTimerCoroutine);
        }
        answerTimerCoroutine = StartCoroutine(AnswerTimer());
    }

    public void CheckAnswer()
    {
        int playerAnswer;
        if (int.TryParse(answerInputField.text, out playerAnswer))
        {
            if (playerAnswer == correctAnswer)
            {
                Debug.Log("Resposta correcta!");
                score += pointsForCurrentOperation;  // Incrementar la puntuaci� segons el tipus d'operaci�
            }
            else
            {
                Debug.Log("Resposta incorrecta! Torna a intentar-ho.");
                // Reducir la vida del jugador en lugar de cambiar a la escena de Game Over
                jugador.ReducirVida(1);
            }

            // Destruir el cub despu�s de verificar la respuesta
            Destroy(currentCube);

            // Guardar la puntuaci�n cuando se termine
            PlayerPrefs.SetInt("FinalScore", score);

            // Reiniciar el camp d'entrada i amagar-lo
            answerInputField.text = "";
            answerInputField.gameObject.SetActive(false);
            submitButton.gameObject.SetActive(false);
            timerText.gameObject.SetActive(false);

            // Reprendre el temps del joc
            Time.timeScale = 1;

            // Amagar l'operaci�
            operationText.text = "";

            // Detener el temporizador
            if (answerTimerCoroutine != null)
            {
                StopCoroutine(answerTimerCoroutine);
                answerTimerCoroutine = null;
            }
        }
        else
        {
            Debug.Log("Introdueix un nombre v�lid.");
            answerInputField.text = "";
            answerInputField.Select();
            answerInputField.ActivateInputField();
        }
    }


    private IEnumerator AnswerTimer()
    {
        timeRemaining = 5f;
        while (timeRemaining > 0)
        {
            yield return new WaitForSecondsRealtime(1f);
            timeRemaining -= 1f;
        }

        Debug.Log("Temps esgotat! Restant una vida.");
        jugador.ReducirVida(1);

        // Destruir el cub
        Destroy(currentCube);

        // Reiniciar el camp d'entrada i amagar-lo
        answerInputField.text = "";
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);
        timerText.gameObject.SetActive(false);

        // Reprendre el temps del joc
        Time.timeScale = 1;

        // Amagar l'operaci�
        operationText.text = "";

        answerTimerCoroutine = null;
    }
}
