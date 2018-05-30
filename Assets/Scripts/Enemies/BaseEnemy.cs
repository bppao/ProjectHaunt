using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_FadeOutTime;
    [SerializeField] private float m_MinAttackDistance;
    [SerializeField] private float m_TimeBetweenAttacks;
    [SerializeField] private float m_Damage;

    private const string BASE_ENEMY_DEATH_ANIM_TRIGGER = "BaseEnemyDeath";
    private const float TIME_TO_CHECK_CAN_ATTACK = 0.1f;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshRenderer m_MeshRenderer;
    private Color m_CurrentColor;
    private BaseCharacter m_Player;
    private NavMeshAgent m_NavMeshAgent;

    private float m_CurrentHealth;
    private bool m_IsAlive { get { return m_CurrentHealth > 0f; } }
    private bool m_FadingOut;
    private float m_AttackTimer;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_CurrentColor = m_MeshRenderer.material.color;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        m_CurrentHealth = m_MaxHealth;
        m_FadingOut = false;
        m_AttackTimer = 0f;

        StartCoroutine(CheckCanAttack());
    }

    private void Update()
    {
        if (m_IsAlive && m_Player.IsAlive)
        {
            // Head towards the player if both the enemy and player are alive
            m_NavMeshAgent.SetDestination(m_Player.transform.position);
        }
        else
        {
            // Otherwise, disable the nav mesh agent
            m_NavMeshAgent.enabled = false;
        }

        // Update the attack timer
        m_AttackTimer += Time.deltaTime;

        if (m_FadingOut)
        {
            // Fade out the enemy based on the fade out time
            m_CurrentColor.a -= Time.deltaTime / m_FadeOutTime;
            m_MeshRenderer.material.color = m_CurrentColor;

            if (m_CurrentColor.a <= 0f)
            {
                // Destroy the enemy object when fully faded out
                Destroy(gameObject);
            }
        }
    }

    // Coroutine used to check if the enemy can attack the player every
    // TIME_TO_CHECK_CAN_ATTACK interval rather than every frame
    private IEnumerator CheckCanAttack()
    {
        // This will run infinitely for the lifetime of the enemy object
        while (true)
        {
            if (CanAttack())
            {
                m_Player.TakeDamage(m_Damage);
                m_AttackTimer = 0f;
            }

            yield return new WaitForSeconds(TIME_TO_CHECK_CAN_ATTACK);
        }
    }

    private bool CanAttack()
    {
        // The enemy can attack the player if both are alive, if enough time has elapsed since
        // the last time it attacked, and if it's in range
        return m_IsAlive && m_Player.IsAlive &&
            m_AttackTimer > m_TimeBetweenAttacks &&
            Vector3.Distance(transform.position, m_Player.transform.position) < m_MinAttackDistance;
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= damageAmount;
        Debug.Log(name + "'s CurrentHealth: " + m_CurrentHealth);

        if (!m_IsAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        // Ensure the dead enemy can no longer collide with the player and ignore physics
        m_Collider.isTrigger = true;
        m_Rigidbody.isKinematic = true;

        // Play the enemy's death animation
        m_Animator.SetTrigger(BASE_ENEMY_DEATH_ANIM_TRIGGER);
    }

    // NOTE: This should only be called as an Animation Event (needs to be public)!
    public void StartFadingOutDelay(float delay)
    {
        Invoke("StartFadingOut", delay);
    }

    private void StartFadingOut()
    {
        m_FadingOut = true;
    }
}
