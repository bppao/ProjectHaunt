using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class BaseCharacter : PlayerController
{
    [SerializeField] private float m_MaxHealth;
    [SerializeField] private Image m_HealthBar;
    [SerializeField] private Image m_DamageImage;
    [SerializeField] private Color m_DamageFlashColor;
    [SerializeField] private float m_DamageFlashSpeed;
    [SerializeField] private float m_RegenerateHealthAmount = 1f;
    [SerializeField] private float m_RegenerateHealthTime = 2f;

    private const string BASE_CHARACTER_DEATH_ANIM_TRIGGER = "BaseCharacterDeath";

    private Animator m_Animator;
    private PlayerController m_PlayerController;
    private WeaponController m_WeaponController;
    private CameraFollow m_CameraFollow;
    private Text m_HealthBarText;
    private bool m_Damaged;
    private Coroutine m_RegenerateHealthCoroutine;

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
        UpdateHealthBar();

        // Start the coroutine responsible for gradually regenerating health over time
        m_RegenerateHealthCoroutine = StartCoroutine(RegenerateHealth());
    }

    protected override void Update()
    {
        base.Update();

        // Flash the damage image if we just took damage
        m_DamageImage.color = m_Damaged ? m_DamageFlashColor :
            Color.Lerp(m_DamageImage.color, Color.clear, m_DamageFlashSpeed * Time.deltaTime);

        m_Damaged = false;
    }

    private IEnumerator RegenerateHealth()
    {
        yield return new WaitForSeconds(m_RegenerateHealthTime);

        // Add the health regeneration amount and ensure it does not go over the max health
        m_CurrentHealth += m_RegenerateHealthAmount;
        m_CurrentHealth = Mathf.Min(m_CurrentHealth, m_MaxHealth);

        UpdateHealthBar();

        m_RegenerateHealthCoroutine = StartCoroutine(RegenerateHealth());
    }

    public void TakeDamage(float damageAmount)
    {
        m_Damaged = true;
        m_CurrentHealth -= damageAmount;

        UpdateHealthBar();

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

        // Stop the health regeneration coroutine and update the health bar
        StopCoroutine(m_RegenerateHealthCoroutine);
        m_CurrentHealth = 0f;
        UpdateHealthBar();

        // Clear the damage image color
        m_DamageImage.color = Color.clear;

        // Play the character's death animation
        m_Animator.SetTrigger(BASE_CHARACTER_DEATH_ANIM_TRIGGER);
    }

    private void UpdateHealthBar()
    {
        // Update the player's health bar
        m_HealthBar.fillAmount = m_CurrentHealth / m_MaxHealth;
        m_HealthBarText.text = m_CurrentHealth + " / " + m_MaxHealth;
    }
}
