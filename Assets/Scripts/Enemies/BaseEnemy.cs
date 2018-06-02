using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_FadeOutTime;
    [SerializeField] private float m_MinAttackDistance;
    [SerializeField] private float m_TimeBetweenAttacks;
    [SerializeField] private float m_Damage;
    [SerializeField] private Canvas m_HealthBarCanvas;
    [SerializeField] private Image m_HealthBarBackground;
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private float m_TimeUntilHealthBarFadesOut;
    [SerializeField] private float m_HealthBarFadeOutTime;

    private const string BASE_ENEMY_DEATH_ANIM_TRIGGER = "BaseEnemyDeath";
    private const float TIME_TO_CHECK_CAN_ATTACK = 0.1f;

    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshRenderer m_MeshRenderer;
    private Color m_CurrentEnemyColor;
    private Color m_HealthBarBackgroundColor;
    private Color m_HealthBarColor;
    private BaseCharacter m_Player;
    private NavMeshAgent m_NavMeshAgent;

    private float m_CurrentHealth;
    public bool IsAlive { get { return m_CurrentHealth > 0f; } }
    private bool m_FadingOut;
    private float m_AttackTimer, m_HealthBarFadeTimer;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_MeshRenderer = GetComponentInChildren<MeshRenderer>();
        m_CurrentEnemyColor = m_MeshRenderer.material.color;
        m_Player = GameObject.FindGameObjectWithTag("Player").GetComponent<BaseCharacter>();
        m_NavMeshAgent = GetComponent<NavMeshAgent>();

        // Start with the health bar disabled
        m_HealthBarCanvas.enabled = false;

        m_HealthBarColor = m_HealthBar.color;
        m_HealthBarBackgroundColor = m_HealthBarBackground.color;

        m_CurrentHealth = m_MaxHealth;
        m_FadingOut = false;
        m_AttackTimer = 0f;
        m_HealthBarFadeTimer = 0f;

        StartCoroutine(CheckCanAttack());
    }

    private void Update()
    {
        if (IsAlive && m_Player.IsAlive)
        {
            // Head towards the player if both the enemy and player are alive
            m_NavMeshAgent.SetDestination(m_Player.transform.position);
        }
        else
        {
            // Otherwise, disable the nav mesh agent
            m_NavMeshAgent.enabled = false;
        }

        // Update timers
        m_AttackTimer += Time.deltaTime;
        m_HealthBarFadeTimer += Time.deltaTime;

        FadeOutHealthBar();

        FadeOutEnemy();
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
        return IsAlive && m_Player.IsAlive &&
            m_AttackTimer > m_TimeBetweenAttacks &&
            Vector3.Distance(transform.position, m_Player.transform.position) < m_MinAttackDistance;
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= damageAmount;

        ResetHealthBar();
        
        // Update the enemy's health bar
        m_HealthBar.fillAmount = m_CurrentHealth / m_MaxHealth;

        if (!IsAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        // Ensure the dead enemy can no longer collide with the player and ignore physics
        m_Collider.isTrigger = true;
        m_Rigidbody.isKinematic = true;

        // Remove the enemy from the "Enemy" layer so that the player is not checking for
        // collisions with dead enemies
        gameObject.layer = LayerMask.GetMask("Default");

        // Disable the enemy's health bar canvas
        m_HealthBarCanvas.enabled = false;

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

    private void ResetHealthBar()
    {
        // Enable the health bar canvas
        m_HealthBarCanvas.enabled = true;

        // Reset the health bar alpha
        m_HealthBarBackgroundColor.a = 1f;
        m_HealthBarColor.a = 1f;
        m_HealthBarBackground.color = m_HealthBarBackgroundColor;
        m_HealthBar.color = m_HealthBarColor;

        // Reset the fade timer
        m_HealthBarFadeTimer = 0f;
    }

    private void FadeOutHealthBar()
    {
        // Check if it's time to fade out the enemy's health bar
        if (m_HealthBarFadeTimer > m_TimeUntilHealthBarFadesOut)
        {
            // Fade out the enemy health bar based on the health bar fade out time
            float alphaChangeThisFrame = Time.deltaTime / m_HealthBarFadeOutTime;
            m_HealthBarBackgroundColor.a -= alphaChangeThisFrame;
            m_HealthBarColor.a -= alphaChangeThisFrame;

            m_HealthBarBackground.color = m_HealthBarBackgroundColor;
            m_HealthBar.color = m_HealthBarColor;

            if (m_HealthBarBackgroundColor.a <= 0f && m_HealthBarColor.a <= 0f)
            {
                // Disable the health bar canvas when fully faded out
                m_HealthBarCanvas.enabled = false;
            }
        }
    }

    private void FadeOutEnemy()
    {
        if (m_FadingOut)
        {
            // Fade out the enemy based on the fade out time
            m_CurrentEnemyColor.a -= Time.deltaTime / m_FadeOutTime;
            m_MeshRenderer.material.color = m_CurrentEnemyColor;

            if (m_CurrentEnemyColor.a <= 0f)
            {
                // Destroy the enemy object when fully faded out
                Destroy(gameObject);
            }
        }
    }
}
