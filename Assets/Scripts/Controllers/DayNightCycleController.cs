using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DayNightCycleController : MonoBehaviour
{
    [Tooltip("Increase this value for shorter in-game days; 60 => 24 minute in-game days")]
    [SerializeField] private float m_TimeScale = 60f;

    private GameController m_GameController;
    private float m_StartRotationX;

    private void Start()
    {
        m_GameController = FindObjectOfType<GameController>();
        m_StartRotationX = transform.rotation.x;
    }

    private void Update()
    {
        float angleThisFrame = Time.deltaTime / 360f * m_TimeScale;
        transform.RotateAround(transform.position, Vector3.forward, angleThisFrame);

        // TODO: Finish day count implementation
        if (Mathf.Approximately(m_StartRotationX, transform.rotation.x))
        {
            m_GameController.IncrementDayCount();
            Debug.Log("Day count: " + m_GameController.DayCount);
        }
    }
}
