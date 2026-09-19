using System;
using System.Collections;
using UnityEngine;
using System.Collections.Generic;

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
    [Header("Wave Settings")]
    [SerializeField] private EnemyWaveManager waveManager;

    [Header("Enemy")]
    [SerializeField] private EnemyType enemyTypeToSpawn = EnemyType.EnemyMelee;
    [SerializeField] private EnemyPrefabEntry[] enemyPrefabs;

    [Header("Spawn Settings")]
    [SerializeField] private GameObject[] spawnPoints;
    [SerializeField] private float spawnDelay = 1f;
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

    public event Action<GameObject> OnWaveEnemySpawned;
    public event Action OnWaveSpawningFinished;

    private float spawnTimer;
    private GameObject player;


    private void OnEnable()
    {
        waveManager.OnWaveStarted += SpawnEnemyWave;
    }

    private void OnDisable()
    {
        waveManager.OnWaveStarted -= SpawnEnemyWave;
    }

    private void Start()
    {
        FindPlayer();
    }

    private void Update()
    {
        if (player == null)
            FindPlayer();

        if (spawnInInterval && player != null)
            SpawnEnemyInInterval();
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

    private void SpawnEnemyInInterval()
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

    private void SpawnEnemyWave(EnemyWave wave)
    {
        StartCoroutine(SpawnEnemyWaveCoroutine(wave));
    }

    private IEnumerator SpawnEnemyWaveCoroutine(EnemyWave wave)
    {
        List<EnemyType> enemiesToSpawn = new List<EnemyType>();

        foreach (WaveContent content in wave.waveContent)
        {
            for (int i = 0; i < content.numberToSpawn; i++)
            {
                enemiesToSpawn.Add(content.enemyType);
            }
        }

        for (int i = enemiesToSpawn.Count - 1; i > 0; i--)
        {
            int randomIndex = UnityEngine.Random.Range(0, i + 1);

            EnemyType temp = enemiesToSpawn[i];
            enemiesToSpawn[i] = enemiesToSpawn[randomIndex];
            enemiesToSpawn[randomIndex] = temp;
        }

        foreach (EnemyType enemyType in enemiesToSpawn)
        {
            Vector3 position;
            bool isBlocked;

            do
            {
                position = spawnPoints[UnityEngine.Random.Range(0, spawnPoints.Length)].transform.position;

                isBlocked = Physics.CheckSphere(
                    position,
                    spawnCheckRadius,
                    blockingLayers
                );

            } while (isBlocked);

            SpawnEnemy(enemyType: enemyType, spawnPos: position, isWaveEnemy: true);

            yield return new WaitForSeconds(spawnDelay);
        }

        OnWaveSpawningFinished?.Invoke();
    }

    public void SpawnEnemy(
        EnemyType? enemyType = null,
        Vector3? spawnPos = null,
        bool? doesDamage = null,
        float? customDamage = null,
        float? customChaseSpeed = null,
        bool isWaveEnemy = false)
    {
        bool isBlocked = Physics.CheckSphere(
            spawnPos ?? transform.position,
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

        Vector3 direction = player.transform.position - (spawnPos ?? transform.position);
        Quaternion spawnRotation = Quaternion.identity;

        if (direction != Vector3.zero)
            spawnRotation = Quaternion.LookRotation(direction);

        GameObject enemyInstance = Instantiate(
            enemyPrefab,
            spawnPos ?? transform.position,
            spawnRotation
        );
        if (isWaveEnemy)
            OnWaveEnemySpawned?.Invoke(enemyInstance);

        EnemyController enemyBehaviour = enemyInstance.GetComponent<EnemyController>();

        if (enemyBehaviour == null)
            return;

        enemyBehaviour.isDoingDamage = selectedDoesDamage;

        if (customDamage.HasValue)
        {
            enemyBehaviour.damageDealtMelee = customDamage.Value;
        }
        else if (enableCustomDamage)
        {
            enemyBehaviour.damageDealtMelee = this.customDamage;
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
}