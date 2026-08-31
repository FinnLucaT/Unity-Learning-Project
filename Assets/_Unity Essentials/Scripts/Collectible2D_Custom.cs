using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Collectible2D_Custom : MonoBehaviour
{

    public float rotationSpeed = 0.5f;
    public GameObject onCollectEffect;

    [SerializeField]
    private UpdateCollectibleCount uCC;

    // Update is called once per frame
    void Update()
    {
        transform.Rotate(0, 0, rotationSpeed);
    }

    private void OnTriggerEnter2D(Collider2D other) 
    {

        if (other.CompareTag("Player"))
        {
            StartCoroutine(Collect2D());
        }

    }

    IEnumerator Collect2D()
    {
        GameObject effectInstance = Instantiate(onCollectEffect, transform.position, transform.rotation);

        GetComponent<SpriteRenderer>().enabled = false;

        if (GetComponent<BoxCollider2D>() != null)
            GetComponent<BoxCollider2D>().enabled = false;
        else if (GetComponent<CircleCollider2D>() != null)
            GetComponent<CircleCollider2D>().enabled = false;
        else if (GetComponent<PolygonCollider2D>() != null)
            GetComponent<PolygonCollider2D>().enabled = false;

        if (uCC != null)
        {
            uCC.SubtractCollectible();
        }

        yield return new WaitForSeconds(2f);

        Destroy(effectInstance);
        Destroy(gameObject);
    }


}


