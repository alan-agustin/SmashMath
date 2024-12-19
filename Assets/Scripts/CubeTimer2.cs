using UnityEngine;

public class CubeTimer2 : MonoBehaviour
{
    public float tiempoEnSuelo = 0f;
    private bool enSuelo = false;
    private zombi jugador;

    // Variable para identificar si este es el cubo especial que suma vida
    public bool esCuboDeVida = false;

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
