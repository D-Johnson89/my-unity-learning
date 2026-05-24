using System;
using UnityEngine;

// Serializable class to hold configuration for spawning enemies or other game objects, includes prefab reference, spawn count, interval, position, and parent transform for organization in the hierarchy
[Serializable]
public class SpawnConfig
{
    public GameObject prefab; // Prefab to spawn
    public int spawnCount; // Number of instances to spawn
    public float spawnInterval; // Time interval between spawns
    public Transform spawnPosition; // Position and rotation for spawning
    public Transform spawnParent; // Parent transform for organizing spawned objects in the hierarchy
}