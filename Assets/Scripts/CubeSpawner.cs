using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CubeSpawner : MonoBehaviour
{
    // Prefab del cubo que vamos a generar
    public GameObject cubePrefab;

    // Rango de aparición en el eje X
    public float xRange = 8f;

    // Tiempo entre la aparición de cubos
    public float spawnRate = 2f;

    // Altura desde la cual los cubos aparecerán
    public float spawnHeight = 5f;

    // Control del tiempo para el próximo spawn
    private float nextSpawnTime = 0f;

    // Update se llama una vez por frame
    void Update()
    {
        // Si ha pasado suficiente tiempo, genera un nuevo cubo
        if (Time.time > nextSpawnTime)
        {
            nextSpawnTime = Time.time + spawnRate;

            // Generar una posición aleatoria en el eje X
            float randomX = Random.Range(-xRange, xRange);
            Vector2 spawnPosition = new Vector2(randomX, spawnHeight);

            // Instanciar el cubo
            GameObject newCube = Instantiate(cubePrefab, spawnPosition, Quaternion.identity);

            // Asegurar que el cubo tenga un Rigidbody2D para que se acumule al tocar el suelo
            Rigidbody2D rb = newCube.GetComponent<Rigidbody2D>();
            if (rb == null)
            {
                rb = newCube.AddComponent<Rigidbody2D>();
            }

            // Ajustar propiedades del Rigidbody2D para evitar que los cubos reboten demasiado
            rb.gravityScale = 1f; // Asegúrate de que el cubo caiga
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Evita que roten al colisionar
        }
    }
}
