using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private float m_MovementSpeed;
    [SerializeField] private float m_RotationSpeed;

    private int m_EnvironmentMask;
    private int m_MaxRayLength = 100;

    // Use this for initialization
    private void Start()
    {
        m_EnvironmentMask = LayerMask.GetMask("Environment");
    }

    // Update is called once per frame
    private void Update()
    {
        Move();
        //Turn();
    }

    private void LateUpdate()
    {
        // Manually constrain the X and Z axes
        //transform.localEulerAngles = new Vector3(0f, transform.localEulerAngles.y, 0f);
    }

    private void Move()
    {
        // Get the user input, multiply the values by the appropriate speeds, and
        // make movement frame-independent
        float translation = Input.GetAxis("Vertical") * m_MovementSpeed * Time.deltaTime;
        float rotation = Input.GetAxis("Horizontal") * m_RotationSpeed * Time.deltaTime;

        transform.Translate(0f, 0f, translation);
        transform.Rotate(0f, rotation, 0f);
    }

    private void Turn()
    {
        Ray mouseRay = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(mouseRay, out hit, m_MaxRayLength, m_EnvironmentMask))
        {
            Debug.DrawLine(mouseRay.origin, hit.point, Color.red);
            transform.LookAt(hit.point);
        }
    }
}
