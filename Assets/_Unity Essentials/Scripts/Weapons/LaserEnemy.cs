using System.Collections;
using UnityEngine;

public class LaserEnemy : LaserWeapon
{
    [SerializeField] private float shootingInterval = 1f; // Time between shots in seconds

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        StartCoroutine(ShootCoroutine());
    }

    private IEnumerator ShootCoroutine()
    {
        while (true)
        {
            Shoot();
            yield return new WaitForSeconds(shootingInterval);
        }
    }
}
