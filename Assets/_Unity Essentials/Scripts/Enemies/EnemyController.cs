using UnityEngine;
using UnityEngine.Audio;

[RequireComponent(typeof(Health))]
public class EnemyController : MonoBehaviour
{
    [SerializeField] private Collider hornCollider;
    [SerializeField] private GameObject mouseBody;

    public GameObject onDeathEffect;
    public float deathEffectDuration = 1f;
    public AudioClip[] onDeathSounds;
    public AudioMixerGroup deathSoundMixerGroup;
    public bool isChasing = true;
    public float chaseSpeed = 5f;
    public bool isDoingDamage = true;
    public float damageDealtMelee = 10f;

    private Transform playerPos;
    private Health health;
    private readonly float turnSpeed = 5f;

    private void OnEnable()
    {
        health = GetComponent<Health>();

        if (null != health)
        {
            health.EventOnDeath += SpawnDeathEffect;
            health.EventOnDeath += PlayDeathSound;
            health.EventOnDeath += DestroyThisGameObject;
        }
    }

    private void OnDisable()
    {
        if (null != health)
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

        if (null != player)
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
        if (null == playerPos)
            return;

        Vector3 direction = playerPos.position - mouseBody.transform.position;
        direction.y = 0f;
        direction.Normalize();
        Quaternion directionLook = Quaternion.LookRotation(direction);

        transform.position += chaseSpeed * Time.deltaTime * direction;
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

        
        if (other.TryGetComponent<Health>(out var otherHealthController))
        {
            otherHealthController.TakeDamage(damageDealtMelee);

            if (otherHealthController.healthMax <= 0)
            {
                playerPos = null;
            }
        }
    }

    public void SpawnDeathEffect(GameObject deadObject)
    {
        if (null != onDeathEffect)
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
        if (null == onDeathSounds || onDeathSounds.Length == 0)
            return;

        AudioClip clip =
            onDeathSounds[Random.Range(0, onDeathSounds.Length)];

        GameObject audioObject = new ("DeathSound");
        audioObject.transform.position = deadObject.transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = deathSoundMixerGroup;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }
}