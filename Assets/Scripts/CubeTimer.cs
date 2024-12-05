using UnityEngine;

public class CubeTimer : MonoBehaviour
{
    public float tiempoEnSuelo = 0f;
    private bool enSuelo = false;
    private zombi jugador;

    void Start()
    {
        jugador = FindObjectOfType<zombi>();
    }

    void Update()
    {
        if (enSuelo)
        {
            tiempoEnSuelo += Time.deltaTime;

            if (tiempoEnSuelo > 2f)  // Si el cubo lleva más de 2 segundos en el suelo
            {
                jugador.ReducirVida(1);  // Restamos vida al jugador
                Destroy(gameObject);  // Destruimos el cubo
            }
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = true;  // El cubo ha tocado el suelo
        }
    }

    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            enSuelo = false;  // El cubo ha dejado el suelo
            tiempoEnSuelo = 0f;  // Reiniciamos el temporizador si deja de estar en el suelo
        }
    }
}
