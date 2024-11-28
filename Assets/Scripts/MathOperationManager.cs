using UnityEngine;
using TMPro;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MathOperationManager : MonoBehaviour
{
    public TMP_Text operationText;  // Refer�ncia al TextMeshPro UI on es mostrar� l'operaci�
    public TMP_InputField answerInputField;  // Refer�ncia al InputField on el jugador escriur� la seva resposta
    public Button submitButton;  // Refer�ncia al bot� per enviar la resposta

    private int correctAnswer;  // La resposta correcta de l'operaci�
    private GameObject currentCube;  // Refer�ncia al cub que ha col�lisionat
    private int score = 0;  // Puntuaci� del jugador
    private int pointsForCurrentOperation = 0;  // Punts per l'operaci� actual

    void Start()
    {
        if (operationText == null || answerInputField == null || submitButton == null)
        {
            Debug.LogError("Assegura't que tots els components estan assignats a l'Inspector!");
            return;
        }

        // Amagar l'operaci� i el camp d'entrada al comen�ar
        operationText.text = "";
        answerInputField.gameObject.SetActive(false);
        submitButton.gameObject.SetActive(false);

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
        int number1 = Random.Range(1, 100);
        int number2 = Random.Range(1, 10);

        // Generar un nombre aleatori per seleccionar l'operaci�
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
                operation = number1 + " * " + number2;
                correctAnswer = number1 * number2;
                pointsForCurrentOperation = 5;  // Punts per multiplicaci�
                break;
            case 3:
                // Assegurar-se que la divisi� no t� residu
                while (number2 == 0 || number1 % number2 != 0)
                {
                    number1 = Random.Range(1, 100);
                    number2 = Random.Range(1, 10);
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

        // Seleccionar autom�ticament l'InputField perqu� el jugador comenci a escriure
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
                score += pointsForCurrentOperation;  // Incrementar la puntuaci� segons el tipus d'operaci�

                // Destruir el cub despr�s de resoldre correctament l'operaci�
                Destroy(currentCube);

                // Reiniciar el camp d'entrada i amagar-lo
                answerInputField.text = "";
                answerInputField.gameObject.SetActive(false);
                submitButton.gameObject.SetActive(false);

                // Reprendre el temps del joc
                Time.timeScale = 1;

                // Amagar l'operaci�
                operationText.text = "";
            }
            else
            {
                Debug.Log("Resposta incorrecta! Torna a intentar-ho.");
                // Aqu� es canvia a la escena de Game Over
                PlayerPrefs.SetInt("FinalScore", score);  // Guarda la puntuaci� final
                SceneManager.LoadScene("GameOver");  // Aseg�rate de que la escena "Game Over" est� en los Build Settings
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
}
