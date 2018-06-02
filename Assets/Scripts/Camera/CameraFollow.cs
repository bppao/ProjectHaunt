using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_SmoothSpeed;
    [SerializeField] private float m_RotationSpeed;
    [SerializeField] private Vector3 m_Offset;

    private const int MAX_RAY_LENGTH = 100;

    public int EnvironmentMask { get; private set; }
    private Camera m_MainCamera;

    // Use this for initialization
    private void Start()
    {
        EnvironmentMask = LayerMask.GetMask("Environment");
        m_MainCamera = Camera.main;

        // Keep the cursor locked to the center of the screen
        Cursor.lockState = CursorLockMode.Locked;
    }

    // LateUpdate() runs right after Update(), so this ensures that all of the
    // player's movement has been calculated before the camera's movement is calculated
    private void LateUpdate()
    {
        Rotate();
        PlayerLookAtMouse();
        FollowPlayer();
    }

    private void Rotate()
    {
        // Get the user's mouse input, multiply it by the rotation speed, and
        // make it frame-independent
        float mouseInputX = Input.GetAxis("Mouse X") * m_RotationSpeed * Time.deltaTime;
        float mouseInputY = Input.GetAxis("Mouse Y") * m_RotationSpeed * Time.deltaTime;
        transform.Rotate(-mouseInputY, mouseInputX, 0f);
    }

    private void PlayerLookAtMouse()
    {
        // Create a ray based on the current mouse position
        Ray mouseRay = m_MainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, MAX_RAY_LENGTH, EnvironmentMask))
        {
            Debug.DrawLine(mouseRay.origin, mouseRay.direction * 100, Color.red);
            m_Target.LookAt(hit.point);

            // Manually constrain the X and Z axes
            m_Target.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
        }
    }

    private void FollowPlayer()
    {
        // Calculate the camera's new rotation looking down the target's forward vector
        Quaternion newRotation = Quaternion.LookRotation(m_Target.forward);
        transform.rotation = newRotation;

        // Calculate the camera's new position
        Vector3 newCameraPos = m_Target.position +
            (newRotation * Vector3.forward * m_Offset.z + new Vector3(0f, m_Offset.y, 0f));

        // Linearly interpolate between the camera's current position and its new
        // position, smooth it out, and make it frame-independent
        transform.position = Vector3.Lerp(
            transform.position, newCameraPos, m_SmoothSpeed * Time.deltaTime);
    }
}
