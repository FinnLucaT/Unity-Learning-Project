using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;

    private bool hasHit;
    private float damage;
    private float remainingRange;
    private string ignoredTag;


    public void Initialize(float damage, float range, string ignoredTag)
    {
        this.damage = damage;
        this.ignoredTag = ignoredTag;
        remainingRange = range;
    }


    private void Update()
    {
        transform.Translate(Vector3.forward * speed * Time.deltaTime);
        remainingRange -= speed * Time.deltaTime;

        if (remainingRange <= 0f)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        Health health = other.GetComponentInParent<Health>();

        if (health != null && !other.CompareTag(ignoredTag))
        {
            hasHit = true;

            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
