using Unity.Cinemachine;
using UnityEngine;

public class PlayerCustomScript : MonoBehaviour
{
    public float playerHealth = 100f;

    public void TakeDamage(float incomingDamage)
    {
        playerHealth -= incomingDamage;

        if (playerHealth <= 0)
            Destroy(gameObject);
    }
}
