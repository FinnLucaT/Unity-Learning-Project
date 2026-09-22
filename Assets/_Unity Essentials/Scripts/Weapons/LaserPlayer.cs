using UnityEngine;
using UnityEngine.InputSystem;

public class LaserPlayer : LaserWeapon
{
    [SerializeField] private float shootingInterval = 0.1f;

    private float nextShotTime;

    private void Update()
    {
        if (Mouse.current.leftButton.wasPressedThisFrame && Time.time >= nextShotTime)
        {
            Shoot();
            nextShotTime = Time.time + shootingInterval;
        }
    }
}