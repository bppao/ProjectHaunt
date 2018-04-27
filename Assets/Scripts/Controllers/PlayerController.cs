using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_JumpStrength;
    [SerializeField] private float m_FallMultiplier;
    [SerializeField] private float m_LowJumpMultiplier;

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
    private bool m_JumpButtonPressed;
    private const float GROUND_CHECK_TOLERANCE = 0.1f;

    // Use this for initialization
    private void Start()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Collider = GetComponent<Collider>();
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
    }

    // FixedUpdate() runs during the physics loop. Perform any physics-based code here.
    private void FixedUpdate()
    {
        Jump();
        AdjustGravity();
    }

    private void Move()
    {
        // Get the user movement input
        float translation = Input.GetAxisRaw("Vertical");
        float strafeTranslation = Input.GetAxisRaw("Horizontal");

        // Did the user hit the jump button and are we grounded?
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_JumpButtonPressed = true;
        }

        // Create a new translation vector, normalize it so the player does not
        // gain an advantage when using multiple additive input keys, multiply it
        // by the movement speed, and make it frame-independent
        Vector3 newTranslation = new Vector3(strafeTranslation, 0f, translation);
        transform.Translate(newTranslation.normalized * m_MovementSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (m_JumpButtonPressed)
        {
            m_Rigidbody.AddForce(Vector3.up * m_JumpStrength, ForceMode.Impulse);
            m_JumpButtonPressed = false;
        }
    }

    private void AdjustGravity()
    {
        Vector3 newGravity = Physics.gravity.y * Vector3.up;

        if (m_Rigidbody.velocity.y < 0f)
        {
            // Falling
            newGravity = m_FallMultiplier * newGravity;
        }
        else if (m_Rigidbody.velocity.y > 0f && !Input.GetButton("Jump"))
        {
            // Just tapping the jump button
            newGravity = m_LowJumpMultiplier * newGravity;
        }

        m_Rigidbody.AddForce(newGravity, ForceMode.Acceleration);
    }

    private bool IsGrounded()
    {
        // Cast a ray downwards from the player with length equal to the height of
        // the collider plus some tolerance
        return Physics.Raycast(
            transform.position,
            Vector3.down,
            m_Collider.bounds.extents.y + GROUND_CHECK_TOLERANCE);
    }
}
