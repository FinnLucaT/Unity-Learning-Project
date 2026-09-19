using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Öffentliche Felder / Properties
    public float healthMax = 100f;

    // Events
    public event Action<GameObject> EventOnDeath;

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

        healthMax -= incomingDamage;

        if (healthMax <= 0)
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