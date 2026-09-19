using System;
using UnityEngine;
using UnityEngine.UI;

public class Health : MonoBehaviour
{
    // Öffentliche Felder / Properties
    public float healthMax = 100f;

    // Events
    public event Action<GameObject> EventOnDeath;
    public event Action<float, float> EventOnHealthChanged;

    // Inspector-Felder
    [SerializeField] private DamageNumber damageNumberPrefab;

    // Private Felder
    private bool isDead = false;
    private float health;


    // Unity-Methoden
    private void Awake()
    {
        health = healthMax;
    }

    public void TakeDamage(float incomingDamage)
    {
        if (isDead)
            return;

        Vector3 spawnPosition =
            transform.position +
            Vector3.up * 0.6f +
            Vector3.right * 0.3f;

        DamageNumber damageNumber = Instantiate(
            damageNumberPrefab,
            spawnPosition,
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


    // Private Methoden
    private void OnDeath()
    {
        EventOnDeath?.Invoke(gameObject);
    }
}