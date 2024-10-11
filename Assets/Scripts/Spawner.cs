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

    // Distancia m�nima entre las posiciones generadas para evitar repetici�n
    public float minDistanceBetweenSpawns = 1.0f;

    void Start()
    {
        // Obt�n la c�mara principal
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
            // Intentamos generar una posici�n v�lida dentro de la c�mara
            Vector3 position = GetUniqueRandomPositionWithinCameraBounds();

            // Selecciona un prefab de la lista, ya sea uno espec�fico o al azar si hay varios
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

            // Instancia el prefab en la posici�n generada
            GameObject currentEntity = Instantiate(prefabToSpawn, position, Quaternion.identity);

            // Asigna un nombre al objeto instanciado
            currentEntity.name = spawnManagerValues.spawnName + instanceNumber;

            instanceNumber++;
        }
    }

    // M�todo para obtener una posici�n �nica aleatoria dentro de los l�mites de la c�mara
    Vector3 GetUniqueRandomPositionWithinCameraBounds()
    {
        Vector3 position;
        int attempts = 0; // Contador para evitar bucles infinitos

        do
        {
            // Generamos una posici�n aleatoria dentro de los l�mites de la c�mara
            position = GetRandomPositionWithinCameraBounds();
            attempts++;
        }
        // Seguimos generando una nueva posici�n hasta que encontremos una que no est� demasiado cerca de las ya generadas
        while (!IsPositionValidDuringCycle(position) && attempts < 20);

        // Si encontramos una posici�n v�lida, la a�adimos a la lista de posiciones generadas en este ciclo
        generatedPositionsDuringCycle.Add(position);
        return position;
    }

    // M�todo para generar una posici�n aleatoria dentro de los l�mites visibles de la c�mara
    Vector3 GetRandomPositionWithinCameraBounds()
    {
        // Obt�n las posiciones de las esquinas de la c�mara en coordenadas del mundo
        Vector3 bottomLeft = mainCamera.ViewportToWorldPoint(new Vector3(0, 0, mainCamera.nearClipPlane));
        Vector3 topRight = mainCamera.ViewportToWorldPoint(new Vector3(1, 1, mainCamera.nearClipPlane));

        // Genera una posici�n aleatoria dentro de estos l�mites
        float randomX = Random.Range(bottomLeft.x, topRight.x);
        float randomY = Random.Range(bottomLeft.y, topRight.y);

        // Devuelve la posici�n aleatoria (Z puede ser 0 si trabajas en 2D)
        return new Vector3(randomX, randomY, 0);
    }

    // M�todo para verificar si una posici�n generada es v�lida durante el ciclo actual (no muy cerca de otras)
    bool IsPositionValidDuringCycle(Vector3 position)
    {
        // Comprobamos la distancia de esta nueva posici�n con las ya generadas en el ciclo actual
        foreach (Vector3 existingPosition in generatedPositionsDuringCycle)
        {
            if (Vector3.Distance(existingPosition, position) < minDistanceBetweenSpawns)
            {
                return false; // Si la distancia es menor que el l�mite, la posici�n no es v�lida
            }
        }
        return true; // La posici�n es v�lida si est� lo suficientemente lejos de las dem�s en este ciclo
    }
}
