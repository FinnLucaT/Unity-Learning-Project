using System;
using UnityEngine;
using UnityEngine.InputSystem;

public enum EnemyType
{
    EnemyMelee,
    EnemyRanged
}

[Serializable]
public class EnemyPrefabEntry
{
    public EnemyType enemyType;
    public GameObject prefab;
}

public class EnemySpawner : MonoBehaviour
{
    [Header("Enemy")]
    [SerializeField] private EnemyType enemyTypeToSpawn = EnemyType.EnemyMelee;
    [SerializeField] private EnemyPrefabEntry[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private float spawnCheckRadius = 2f;
    [SerializeField] private LayerMask blockingLayers;

    [Header("Interval Spawning")]
    [SerializeField] private bool spawnInInterval = true;
    [Tooltip("The rate at which enemies spawn per second.")]
    [SerializeField] private float spawnRate = 1f;

    [Header("Enemy Settings")]
    [SerializeField] private bool enableCustomChaseSpeed = false;
    [SerializeField] private float customChaseSpeed = 1f;
    [SerializeField] private bool enemyDoesDamage = true;
    [SerializeField] private bool enableCustomDamage = false;
    [SerializeField] private float customDamage = 10f;

    private float spawnTimer;
    private GameObject player;

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player == null)
            FindPlayer();

        if (spawnInInterval && player != null)
            HandleIntervalSpawning();

        //TriggerSpawn();
    }

    private void OnValidate()
    {
        EnemyType[] enemyTypes = (EnemyType[])Enum.GetValues(typeof(EnemyType));
        EnemyPrefabEntry[] updatedEntries = new EnemyPrefabEntry[enemyTypes.Length];

        for (int i = 0; i < enemyTypes.Length; i++)
        {
            EnemyType type = enemyTypes[i];
            EnemyPrefabEntry existingEntry = null;

            if (enemyPrefabs != null)
            {
                foreach (EnemyPrefabEntry entry in enemyPrefabs)
                {
                    if (entry != null && entry.enemyType == type)
                    {
                        existingEntry = entry;
                        break;
                    }
                }
            }

            if (existingEntry != null)
            {
                updatedEntries[i] = existingEntry;
            }
            else
            {
                updatedEntries[i] = new EnemyPrefabEntry
                {
                    enemyType = type
                };
            }
        }

        enemyPrefabs = updatedEntries;
    }

    private void FindPlayer()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    private void HandleIntervalSpawning()
    {
        if (spawnRate <= 0f)
            return;

        spawnTimer += Time.deltaTime;
        float timeBetweenSpawns = 1f / spawnRate;

        if (spawnTimer >= timeBetweenSpawns)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    public void SpawnEnemy(
        EnemyType? enemyType = null,
        bool? doesDamage = null,
        float? customDamage = null,
        float? customChaseSpeed = null)
    {
        bool isBlocked = Physics.CheckSphere(
            transform.position,
            spawnCheckRadius,
            blockingLayers
        );

        if (isBlocked)
            return;

        if (player == null)
        {
            FindPlayer();

            if (player == null)
                return;
        }

        EnemyType selectedEnemyType = enemyType ?? enemyTypeToSpawn;
        bool selectedDoesDamage = doesDamage ?? enemyDoesDamage;
        GameObject enemyPrefab = GetEnemyPrefab(selectedEnemyType);

        if (enemyPrefab == null)
        {
            Debug.LogWarning($"No prefab assigned for enemy type {selectedEnemyType}.");
            return;
        }

        Vector3 direction = player.transform.position - transform.position;
        Quaternion spawnRotation = Quaternion.identity;

        if (direction != Vector3.zero)
            spawnRotation = Quaternion.LookRotation(direction);

        GameObject enemyInstance = Instantiate(
            enemyPrefab,
            transform.position,
            spawnRotation
        );

        EnemyController enemyBehaviour = enemyInstance.GetComponent<EnemyController>();

        if (enemyBehaviour == null)
            return;

        enemyBehaviour.isDoingDamage = selectedDoesDamage;

        if (customDamage.HasValue)
        {
            enemyBehaviour.damageDealt = customDamage.Value;
        }
        else if (enableCustomDamage)
        {
            enemyBehaviour.damageDealt = this.customDamage;
        }

        if (customChaseSpeed.HasValue)
        {
            enemyBehaviour.chaseSpeed = customChaseSpeed.Value;
        }
        else if (enableCustomChaseSpeed)
        {
            enemyBehaviour.chaseSpeed = this.customChaseSpeed;
        }
    }

    private GameObject GetEnemyPrefab(EnemyType enemyType)
    {
        foreach (EnemyPrefabEntry entry in enemyPrefabs)
        {
            if (entry.enemyType == enemyType)
                return entry.prefab;
        }

        return null;
    }

    private void TriggerSpawn()
    {
        //if (Keyboard.current.altKey.wasPressedThisFrame)
            SpawnEnemy();
    }
}
