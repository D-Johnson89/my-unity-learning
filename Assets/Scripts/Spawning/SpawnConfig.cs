using System;
using UnityEngine;

[Serializable]
public class SpawnConfig
{
    public GameObject prefab;
    public int spawnCount;
    public float spawnInterval;
    public Transform spawnPosition;
    public Transform spawnParent;
}