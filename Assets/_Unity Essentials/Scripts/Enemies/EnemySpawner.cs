using System.Collections;
using UnityEngine;

public enum EnemyType
{
    EnemyNormal,
    EnemyElite,
    EnemyBoss
}

public class EnemySpawner : MonoBehaviour
{
    [Tooltip("The Enemy type to spawn.")]
    public EnemyType enemyTypeToSpawn = EnemyType.EnemyNormal;

    [Tooltip("Array of enemy prefabs corresponding to EnemyType enum.")]
    public GameObject[] enemyPrefabs = new GameObject[3];

    [Tooltip("The rate at which enemies spawn (per second).")]
    public float spawnRate = 1f;
    public bool isActive = true;
    public bool enemyDoesDamage = true;
    public bool enableCustomDamage = false;
    public float enemyCustomDamage = 10f;

    private float spawnTimer = 0f;

    void Update()
    {
        if (isActive)
            SpawnEnemy();
    }

    private void SpawnEnemy()
    {
        GameObject enemyPrefab = enemyPrefabs[(int)enemyTypeToSpawn];

        if (enemyPrefab == null)
            return;

        float spawnInterval = 1f / spawnRate;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            GameObject enemyInstance = Instantiate(enemyPrefab, transform.position, transform.rotation);

            if (enemyDoesDamage)
            {
                enemyInstance.GetComponent<EnemyBehaviour>().isDoingDamage = true;

                if (enableCustomDamage)
                    enemyInstance.GetComponent<EnemyBehaviour>().damageDealt = enemyCustomDamage;
            }

            spawnTimer = 0f;
        }
    }
}
