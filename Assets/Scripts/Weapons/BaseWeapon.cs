using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] private float m_Damage;

    protected Animator Animator;

    private void Start()
    {
        Animator = GetComponent<Animator>();
    }

    public void PerformAttack()
    {
        Debug.Log("Base weapon performed attack!");
    }
}
