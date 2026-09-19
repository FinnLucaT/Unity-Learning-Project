using UnityEngine;
using UnityEngine.Audio;

public class EnemyController : MonoBehaviour
{
    [SerializeField] private Collider hornCollider;

    public GameObject onDeathEffect;
    public float deathEffectDuration = 1f;
    public AudioClip[] onDeathSounds;
    public AudioMixerGroup deathSoundMixerGroup;
    public bool isChasing = true;
    public float chaseSpeed = 5f;
    public bool isDoingDamage = true;
    public float damageDealt = 10f;

    private Transform playerPos;
    private Health health;
    private float turnSpeed = 5f;

    private void OnEnable()
    {
        health = GetComponent<Health>();

        if (health != null)
        {
            health.EventOnDeath += SpawnDeathEffect;
            health.EventOnDeath += PlayDeathSound;
            health.EventOnDeath += DestroyThisGameObject;
        }
    }

    private void OnDisable()
    {
        if (health != null)
        {
            health.EventOnDeath -= SpawnDeathEffect;
            health.EventOnDeath -= PlayDeathSound;
            health.EventOnDeath -= DestroyThisGameObject;
        }
    }

    private void DestroyThisGameObject(GameObject deadObject)
    {
        Destroy(deadObject);
    }

    void Start()
    {
        GameObject player = GameObject.FindGameObjectWithTag("Player");

        if (player != null)
        {
            playerPos = player.transform;
        }
    }

    void Update()
    {
        if (isChasing)
        {
            ChaseTarget();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        if (hornCollider.bounds.Intersects(other.bounds))
        {
            OnPlayerCollision(other);
        }
    }

    private void ChaseTarget()
    {
        if (playerPos == null)
            return;

        Vector3 direction = (playerPos.position - transform.position).normalized;
        Quaternion directionLook = Quaternion.LookRotation(direction);

        transform.position += direction * chaseSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(
            transform.rotation,
            directionLook,
            turnSpeed * Time.deltaTime
        );
    }

    private void OnPlayerCollision(Collider other)
    {
        DealDamageOnCollision(other);
    }

    private void DealDamageOnCollision(Collider other)
    {
        if (!isDoingDamage)
            return;

        Health otherHealthController =
            other.GetComponent<Health>();

        if (otherHealthController != null)
        {
            otherHealthController.TakeDamage(damageDealt);

            if (otherHealthController.health <= 0)
            {
                playerPos = null;
            }
        }
    }

    public void SpawnDeathEffect(GameObject deadObject)
    {
        if (onDeathEffect != null)
        {
            GameObject effect = Instantiate(
                onDeathEffect,
                deadObject.transform.position,
                Quaternion.identity
            );

            Destroy(effect, deathEffectDuration);
        }
    }

    public void PlayDeathSound(GameObject deadObject)
    {
        if (onDeathSounds == null || onDeathSounds.Length == 0)
            return;

        AudioClip clip =
            onDeathSounds[Random.Range(0, onDeathSounds.Length)];

        GameObject audioObject = new GameObject("DeathSound");
        audioObject.transform.position = deadObject.transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = deathSoundMixerGroup;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }
}