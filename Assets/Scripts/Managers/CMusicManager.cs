using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CMusicManager : MonoBehaviour
{
    public AudioClip[] LevelMusicChangeArray;

    private AudioSource m_AudioSource;

    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        m_AudioSource = GetComponent<AudioSource>();

        if (CPlayerPrefsManager.HasAnyPreferences())
        {
            m_AudioSource.volume = CPlayerPrefsManager.GetMasterVolume();
        }

        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDestroy()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene newScene, LoadSceneMode sceneMode)
    {
        AudioClip loadedSceneClip = LevelMusicChangeArray[newScene.buildIndex];        

        // Don't play the same clip from the beginning
        if (m_AudioSource.clip != null &&
            m_AudioSource.clip == loadedSceneClip) return;

        if (loadedSceneClip)
        {
            m_AudioSource.clip = loadedSceneClip;
            m_AudioSource.loop = true;
            m_AudioSource.Play();
        }

        Debug.Log("Playing clip: " + loadedSceneClip);
    }

    public void SetVolume(float newVolume)
    {
        m_AudioSource.volume = newVolume;
    }
}
