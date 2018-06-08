using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameController : MonoBehaviour
{
    [Header("Enemy Spawner Variables")]
    [SerializeField] private int m_TotalNumEnemiesSpawned = 5;
    [SerializeField] [Tooltip("In seconds")] private int m_SpawnWindowLength = 60;

    private GameController m_Instance;
    private EnemySpawner[] m_EnemySpawners;

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
        m_EnemySpawners = FindObjectsOfType<EnemySpawner>();
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
}
