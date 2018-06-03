using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter : PlayerController
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private Image m_DamageImage;
    [SerializeField] private Color m_DamageFlashColor;
    [SerializeField] private float m_DamageFlashSpeed;

    private const string BASE_CHARACTER_DEATH_ANIM_TRIGGER = "BaseCharacterDeath";

    private Animator m_Animator;
    private PlayerController m_PlayerController;
    private WeaponController m_WeaponController;
    private CameraFollow m_CameraFollow;
    private Text m_HealthBarText;
    private bool m_Damaged;

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
        m_HealthBarText = m_HealthBar.GetComponentInChildren<Text>();
        m_HealthBarText.text = m_CurrentHealth + " / " + m_MaxHealth;
    }

    protected override void Update()
    {
        base.Update();

        // Flash the damage image if we just took damage
        m_DamageImage.color = m_Damaged ? m_DamageFlashColor :
            Color.Lerp(m_DamageImage.color, Color.clear, m_DamageFlashSpeed * Time.deltaTime);

        m_Damaged = false;
    }

    public void TakeDamage(float damageAmount)
    {
        m_Damaged = true;
        m_CurrentHealth -= damageAmount;

        // Update the player's health bar
        m_HealthBar.fillAmount = m_CurrentHealth / m_MaxHealth;
        m_HealthBarText.text = m_CurrentHealth + " / " + m_MaxHealth;

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
