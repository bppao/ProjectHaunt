using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseProjectile : MonoBehaviour
{
    [SerializeField] protected float Speed;
    [SerializeField] private float m_MaxRange;

    private Vector3 m_StartPosition;
    protected Rigidbody Rigidbody;
    protected BaseRangedWeapon RangedWeapon;

    private void Start()
    {
        m_StartPosition = transform.position;
        Rigidbody = GetComponent<Rigidbody>();
        RangedWeapon =
            (BaseRangedWeapon)GameObject.FindGameObjectWithTag("Player").GetComponent<WeaponController>().EquippedWeapon;

        ShootForward();
    }

    private void Update()
    {
        // Check if the projectile has exceeded its max range and if so, destroy it
        if (Vector3.Distance(m_StartPosition, transform.position) > m_MaxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ignore collision with the camera raycast plane
        if (other.CompareTag("CameraRaycastPlane")) return;

        // Damage the enemy if we hit one
        BaseEnemy enemy = other.GetComponent<BaseEnemy>();
        if (enemy != null)
        {
            // Prevent damaging dead enemies and don't destroy the projectile
            // if it hit a dead enemy
            if (!enemy.IsAlive) return;

            enemy.TakeDamage(RangedWeapon.Damage);
        }

        // Destroy the projectile once it has hit something
        Destroy(gameObject);
    }

    protected virtual void ShootForward()
    {
        // When the projectile is spawned, add a force in the direction of the projectile
        // spawn point's forward vector. The base implementation of ShootForward() will use
        // the forward vector's direction. If the ranged weapon is rotated in any way during
        // its attack animation, then override this method and use the appropriate vector!
        Rigidbody.AddForce(RangedWeapon.ProjectileSpawnPoint.forward * Speed);
    }
}
