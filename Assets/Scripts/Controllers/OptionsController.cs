using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsController : MonoBehaviour
{
    public Slider VolumeSlider, DifficultySlider;

    private LevelManager m_LevelManager;
    private MusicManager m_MusicManager;

    private void Start()
    {
        m_LevelManager = FindObjectOfType<LevelManager>();
        m_MusicManager = FindObjectOfType<MusicManager>();

        if (PlayerPrefsManager.HasAnyPreferences())
        {
            VolumeSlider.value = PlayerPrefsManager.GetMasterVolume();
            DifficultySlider.value = PlayerPrefsManager.GetDifficulty();
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
        PlayerPrefsManager.SetMasterVolume(VolumeSlider.value);
        PlayerPrefsManager.SetDifficulty(DifficultySlider.value);
        m_LevelManager.LoadLevel("01a Start");
    }

    public void SetDefaults()
    {
        VolumeSlider.value = 1f;
        DifficultySlider.value = 1f;
    }
}
