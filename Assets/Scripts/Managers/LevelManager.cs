using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] private float m_AutoLoadNextLevelAfter;

    private void Start()
    {
        if (m_AutoLoadNextLevelAfter <= 0f)
        {
            Debug.Log("Level auto load disabled");
        }
        else
        {
            Invoke("LoadNextLevel", m_AutoLoadNextLevelAfter);
        }
    }

    public void LoadLevel(string levelName)
    {
        Debug.Log("Level load requested for: " + levelName);
        SceneManager.LoadScene(levelName);
    }

    public void LoadNextLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void QuitRequest()
    {
        Debug.Log("Quit requested");
		
		// NOTE: This does not quit out of the editor in play-mode, so it will
        // appear to do nothing. But this does work in the standalone exe.
        Application.Quit();
    }
}
