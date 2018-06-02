using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] private float m_Damage;

    protected const string BASE_ATTACK_ANIM_TRIGGER = "BaseAttack";
    protected Animator Animator;
    protected bool IsAttacking { get; private set; }

    private void Start()
    {
        Animator = GetComponent<Animator>();
        IsAttacking = false;
    }

    // NOTE: This should only be called as an Animation Event (needs to be public)!
    // Animation Events cannot pass booleans as parameters, so use int instead.
    public void SetIsAttacking(int isAttacking)
    {
        IsAttacking = isAttacking > 0;
    }

    public abstract void PerformAttack();
    public abstract void PerformSpecialAbility();

    private void OnTriggerEnter(Collider otherCollider)
    {
        // Prevent hitting the player with their own weapon
        BaseCharacter player = otherCollider.GetComponent<BaseCharacter>();
        if (player != null) return;

        Debug.Log("Hit: " + otherCollider.name);

        // Only take damage if an enemy was hit and is still alive
        BaseEnemy enemy = otherCollider.GetComponent<BaseEnemy>();
        if (enemy != null && enemy.IsAlive)
        {
            enemy.TakeDamage(m_Damage);
        }
    }
}
