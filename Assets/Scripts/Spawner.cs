using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawner : MonoBehaviour
{
    public GameObject entityToSpawn;

    public SpawnerManager spawnManagerValues;

    int instanceNumber = 1;


    void Start()
    {
        SpawnEntities();
    }

    void SpawnEntities()
    {

        for (int i = 0; i < spawnManagerValues.numberOfPrefabs; i++)
        {
            Vector3 position = new Vector3(Random.Range(-10.0f, 10.0f), Random.Range(-5.0f, 5.0f), 0);

            GameObject currentEntity = Instantiate(entityToSpawn, position, Quaternion.identity);

            currentEntity.name = spawnManagerValues.spawnName + instanceNumber;

            currentEntity.GetComponent<Renderer>().material.color = Random.ColorHSV();

            instanceNumber++;
        }
    }
}
