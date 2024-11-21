using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Prefab del cubo que vamos a generar
    public GameObject cubePrefab;

    // Rango de aparici�n en el eje X
    public float xRange = 8f;

    // Tiempo entre la aparici�n de cubos
    public float spawnRate = 2f;

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
    }

    void Update()
    {
        // Si ha pasado suficiente tiempo, genera un nuevo cubo
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            // Generar una posici�n aleatoria en el eje X dentro de los l�mites de la pantalla
            float randomX = Mathf.Clamp(Random.Range(-xRange, xRange), minX, maxX);
            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

            // Instanciar el cubo
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
            // Asignar la velocidad de ca�da
            newCube.GetComponent<Rigidbody2D>().velocity = new Vector2(0, -fallSpeed);
        }
    }
}
