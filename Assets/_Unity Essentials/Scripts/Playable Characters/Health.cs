using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Öffentliche Felder / Properties
    public float healthMax = 100f;

    // Events
    public event Action<GameObject> EventOnDeath;
    public event Action<float, float> EventOnHealthChanged;
    public event Action<bool> EventOnInvincibilityChanged;

    // Inspector-Felder
    [SerializeField] private DamageNumber damageNumberPrefab;

    // Private Felder
    private bool isDead = false;
    private bool isInvincible = false;
    private float health;
    private Vector3 damageNumberSpawnPosition;


    // Unity-Methoden
    private void Awake()
    {
        health = healthMax;
    }


    // Öffentliche Methoden
    public void TakeDamage(float incomingDamage)
    {
        if (isDead || isInvincible)
            return;

        if (gameObject.CompareTag("Player"))
        {
            damageNumberSpawnPosition =
                transform.position +
                Vector3.up * 0.6f +
                Vector3.right * 0.3f;
        }
        else if (gameObject.CompareTag("Enemy"))
        {
            damageNumberSpawnPosition =
                transform.position +
                Vector3.up * 2f +
                Vector3.left * 0.3f;
        }
        else
        {
            damageNumberSpawnPosition =
                transform.position +
                Vector3.up * 0.6f;
        }

        DamageNumber damageNumber = Instantiate(
            damageNumberPrefab,
            damageNumberSpawnPosition,
            Quaternion.identity
        );

        damageNumber.SetText(incomingDamage);

        health -= incomingDamage;

        EventOnHealthChanged?.Invoke(health, healthMax);

        if (health <= 0)
        {
            isDead = true;
            OnDeath();
        }
    }

    public void ToggleInvincibility()
    {
        isInvincible = !isInvincible;
        EventOnInvincibilityChanged?.Invoke(isInvincible);

        Debug.Log($"Invincibility: {isInvincible}");
    }


    // Private Methoden
    private void OnDeath()
    {
        EventOnDeath?.Invoke(gameObject);
    }
}