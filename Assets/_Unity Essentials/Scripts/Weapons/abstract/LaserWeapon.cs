using UnityEngine;

public abstract class LaserWeapon : MonoBehaviour
{
    [SerializeField] protected GameObject laserPrefab;
    [SerializeField] protected Transform firePoint;
    [SerializeField] protected AudioSource audioSource;
    [SerializeField] protected float damageRanged = 10f;
    [SerializeField] protected float range = 20f;


    protected virtual void Shoot()
    {
        Vector3 rotation = new Vector3(0f, firePoint.rotation.eulerAngles.y, firePoint.rotation.eulerAngles.z); // Reset the x rotation of the firePoint to 0
        
        GameObject laser = Instantiate(
            laserPrefab,
            firePoint.position,
            Quaternion.Euler(rotation)
        );
        laser.GetComponent<LaserProjectile>().Initialize(damageRanged, range, gameObject.tag);
        if (audioSource != null)
            audioSource.Play();
    }
}