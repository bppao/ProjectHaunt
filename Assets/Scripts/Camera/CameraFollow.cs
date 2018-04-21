using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Transform m_Target;
    [SerializeField] private float m_Distance;
    [SerializeField] private float m_Height;
    [SerializeField] private float m_SmoothLag;
    [SerializeField] private float m_MaxFollowSpeed;
    [SerializeField] private float m_ClampHeadPosition;

    private Vector3 m_HeadOffset = Vector3.zero;
    private Vector3 m_CenterOffset = Vector3.zero;
    private Vector3 m_Velocity = Vector3.zero;

    // Use this for initialization
    private void Start()
    {
    }

    // Update is called once per frame
    private void Update()
    {
    }
}
