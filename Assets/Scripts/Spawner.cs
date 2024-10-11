using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public List<GameObject> entitiesToSpawn;
    public SpawnerManager spawnManagerValues;

    private int instanceNumber = 1;

    public float initSpawnTime = 0.2f;
    public float reSpawnTime = 7.0f;

    private Camera mainCamera;

    // Lista para almacenar las posiciones generadas en cada llamada a SpawnEntities
    private List<Vector3> generatedPositionsDuringCycle;

    // Distancia mínima entre las posiciones generadas para evitar repetición
    public float minDistanceBetweenSpawns = 1.0f;

    void Start()
    {
        // Obtén la cámara principal
        mainCamera = Camera.main;

        // Llama repetidamente a SpawnEntities
        InvokeRepeating(nameof(SpawnEntities), initSpawnTime, reSpawnTime);
    }

    void SpawnEntities()
    {
        // Limpiar la lista de posiciones generadas al inicio de cada ciclo
        generatedPositionsDuringCycle = new List<Vector3>();

        for (int i = 0; i < spawnManagerValues.numberOfPrefabs; i++)
        {
            // Intentamos generar una posición válida dentro de la cámara
            Vector3 position = GetUniqueRandomPositionWithinCameraBounds();

            // Selecciona un prefab de la lista, ya sea uno específico o al azar si hay varios
            GameObject prefabToSpawn;
            if (entitiesToSpawn.Count == 1)
            {
                prefabToSpawn = entitiesToSpawn[0];
            }
            else
            {
                int randomIndex = Random.Range(0, entitiesToSpawn.Count);
                prefabToSpawn = entitiesToSpawn[randomIndex];
            }

            // Instancia el prefab en la posición generada
            GameObject currentEntity = Instantiate(prefabToSpawn, position, Quaternion.identity);

            // Asigna un nombre al objeto instanciado
            currentEntity.name = spawnManagerValues.spawnName + instanceNumber;

            instanceNumber++;
        }
    }

    // Método para obtener una posición única aleatoria dentro de los límites de la cámara
    Vector3 GetUniqueRandomPositionWithinCameraBounds()
    {
        Vector3 position;
        int attempts = 0; // Contador para evitar bucles infinitos

        do
        {
            // Generamos una posición aleatoria dentro de los límites de la cámara
            position = GetRandomPositionWithinCameraBounds();
            attempts++;
        }
        // Seguimos generando una nueva posición hasta que encontremos una que no esté demasiado cerca de las ya generadas
        while (!IsPositionValidDuringCycle(position) && attempts < 20);

        // Si encontramos una posición válida, la añadimos a la lista de posiciones generadas en este ciclo
        generatedPositionsDuringCycle.Add(position);
        return position;
    }

    // Método para generar una posición aleatoria dentro de los límites visibles de la cámara
    Vector3 GetRandomPositionWithinCameraBounds()
    {
        // Obtén las posiciones de las esquinas de la cámara en coordenadas del mundo
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Genera una posición aleatoria dentro de estos límites
        float randomX = Random.Range(bottomLeft.x, topRight.x);
        float randomY = Random.Range(bottomLeft.y, topRight.y);

        // Devuelve la posición aleatoria (Z puede ser 0 si trabajas en 2D)
        return new Vector3(randomX, randomY, 0);
    }

    // Método para verificar si una posición generada es válida durante el ciclo actual (no muy cerca de otras)
    bool IsPositionValidDuringCycle(Vector3 position)
    {
        // Comprobamos la distancia de esta nueva posición con las ya generadas en el ciclo actual
        foreach (Vector3 existingPosition in generatedPositionsDuringCycle)
        {
            if (Vector3.Distance(existingPosition, position) < minDistanceBetweenSpawns)
            {
                return false; // Si la distancia es menor que el límite, la posición no es válida
            }
        }
        return true; // La posición es válida si está lo suficientemente lejos de las demás en este ciclo
    }
}
