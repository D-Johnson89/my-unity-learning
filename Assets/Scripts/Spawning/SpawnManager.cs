using UnityEngine;
using System.Collections;

// Manages spawning of enemies or other game objects based on an array of SpawnConfig, starts a coroutine for each config to handle timed spawning of specified prefabs at designated positions and intervals
public class SpawnManager : MonoBehaviour
{
    [SerializeField] private SpawnConfig[] spawnConfigs;

    // Initiates a spawn routine for each configuration in the spawnConfigs array, allowing for multiple types of enemies or objects to be spawned with different settings simultaneously
    private void Start()
    {
        foreach (var config in spawnConfigs)
        {
            StartCoroutine(SpawnRoutine(config));
        }
    }

    // Coroutine to manage spawning of a specific prefab based on the provided SpawnConfig, instantiates the prefab at the specified position and rotation, and waits for the defined interval before spawning the next instance until the spawn count is reached
    private IEnumerator SpawnRoutine(SpawnConfig config)
    {
        for (int i = 0; i < config.spawnCount; i++)
        {
            Instantiate(config.prefab, config.spawnPosition.position, config.spawnPosition.rotation, config.spawnParent);
            yield return new WaitForSeconds(config.spawnInterval);
        }
    }
}