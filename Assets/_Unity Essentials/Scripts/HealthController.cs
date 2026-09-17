using System.Runtime.CompilerServices;
using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 100f;

    [SerializeField] private DamageNumber damageNumberPrefab;


    public void TakeDamage(float incomingDamage)
    {
        Vector3 spawnPosition = transform.position + Vector3.up * 0.6f + Vector3.right * 0.3f;

        DamageNumber damageNumber = Instantiate(damageNumberPrefab, spawnPosition, Quaternion.identity);
        damageNumber.SetText(incomingDamage);

        health -= incomingDamage;
        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        if (gameObject.tag == "Enemy")
        {
            EnemyBehaviour enemyBehaviour = GetComponent<EnemyBehaviour>();

            enemyBehaviour.SpawnDeathEffect();
            enemyBehaviour.PlayDeathSound();
            Destroy(gameObject);
        }
        else if (gameObject.tag == "Player")
        {
            Destroy(gameObject);
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }
        else
        {
            Destroy(gameObject);
        }   
    }
}