using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseRangedWeapon : BaseWeapon
{
    [SerializeField] private BaseProjectile m_ProjectileToSpawn;

    public Transform ProjectileSpawnPoint { get; private set; }

    protected override void Start()
    {
        base.Start();

        ProjectileSpawnPoint = transform.Find("ProjectileSpawnPoint");
    }

    public override void PerformSpecialAbility()
    {
        throw new NotImplementedException();
    }

    // NOTE: This should only be called as an Animation Event (needs to be public)!
    public void SpawnProjectile()
    {
        Instantiate(m_ProjectileToSpawn, ProjectileSpawnPoint.position, Quaternion.identity);
    }
}
