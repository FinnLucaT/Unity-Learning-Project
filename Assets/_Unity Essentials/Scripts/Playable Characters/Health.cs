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
    private Vector3 damageNumberSpawnPosition;


    // Unity-Methoden
    private void Awake()
    {
        health = healthMax;
    }

    public void TakeDamage(float incomingDamage)
    {
        if (isDead)
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
            damageNumberSpawnPosition = transform.position + Vector3.up * 0.6f;
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


    // Private Methoden
    private void OnDeath()
    {
        EventOnDeath?.Invoke(gameObject);
    }
}