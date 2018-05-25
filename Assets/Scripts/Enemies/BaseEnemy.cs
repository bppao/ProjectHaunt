using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BaseEnemy : MonoBehaviour
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private float m_FadeOutTime;

    private const string BASE_ENEMY_DEATH_ANIM_TRIGGER = "BaseEnemyDeath";
    private float m_CurrentHealth;
    private Animator m_Animator;
    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private MeshRenderer m_MeshRenderer;
    private Color m_CurrentColor;
    private bool m_FadingOut;

    private void Start()
    {
        m_Animator = GetComponent<Animator>();
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
        m_MeshRenderer = GetComponent<MeshRenderer>();
        m_CurrentColor = m_MeshRenderer.material.color;

        m_CurrentHealth = m_MaxHealth;
        m_FadingOut = false;
    }

    private void Update()
    {
        if (!m_FadingOut) return;

        // Fade out the enemy based on the fade out time
        m_CurrentColor.a -= Time.deltaTime / m_FadeOutTime;
        m_MeshRenderer.material.color = m_CurrentColor;

        if (m_CurrentColor.a <= 0f)
        {
            // Destroy the enemy object when fully faded out
            Destroy(gameObject);
        }
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= damageAmount;
        Debug.Log(name + "'s CurrentHealth: " + m_CurrentHealth);

        if (m_CurrentHealth <= 0f)
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
