using System;
using UnityEngine;

public class Health : MonoBehaviour
{
    // Öffentliche Felder / Properties
    public float health = 100f;

    // Events
    public event Action EventOnDeath;

    // Inspector-Felder
    [SerializeField] private DamageNumber damageNumberPrefab;

    // Private Felder
    private bool isDead = false;


    // Unity-Methoden
    private void Awake()
    {

    }

    private void Update()
    {

    }


    // Öffentliche Methoden
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

        if (health <= 0)
        {
            isDead = true;
            OnDeath();
        }
    }


    // Private Methoden
    private void OnDeath()
    {
        EventOnDeath?.Invoke();
    }
}