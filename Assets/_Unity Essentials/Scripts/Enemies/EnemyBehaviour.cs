using UnityEngine;
using UnityEngine.Audio;

public class EnemyBehaviour : MonoBehaviour
{
    public GameObject onDeathEffect;
    public float deathEffectDuration = 1f;
    public AudioClip[] onDeathSounds;
    public AudioMixerGroup deathSoundMixerGroup;
    public bool isChasing = true;
    public float chaseSpeed = 5f;
    public bool isDoingDamage = true;
    public float damageDealt = 10f;

    private Transform targetPos;
    private PlayerCustomScript playerScript;

    void Start()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        playerScript = targetPos.GetComponent<PlayerCustomScript>();
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
        if (other.CompareTag("Player"))
        {
            OnPlayerCollision();
        }
    }



    private void ChaseTarget()
    {
        if (targetPos == null)
            return;

        Vector3 direction = (targetPos.position - transform.position).normalized;
        Quaternion directionLook = Quaternion.LookRotation(direction);
        transform.position += direction * chaseSpeed * Time.deltaTime;
        transform.rotation = Quaternion.Slerp(transform.rotation, directionLook, chaseSpeed * Time.deltaTime);
    }

    private void OnPlayerCollision()
    {
        DamagePlayer();
        SpawnDeathEffect();
        PlayDeathSound();

        Destroy(gameObject);
    }

    private void DamagePlayer()
    {
        if (isDoingDamage && playerScript != null)
        {
            playerScript.TakeDamage(damageDealt);
        }
    }

    private void SpawnDeathEffect()
    {
        if (onDeathEffect != null)
        {
            GameObject effect = Instantiate(onDeathEffect, transform.position, Quaternion.identity);
            Destroy(effect, deathEffectDuration);
        }
    }

    private void PlayDeathSound()
    {
        if (onDeathSounds == null || onDeathSounds.Length == 0)
            return;

        AudioClip clip = onDeathSounds[Random.Range(0, onDeathSounds.Length)];
        
        GameObject audioObject = new GameObject("DeathSound");
        audioObject.transform.position = transform.position;
        
        AudioSource audioSource = audioObject.AddComponent<AudioSource>();
        audioSource.clip = clip;
        audioSource.outputAudioMixerGroup = deathSoundMixerGroup;
        audioSource.Play();
        
        Destroy(audioObject, clip.length);
    }
}
