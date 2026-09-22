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
    public event Action<int, int> OnWaveNumberChanged;
    public event Action OnWaveFinished;
    public event Action OnAllWavesCompleted;

    private int currentWave = 0;
    private bool isWaveActive = false;
    private bool hasFinishedSpawning = false;

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

    private void StartWave()
    {
        if (currentWave >= enemyWaves.Length)
        {
            Debug.Log("All waves have already been completed.");
            return;
        }

        currentWave++;

        isWaveActive = true;
        hasFinishedSpawning = false;
        aliveEnemies.Clear();

        OnWaveStarted?.Invoke(enemyWaves[currentWave - 1]);
        OnWaveNumberChanged?.Invoke(currentWave, enemyWaves.Length);
    }

    private void AddAliveEnemy(GameObject enemy)
    {
        aliveEnemies.Add(enemy);

        Health health = enemy.GetComponent<Health>();

        if (health != null)
            health.EventOnDeath += RemoveAliveEnemy;
    }

    private void RemoveAliveEnemy(GameObject enemy)
    {
        aliveEnemies.Remove(enemy);

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
        if (hasFinishedSpawning && aliveEnemies.Count == 0)
            EndWave();
    }

    private void EndWave()
    {
        isWaveActive = false;

        OnWaveFinished?.Invoke();
        Debug.Log($"Wave {currentWave} completed!");

        if (currentWave >= enemyWaves.Length)
        {
            OnAllWavesCompleted?.Invoke();
        }
    }

    private bool IsPlayerStartingNextWave()
    {
        bool isPlayerInRange = Physics.CheckSphere(
            transform.position,
            checkRadius,
            playerLayer
        );

        return //isPlayerInRange &&
               Keyboard.current.eKey.wasPressedThisFrame &&
               !isWaveActive;
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