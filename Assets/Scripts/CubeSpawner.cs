using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Prefab del cubo que vamos a generar
    public GameObject cubePrefab;

    // Rango de aparición en el eje X
    public float xRange = 7.5f;

    // Tiempo entre la aparición de cubos
    public float spawnRate = 0.5f;

    // Altura desde la cual los cubos aparecerán
    public float spawnHeight = 5f;

    // Control del tiempo para el próximo spawn
    private float nextSpawnTime = 0f;

    // Límites en X calculados
    private float minX, maxX;

    // Velocidad de caída de los cubos
    public float fallSpeed = 2f;

    void Start()
    {
        // Calcular los límites de la pantalla
        Camera cam = Camera.main;
        float screenHalfWidthInWorldUnits = cam.aspect * cam.orthographicSize;
        minX = -screenHalfWidthInWorldUnits;
        maxX = screenHalfWidthInWorldUnits;
    }

    void Update()
    {
        // Si ha pasado suficiente tiempo, genera un nuevo cubo
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            // Generar una posición aleatoria en el eje X dentro de los límites de la pantalla
            float randomX = Random.Range(minX, maxX);
            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

            // Instanciar el cubo
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            // Asignar la velocidad de caída
            newCube.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallSpeed);

            // Asegurar que el cubo tenga un Rigidbody2D para que se acumule al tocar el suelo
            Rigidbody2D rb = newCube.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = newCube.AddComponent<Rigidbody2D>();
            }

            // Ajustar propiedades del Rigidbody2D para evitar que los cubos reboten demasiado
            rb.gravityScale = 1f; // Asegúrate de que el cubo caiga
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evita que roten al colisionar

            // Añadir un script para gestionar el temporizador del cubo
            CubeTimer cubeTimer = newCube.AddComponent<CubeTimer>();
        }
    }
}
