using UnityEngine;
using UnityEngine.InputSystem;

public class EnemyWaveManager : MonoBehaviour
{
    [Header("Button Settings")]
    [SerializeField] private float checkRadius = 2f;
    [SerializeField] private LayerMask playerLayer;

    private int currentWave = 0;
    private bool isWaveActive = false;

    private void Update()
    {
        if (IsPlayerStartingNextWave())
        {
            StartWave();
        }
    }

    private bool IsPlayerStartingNextWave()
    {
        bool isPlayerInRange = Physics.CheckSphere(
            transform.position,
            checkRadius,
            playerLayer
        );

        if (isPlayerInRange && Keyboard.current.eKey.wasPressedThisFrame && !isWaveActive)
        {
            return true;
        }

        return false;
    }

    private void StartWave()
    {
        isWaveActive = true;
        currentWave++;

        Debug.Log("Wave: " + currentWave + " started.");

        isWaveActive = false;
    }

    // What happens during a wave
}
