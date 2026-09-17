using UnityEngine;
using UnityEngine.Audio;

public class EnemyBehaviour : MonoBehaviour
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
    private float turnSpeed = 5f;

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

        HealthController otherHealthController =
            other.GetComponent<HealthController>();

        if (otherHealthController != null)
        {
            otherHealthController.TakeDamage(damageDealt);

            if (otherHealthController.health <= 0)
            {
                playerPos = null;
            }
        }
    }

    public void SpawnDeathEffect()
    {
        if (onDeathEffect != null)
        {
            GameObject effect = Instantiate(
                onDeathEffect,
                transform.position,
                Quaternion.identity
            );

            Destroy(effect, deathEffectDuration);
        }
    }

    public void PlayDeathSound()
    {
        if (onDeathSounds == null || onDeathSounds.Length == 0)
            return;

        AudioClip clip =
            onDeathSounds[Random.Range(0, onDeathSounds.Length)];

        GameObject audioObject = new GameObject("DeathSound");
        audioObject.transform.position = transform.position;

        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = deathSoundMixerGroup;
        audioSource.Play();

        Destroy(audioObject, clip.length);
    }
}