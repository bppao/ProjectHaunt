using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFacingBillboard : MonoBehaviour
{
    [SerializeField] private Camera m_Camera;

    private void Update()
    {
        // Calculate the camera's target rotation looking down its forward vector and
        // set the new rotation which results in a billboard effect
        Quaternion targetRotation = Quaternion.LookRotation(m_Camera.transform.forward);
        transform.rotation = targetRotation;
    }
}
