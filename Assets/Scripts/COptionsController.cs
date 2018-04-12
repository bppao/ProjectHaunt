using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class COptionsController : MonoBehaviour
{
    public Slider VolumeSlider, DifficultySlider;

    private CLevelManager m_LevelManager;
    private CMusicManager m_MusicManager;

    private void Start()
    {
        m_LevelManager = FindObjectOfType<CLevelManager>();
        m_MusicManager = FindObjectOfType<CMusicManager>();

        if (CPlayerPrefsManager.HasAnyPreferences())
        {
            VolumeSlider.value = CPlayerPrefsManager.GetMasterVolume();
            DifficultySlider.value = CPlayerPrefsManager.GetDifficulty();
        }
        else
        {
            SetDefaults();
        }
    }

    private void Update()
    {
        if (m_MusicManager)
        {
            m_MusicManager.SetVolume(VolumeSlider.value);
        }
    }

    public void SaveAndExit()
    {
        CPlayerPrefsManager.SetMasterVolume(VolumeSlider.value);
        CPlayerPrefsManager.SetDifficulty(DifficultySlider.value);
        m_LevelManager.LoadLevel("01a Start");
    }

    public void SetDefaults()
    {
        VolumeSlider.value = 1f;
        DifficultySlider.value = 1f;
    }
}
