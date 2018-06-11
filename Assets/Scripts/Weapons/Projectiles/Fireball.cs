using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : BaseProjectile
{
    protected override void ShootForward()
    {
        // Intentionally not calling base.ShootForward()

        // The fireball is spawned by the staff which is rotated by 90 degrees during
        // its attack animation, so need to use the projectile spawn point's up vector
        // for the direction rather than the forward vector
        Rigidbody.AddForce(RangedWeapon.ProjectileSpawnPoint.up * Speed);
    }
}
