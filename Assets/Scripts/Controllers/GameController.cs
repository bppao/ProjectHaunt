using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameController : MonoBehaviour
{
    [Header("Enemy Spawner Variables")]
    [SerializeField] private int m_TotalNumEnemiesSpawned = 5;
    [SerializeField] [Tooltip("In seconds")] private int m_SpawnWindowLength = 60;

    [SerializeField] private List<BaseCharacter> m_CharacterClassPrefabs;

    private GameController m_Instance;
    private EnemySpawner[] m_EnemySpawners;
    public string SelectedCharacterClass { get; private set; }
    public int DayCount { get; private set; }

    private void Awake()
    {
        if (m_Instance == null)
        {
            m_Instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    private void Start()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void Update()
    {
        if (Input.GetButtonDown("EnableEnemySpawners"))
        {
            foreach (EnemySpawner enemySpawner in m_EnemySpawners)
            {
                enemySpawner.EnableSpawner(
                    true,
                    m_TotalNumEnemiesSpawned,
                    m_SpawnWindowLength);
            }
        }

        if (Input.GetButtonDown("DisableEnemySpawners"))
        {
            foreach (EnemySpawner enemySpawner in m_EnemySpawners)
            {
                enemySpawner.EnableSpawner(false);
            }
        }
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "03 Low-Poly Game Terrain")
        {
            // Grab all of the enemy spawners
            m_EnemySpawners = FindObjectsOfType<EnemySpawner>();

            // Find the player start transform as well as the character prefab of
            // the class the user selected to play
            Transform playerStart = GameObject.Find("PlayerStart").transform;
            BaseCharacter selectedCharacter = GetSelectedCharacter();
            if (selectedCharacter == null)
            {
                Debug.LogError("Selected character is null!");
                return;
            }

            // Spawn the character at player start
            Instantiate(selectedCharacter, playerStart.position, playerStart.rotation);
        }
    }

    private BaseCharacter GetSelectedCharacter()
    {
        BaseCharacter selectedCharacter = null;
        foreach (BaseCharacter character in m_CharacterClassPrefabs)
        {
            if (character == null) continue;
            if (character.GetType().ToString() != SelectedCharacterClass) continue;
            selectedCharacter = character;
            break;
        }
        return selectedCharacter;
    }

    public void SetSelectedCharacterClass(string characterClass)
    {
        SelectedCharacterClass = characterClass;
    }

    public void IncrementDayCount()
    {
        DayCount++;
    }
}
