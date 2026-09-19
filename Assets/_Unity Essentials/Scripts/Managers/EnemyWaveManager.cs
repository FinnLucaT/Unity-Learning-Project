using System;
using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private float checkRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    [Header("Spawner Settings")]
    [SerializeField] private EnemySpawner enemySpawner;

    [Header("Wave Settings")]
    [SerializeField] private EnemyWave[] enemyWaves;

    public event Action<EnemyWave> OnWaveStarted;

    private int currentWave = 0;
    private bool isWaveActive = false;
    private bool hasLastWaveEnded = false;
    private bool hasFinishedSpawning = false;

    private int enemiesAlive = 0;
    private List<GameObject> aliveEnemies = new List<GameObject>();

    private void OnEnable()
    {
        enemySpawner.OnWaveEnemySpawned += AddAliveEnemy;
        enemySpawner.OnWaveSpawningFinished += WaveSpawningFinished;
    }

    private void OnDisable()
    {
        enemySpawner.OnWaveEnemySpawned -= AddAliveEnemy;
        enemySpawner.OnWaveSpawningFinished -= WaveSpawningFinished;
    }

    private void Update()
    {
        if (IsPlayerStartingNextWave())
            StartWave();
    }

    private void AddAliveEnemy(GameObject enemy)
    {
        aliveEnemies.Add(enemy);
        enemiesAlive++;

        Health health = enemy.GetComponent<Health>();

        if (health != null)
            health.EventOnDeath += RemoveAliveEnemy;
    }

    private void RemoveAliveEnemy(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);
        enemiesAlive--;

        Health health = enemy.GetComponent<Health>();

        if (health != null)
            health.EventOnDeath -= RemoveAliveEnemy;

        CheckWaveCompleted();
    }

    private void WaveSpawningFinished()
    {
        hasFinishedSpawning = true;

        CheckWaveCompleted();
    }

    private void CheckWaveCompleted()
    {
        if (hasFinishedSpawning && enemiesAlive == 0)
            EndWave();
    }

    private void EndWave()
    {
        isWaveActive = false;

        Debug.Log($"Wave {currentWave} completed!");
    }

    private bool IsPlayerStartingNextWave()
    {
        bool isPlayerInRange = Physics.CheckSphere(
            transform.position,
            checkRadius,
            playerLayer
        );

        if (
            isPlayerInRange &&
            Keyboard.current.eKey.wasPressedThisFrame &&
            !isWaveActive
        )
        {
            return true;
        }

        return false;
    }

    private void StartWave()
    {
        if (hasLastWaveEnded)
        {
            Debug.Log("All waves have already been completed.");
            return;
        }

        currentWave++;

        if (currentWave > enemyWaves.Length)
        {
            Debug.Log("All waves completed!");
            hasLastWaveEnded = true;
            return;
        }

        isWaveActive = true;
        hasFinishedSpawning = false;
        enemiesAlive = 0;
        aliveEnemies.Clear();

        OnWaveStarted?.Invoke(enemyWaves[currentWave - 1]);
    }
}

[Serializable]
public class EnemyWave
{
    public WaveContent[] waveContent;
}

[Serializable]
public class WaveContent
{
    public EnemyType enemyType;
    public int numberToSpawn;
}