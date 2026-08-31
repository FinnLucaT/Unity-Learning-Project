using System.Collections;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{


    [Tooltip("The Enemy that will be spawned.")]
    public GameObject enemyType;

    [Tooltip("The rate at which enemies spawn (per second).")]
    public float spawnRate = 1f;
    public bool isActive = true;
    public bool enemyDoesDamage = true;
    public bool enableCustomDamage = false;
    public float enemyCustomDamage = 10f;

    private float spawnTimer = 0f;


    void Update()
    {

        if (isActive)
            SpawnEnemy();

    }


    private void SpawnEnemy()
    {

        if (enemyType == null)
            return;

        float spawnInterval = 1f / spawnRate;
        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {

            GameObject enemyInstance = Instantiate(enemyType, transform.position, transform.rotation);
            if (enemyDoesDamage)
            {

                enemyInstance.GetComponent<EnemyBehaviour>().isDoingDamage = true;

                if (enableCustomDamage)
                    enemyInstance.GetComponent<EnemyBehaviour>().damageDealt = enemyCustomDamage;

            }
            else
                enemyInstance.GetComponent<EnemyBehaviour>().isDoingDamage = false;

            spawnTimer = 0f;

        }

    }

    
}
