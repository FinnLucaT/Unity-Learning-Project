using UnityEngine;
using TMPro;

public class HudController : MonoBehaviour
{
    [SerializeField] private EnemyWaveManager waveManager;
    [SerializeField] private Health playerHealth;
    [SerializeField] private TMP_Text waveText;
    [SerializeField] private TMP_Text invincibilityText;

    private void OnEnable()
    {
        waveManager.OnWaveNumberChanged += UpdateCurrentWaveText;
        waveManager.OnWaveFinished += UpdateBetweenWaveText;
        waveManager.OnAllWavesCompleted += UpdateAllWavesCompletedText;

        if (playerHealth != null)
            playerHealth.EventOnInvincibilityChanged += UpdateInvincibilityText;
        else
            Debug.LogWarning(
                $"Player health is null! Object: {gameObject.name}, Scene: {gameObject.scene.name}",
                gameObject
            );
    }

    private void OnDisable()
    {
        waveManager.OnWaveNumberChanged -= UpdateCurrentWaveText;
        waveManager.OnWaveFinished -= UpdateBetweenWaveText;
        waveManager.OnAllWavesCompleted -= UpdateAllWavesCompletedText;

        if (playerHealth != null)
            playerHealth.EventOnInvincibilityChanged -= UpdateInvincibilityText;
    }

    private void UpdateCurrentWaveText(int currentWave, int totalWaves)
    {
        if (waveText == null)
        {
            Debug.LogWarning("Wave text reference is not set in the inspector.");
            return;
        }

        waveText.text = $"Wave: {currentWave} of {totalWaves}";
    }

    private void UpdateBetweenWaveText()
    {
        if (waveText == null)
        {
            Debug.LogWarning("Wave text reference is not set in the inspector.");
            return;
        }

        waveText.text = "Press 'E' to start the next wave.";
    }

    private void UpdateAllWavesCompletedText()
    {
        if (waveText == null)
        {
            Debug.LogWarning("Wave text reference is not set in the inspector.");
            return;
        }

        waveText.text = "Congratulations! All waves completed! (You can quit with 'ESC')";
    }

    private void UpdateInvincibilityText(bool isInvincible)
    {
        if (invincibilityText == null)
        {
            Debug.LogWarning("Invincibility text reference is not set in the inspector.");
            return;
        }

        if (isInvincible)
            invincibilityText.text = "Invincibility: On (Toggle with 'Q')";
        else
            invincibilityText.text = "Invincibility: Off (Toggle with 'Q')";
    }
}