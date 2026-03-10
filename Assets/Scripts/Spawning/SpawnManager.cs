using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class SpawnManager : MonoBehaviour
{
    public SpawnConfig[] spawnConfigs;

    private void Start()
    {
        foreach (var config in spawnConfigs)
        {
            StartCoroutine(SpawnRoutine(config));
        }
    }

    private IEnumerator SpawnRoutine(SpawnConfig config)
    {
        for (int i = 0; i < config.spawnCount; i++)
        {
            
            Instantiate(config.prefab, config.spawnPosition, config.spawnRotation, config.spawnParent);
            yield return new WaitForSeconds(config.spawnInterval);
        }
    }
}