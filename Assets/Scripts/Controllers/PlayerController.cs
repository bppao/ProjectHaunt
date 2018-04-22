using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
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
}
