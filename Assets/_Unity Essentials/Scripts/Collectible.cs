using System.Collections;
using UnityEditor.UI;
using UnityEngine;

public class Collectible : MonoBehaviour
{

    public float rotationSpeed = 0.25f;

    [Tooltip("Effect that plays when collected")]
    public GameObject onCollectEffect;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, rotationSpeed, 0);
    }

    private void OnTriggerEnter(Collider other)
    {
        StartCoroutine(Collect(other));
    }

    IEnumerator Collect(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameObject effectInstance = Instantiate(onCollectEffect, transform.position, transform.rotation);

            GetComponent<MeshRenderer>().enabled = false;
            GetComponent<BoxCollider>().enabled = false;

            yield return new WaitForSeconds(2f);

            Destroy(effectInstance);
            Destroy(gameObject);
        }
    }
}
