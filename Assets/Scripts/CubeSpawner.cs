using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{

    private float _vel;
    
    
    
    
    
    
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

    // Update se llama una vez por frame
    void Update()
    {
        // Si ha pasado suficiente tiempo, genera un nuevo cubo
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            // Generar una posici�n aleatoria en el eje X
            float randomX = Random.Range(-xRange, xRange);
            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

            // Instanciar el cubo
            Instantiate(cubePrefab, spawnPosition, Quaternion.identity);
        }
    }
}
