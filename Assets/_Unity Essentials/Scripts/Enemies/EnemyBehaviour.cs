using UnityEngine;

public class EnemyBehaviour : MonoBehaviour
{


    public GameObject onDeathEffect;
    public AudioClip[] onDeathSounds;
    public bool isChasing = true;
    public bool isDoingDamage = true;
    public float chaseSpeed = 5f;
    public float damageDealt = 10f;

    private Transform targetPos;


    void Start()
    {
        targetPos = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
    }


    void Update()
    {
        if (isChasing)
        {
            ChaseTarget();
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


}
