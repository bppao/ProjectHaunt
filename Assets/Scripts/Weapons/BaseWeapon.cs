using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseWeapon : MonoBehaviour
{
    [SerializeField] private float m_Damage;
    public float Damage { get { return m_Damage; } }

    protected const string BASE_ATTACK_ANIM_TRIGGER = "BaseAttack";
    protected Animator Animator;
    private bool m_IsAttacking;

    protected virtual void Start()
    {
        Animator = GetComponent<Animator>();
        m_IsAttacking = false;
    }

    // NOTE: This should only be called as an Animation Event (needs to be public)!
    // Animation Events cannot pass booleans as parameters, so use int instead.
    public void SetIsAttacking(int isAttacking)
    {
        m_IsAttacking = isAttacking > 0;
    }

    public virtual void PerformAttack()
    {
        // Don't allow multiple attacks to queue up if the player is spamming the attack button
        if (m_IsAttacking) return;

        Animator.SetTrigger(BASE_ATTACK_ANIM_TRIGGER);
    }

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
