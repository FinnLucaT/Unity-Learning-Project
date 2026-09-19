using UnityEngine;
using UnityEngine.UI;

public class HealthBar : MonoBehaviour
{
    [SerializeField] private Health healthComponent;
    [SerializeField] private Image fillImage;

    private void OnEnable()
    {
        if (healthComponent != null)
        {
            healthComponent.EventOnHealthChanged += UpdateHealthBar;
        }
    }

    private void OnDisable()
    {
        if (healthComponent != null)
        {
            healthComponent.EventOnHealthChanged -= UpdateHealthBar;
        }
    }

    private void Awake()
    {
        fillImage.fillAmount = 1f; // Set initial fill amount to full
    }

    public void UpdateHealthBar(float currentHealth, float maxHealth)
    {
        if (fillImage != null)
        {
            fillImage.fillAmount = currentHealth / maxHealth;
        }
    }
}