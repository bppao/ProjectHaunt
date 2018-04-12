using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CPlayerPrefsManager : MonoBehaviour
{
    const string MASTER_VOLUME_KEY = "master_volume";
    const string DIFFICULTY_KEY = "difficulty";
    const string LEVEL_KEY = "level_unlocked_";

    public static bool HasAnyPreferences()
    {
        return PlayerPrefs.HasKey(MASTER_VOLUME_KEY) ||
               PlayerPrefs.HasKey(DIFFICULTY_KEY) ||
               PlayerPrefs.HasKey(LEVEL_KEY);
    }

    public static void SetMasterVolume(float volume)
    {
        if (volume >= 0f && volume <= 1f)
        {
            PlayerPrefs.SetFloat(MASTER_VOLUME_KEY, volume);
        }
        else
        {
            Debug.LogError("Master volume out of range!");
        }
    }

    public static float GetMasterVolume()
    {
        return PlayerPrefs.GetFloat(MASTER_VOLUME_KEY);
    }

    public static void UnlockLevel(int level)
    {
        if (level <= SceneManager.sceneCountInBuildSettings - 1)
        {
            // Using 1 for true as PlayerPrefs doesn't handle bools
            PlayerPrefs.SetInt(LEVEL_KEY + level, 1);
        }
        else
        {
            Debug.LogError("Trying to unlock level not in build settings!");
        }
    }

    public static bool IsLevelUnlocked(int level)
    {
        // Using 1 for true as PlayerPrefs doesn't handle bools
        if (PlayerPrefs.GetInt(LEVEL_KEY + level) == 1)
        {
            return true;
        }

        if (level > SceneManager.sceneCountInBuildSettings - 1)
        {
            Debug.LogError("Asking for level unlock not in build settings!");
            return false;
        }

        return false;
    }

    public static void SetDifficulty(float difficulty)
    {
        if (difficulty >= 1f && difficulty <= 3f)
        {
            PlayerPrefs.SetFloat(DIFFICULTY_KEY, difficulty);
        }
        else
        {
            Debug.LogError("Setting invalid difficulty!");
        }
    }

    public static float GetDifficulty()
    {
        return PlayerPrefs.GetFloat(DIFFICULTY_KEY);
    }
}
