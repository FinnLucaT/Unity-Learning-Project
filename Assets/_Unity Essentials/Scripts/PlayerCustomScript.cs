using Unity.Cinemachine;
using UnityEngine;

public class PlayerCustomScript : MonoBehaviour
{


    public float playerHealth = 100f;


    void OnTriggerEnter(Collider other)
    {

        if (other.CompareTag("Enemy"))
        {
            EnemyBehaviour enemyScript = other.GetComponent<EnemyBehaviour>();
            if (enemyScript == null)
                return;

            if (enemyScript.isDoingDamage)
            {
                TakeDamage(enemyScript.damageDealt);
            }

            // Spawn-Effekt
            Instantiate(enemyScript.onDeathEffect, other.transform.position, Quaternion.identity);

            // Sound abspielen
            if (enemyScript.onDeathSounds != null && enemyScript.onDeathSounds.Length > 0)
            {
                AudioClip clip = enemyScript.onDeathSounds[Random.Range(0, enemyScript.onDeathSounds.Length)];
                AudioSource.PlayClipAtPoint(clip, other.transform.position);
            }

            Destroy(other.gameObject);
        }

    }


    public void TakeDamage(float incomingDamage)
    {

        playerHealth -= incomingDamage;
        if (playerHealth <= 0)
        {
            Destroy(gameObject);
        }

    }


}
