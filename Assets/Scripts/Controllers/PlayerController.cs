using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_GravityStrength;
    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_FallMultiplier;
    [SerializeField] private float m_LowJumpMultiplier;
    [SerializeField] private bool m_DrawGroundCheckGizmo;

    private const float GROUND_CHECK_TOLERANCE = 0.3f;
    private const float CENTER = 0.5f;
    private const float CAPSULE_CAST_DISTANCE = 0.15f;

    private CharacterController m_CharacterController;
    public Camera MainCamera { get; private set; }
    private Vector3 m_NewTranslation;
    private float m_VerticalVelocity;
    private bool m_JumpButtonPressed;
    private int m_EnemyMask;

    // Use this for initialization
    protected virtual void Start()
    {
        m_CharacterController = GetComponent<CharacterController>();
        MainCamera = Camera.main;
        m_EnemyMask = LayerMask.GetMask("Enemy");
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    // LateUpdate is called right after Update
    private void LateUpdate()
    {
        // Check if the player is colliding with an enemy this frame. If so, move the player
        // slightly in the direction of the hit's normal. This mitigates an issue where
        // CharacterControllers "step over" kinematic rigidbodies when they have some momentum.
        // It's important that we do this check after the CharacterController's movement from the
        // player's input is finished, which is why we check in LateUpdate().
        CheckCollisions();
    }

    private void FixedUpdate()
    {
        Jump();
    }

    private void Move()
    {
        // Get the user movement input
        float translation = Input.GetAxisRaw("Vertical");
        float strafeTranslation = Input.GetAxisRaw("Horizontal");

        // Set the new translation vector, normalize it so the player does not
        // gain an advantage when using multiple additive input keys, multiply it
        // by the movement speed, and make it frame-independent
        m_NewTranslation.Set(strafeTranslation, 0f, translation);
        m_NewTranslation = MainCamera.transform.TransformDirection(m_NewTranslation);
        m_NewTranslation = m_NewTranslation.normalized * m_MovementSpeed * Time.deltaTime;

        // Did the user hit the jump button and are we grounded?
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_JumpButtonPressed = true;
        }

        AdjustGravity();

        m_CharacterController.Move(m_NewTranslation);
    }

    private void AdjustGravity()
    {
        float adjustedGravity = m_GravityStrength;

        if (m_CharacterController.velocity.y < 0f)
        {
            // Falling
            adjustedGravity *= m_FallMultiplier;
        }
        else if (m_CharacterController.velocity.y > 0f && !Input.GetButton("Jump"))
        {
            // Just tapping the jump button
            adjustedGravity *= m_LowJumpMultiplier;
        }

        // Apply gravity
        m_VerticalVelocity = m_VerticalVelocity - adjustedGravity * Time.deltaTime;
        m_NewTranslation.y = m_VerticalVelocity * Time.deltaTime;
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            // If we're grounded, zero out the vertical velocity
            m_VerticalVelocity = 0f;
        }

        if (m_JumpButtonPressed)
        {
            m_VerticalVelocity += m_JumpStrength;
            m_JumpButtonPressed = false;
        }
    }

    // NOTE: Unity's CharacterController class has an "isGrounded" property, however
    // testing has found it to be unreliable, so implement our own ground check code
    private bool IsGrounded()
    {
        // Create a sphere at the bottom of our player to check for collisions
        Collider[] collidedObjects = Physics.OverlapSphere(
            m_CharacterController.transform.position + Vector3.down * (m_CharacterController.height * CENTER - GROUND_CHECK_TOLERANCE),
            m_CharacterController.radius,
            MainCamera.GetComponent<CameraFollow>().EnvironmentMask);

        // If the array is not empty, then the player is colliding with the ground
        return collidedObjects.Length > 0;
    }

    private void CheckCollisions()
    {
        // Calculate the bottom and top points of the CharacterController for the capsule cast
        Vector3 characterBottom =
            m_CharacterController.transform.position + Vector3.down * (m_CharacterController.height * CENTER);
        Vector3 characterTop = characterBottom + Vector3.up * m_CharacterController.height;

        // Cast a capsule in the direction of the CharacterController's current movement to check
        // if we hit an enemy this frame
        RaycastHit hit;
        if (Physics.CapsuleCast(
            characterBottom,
            characterTop,
            m_CharacterController.radius,
            m_NewTranslation,
            out hit,
            CAPSULE_CAST_DISTANCE,
            m_EnemyMask))
        {
            // We hit an enemy. Move the player slightly in the direction of the hit's normal
            // (i.e., the opposite direction).
            m_CharacterController.Move(hit.normal * (CAPSULE_CAST_DISTANCE - hit.distance));
        }
    }

    private void OnDrawGizmos()
    {
        if (!m_DrawGroundCheckGizmo) return;

        // Used to visualize the ground check sphere
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(
            m_CharacterController.transform.position + Vector3.down * (m_CharacterController.height * CENTER - GROUND_CHECK_TOLERANCE),
            m_CharacterController.radius);
    }
}
