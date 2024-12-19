using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner2 : MonoBehaviour
{
    // Prefab del cubo que vamos a generar
    public GameObject cubePrefab;

    // Rango de aparici�n en el eje X
    public float xRange = 7.5f;

    // Tiempo entre la aparici�n de cubos (solo necesario si decides tener un tiempo fijo)
    public float spawnRate = 10f;

    // Altura desde la cual los cubos aparecer�n
    public float spawnHeight = 5f;

    // Control del tiempo para el pr�ximo spawn
    private float nextSpawnTime = 0f;

    // L�mites en X calculados
    private float minX, maxX;

    // Velocidad de ca�da de los cubos
    public float fallSpeed = 2f;

    void Start()
    {
        // Calcular los l�mites de la pantalla
        Camera cam = Camera.main;
        float screenHalfWidthInWorldUnits = cam.aspect * cam.orthographicSize;
        minX = -screenHalfWidthInWorldUnits;
        maxX = screenHalfWidthInWorldUnits;

        // Establecer el tiempo para el primer spawn a 10 segundos despu�s de iniciar
        nextSpawnTime = Time.time + spawnRate;
    }

    void Update()
    {
        // Si ha pasado suficiente tiempo, genera un nuevo cubo
        if (Time.time > nextSpawnTime)
        {
            // Asignar un nuevo tiempo de spawn aleatorio dentro del rango de 2 a 5 segundos
            nextSpawnTime = Time.time + Random.Range(2f, 5f); // Esto har� que los cubos respawneen entre 2 y 5 segundos

            // Generar una posici�n aleatoria en el eje X dentro de los l�mites de la pantalla
            float randomX = Random.Range(minX, maxX);
            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

            // Instanciar el cubo
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

            // Asignar la velocidad de ca�da
            newCube.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallSpeed);

            // Asegurar que el cubo tenga un Rigidbody2D para que se acumule al tocar el suelo
            Rigidbody2D rb = newCube.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = newCube.AddComponent<Rigidbody2D>();
            }

            // Ajustar propiedades del Rigidbody2D para evitar que los cubos reboten demasiado
            rb.gravityScale = 1f; // Aseg�rate de que el cubo caiga
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evita que roten al colisionar

            // A�adir un script para gestionar el temporizador del cubo
            CubeTimer cubeTimer = newCube.AddComponent<CubeTimer>();
        }
    }
}
