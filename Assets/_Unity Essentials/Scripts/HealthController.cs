using UnityEngine;

public class HealthController : MonoBehaviour
{
    public float health = 100f;

    public void TakeDamage(float incomingDamage)
    {
        health -= incomingDamage;

        Debug.Log(gameObject.name + " took " + incomingDamage + " damage and has " + health + " health left!");

        

        if (health <= 0)
        {
            OnDeath();
        }
    }

    private void OnDeath()
    {
        Destroy(gameObject);
    }
}