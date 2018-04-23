using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_JumpStrength;

    private Rigidbody m_Rigidbody;
    private Collider m_Collider;
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
        Jump();
    }

    private void Move()
    {
        // Get the user movement input
        float translation = Input.GetAxisRaw("Vertical");
        float strafeTranslation = Input.GetAxisRaw("Horizontal");

        // Create a new translation vector, normalize it so the player does not
        // gain an advantage when using multiple additive input keys, multiply it
        // by the movement speed, and make it frame-independent
        Vector3 newTranslation = new Vector3(strafeTranslation, 0f, translation);
        transform.Translate(newTranslation.normalized * m_MovementSpeed * Time.deltaTime);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && IsGrounded())
        {
            m_Rigidbody.AddForce(Vector3.up * m_JumpStrength);
        }
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
