using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private GameObject enemyPrefab;
    [SerializeField] private float spawnTimer;
    [SerializeField] private List<Transform> locations;
    private Transform cachedTransform;

    private void Awake()
    {
        cachedTransform = transform;
        InvokeRepeating(nameof(Spawn), 0f, spawnTimer);
    }

    private void Spawn()
    {
        int randomLocation = Random.Range(0, locations.Count);
        Instantiate(enemyPrefab, locations[randomLocation].position, Quaternion.identity);
    }
}
