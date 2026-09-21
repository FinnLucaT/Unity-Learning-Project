using UnityEngine;

public class LaserProjectile : MonoBehaviour
{
    [SerializeField] private float speed = 15f;

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
        Health health = other.GetComponentInParent<Health>();
        if (health != null && !other.CompareTag(ignoredTag))
        {
            health.TakeDamage(damage);
            Destroy(gameObject);
        }
    }
}
