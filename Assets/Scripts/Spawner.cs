using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entityToSpawn;

    public SpawnerManager spawnManagerValues;

    public int instanceNumber = 1;

    public float initSpawnTime = 0.2f;
    public float reSpawnTime = 7.0f;

    // Lista para almacenar las posiciones generadas
    private List<Vector3> generatedPositions = new List<Vector3>();

    void Start()
    {
        InvokeRepeating(nameof(SpawnEntities), initSpawnTime, reSpawnTime);
    }

    void SpawnEntities()
    {

        for (int i = 0; i < spawnManagerValues.numberOfPrefabs; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0);

            GameObject currentEntity = Instantiate(entityToSpawn, position, Quaternion.identity);

            currentEntity.name = spawnManagerValues.spawnName + instanceNumber;

            instanceNumber++;
        }
    }
}
