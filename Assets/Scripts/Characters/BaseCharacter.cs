using UnityEngine;

public class BaseCharacter : PlayerController
{
    [SerializeField] private float m_MaxHealth;

    private const string BASE_CHARACTER_DEATH_ANIM_TRIGGER = "BaseCharacterDeath";

    private Animator m_Animator;
    private PlayerController m_PlayerController;
    private WeaponController m_WeaponController;
    private CameraFollow m_CameraFollow;

    private float m_CurrentHealth;
    public bool IsAlive { get { return m_CurrentHealth > 0f; } }

    protected override void Start()
    {
        base.Start();

        m_Animator = GetComponent<Animator>();
        m_PlayerController = GetComponent<PlayerController>();
        m_WeaponController = GetComponent<WeaponController>();
        m_CameraFollow = MainCamera.GetComponent<CameraFollow>();

        m_CurrentHealth = m_MaxHealth;
    }

    public void TakeDamage(float damageAmount)
    {
        m_CurrentHealth -= damageAmount;
        Debug.Log(name + "'s CurrentHealth: " + m_CurrentHealth);

        if (!IsAlive)
        {
            Die();
        }
    }

    private void Die()
    {
        // Disable the player controller and camera follow components
        m_PlayerController.enabled = false;
        m_CameraFollow.enabled = false;

        // Destroy the player's weapon if one is equipped
        if (m_WeaponController.IsWeaponEquipped)
        {
            Destroy(m_WeaponController.EquippedWeapon.gameObject);
        }

        // Play the character's death animation
        m_Animator.SetTrigger(BASE_CHARACTER_DEATH_ANIM_TRIGGER);
    }
}
