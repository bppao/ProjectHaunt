using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_SmoothSpeed;
    [SerializeField] private Vector3 m_Offset;

    // Use this for initialization
    private void Start()
    {
    }

    // Use LateUpdate() because it is called after the normal Update() loop
    // where the player's transform is being updated
    private void LateUpdate()
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
