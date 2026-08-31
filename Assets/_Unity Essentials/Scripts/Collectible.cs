using System.Collections;
using UnityEngine;

public class Collectible : MonoBehaviour
{
    public float rotationSpeed = 0.25f;

    [Tooltip("Effect that plays when collected")]
    public GameObject onCollectEffect;

    [SerializeField]
    private UpdateCollectibleCount uCC;

    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            StartCoroutine(Collect());
        }
    }

    IEnumerator Collect()
    {
        GameObject effectInstance = Instantiate(onCollectEffect, transform.position, transform.rotation);

        GetComponent<MeshRenderer>().enabled = false;

        if (GetComponent<BoxCollider>() != null)
            GetComponent<BoxCollider>().enabled = false;
        else if (GetComponent<SphereCollider>() != null)
            GetComponent<SphereCollider>().enabled = false;

        if (uCC != null)
        {
            uCC.SubtractCollectible();
        }

        yield return new WaitForSeconds(2f);

        Destroy(effectInstance);
        Destroy(gameObject);
    }
}
