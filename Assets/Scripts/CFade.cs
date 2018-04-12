using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CFade : MonoBehaviour
{
    public float FadeInTime;

    private Image m_FadePanel;
    private Color m_CurrentColor = Color.black;

    private void Start()
    {
        m_FadePanel = GetComponent<Image>();
    }

    private void Update()
    {
        if (Time.timeSinceLevelLoad < FadeInTime)
        {
            // Fade the panel in
            float alphaChange = Time.deltaTime / FadeInTime;
            m_CurrentColor.a -= alphaChange;
            m_FadePanel.color = m_CurrentColor;
        }
        else
        {
            gameObject.SetActive(false);
        }
    }
}
