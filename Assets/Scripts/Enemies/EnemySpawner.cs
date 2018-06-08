using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField] private BaseEnemy m_EnemyToSpawn;

    private const int DEFAULT_TOTAL_NUM_ENEMIES_SPAWNED = 5;
    private const int DEFAULT_SPAWN_WINDOW_LENGTH = 60;

    private int m_TotalNumEnemiesSpawned = DEFAULT_TOTAL_NUM_ENEMIES_SPAWNED;
    private int m_SpawnWindowLength = DEFAULT_SPAWN_WINDOW_LENGTH;
    private int m_CurrentSpawnTime;
    private Coroutine m_CheckSpawnCoroutine;
    private Camera m_MainCamera;

    /// <summary>
    /// Key: Time value based off of m_MaxSpawnTime
    /// Value: Number of enemies to spawn at the key's time value
    /// </summary>
    private Dictionary<int, int> m_SpawnEvents;

    private void Start()
    {
        m_MainCamera = Camera.main;
    }

    private IEnumerator CheckSpawn()
    {
        // This makes the coroutine wait for one second before executing
        yield return new WaitForSeconds(1f);

        // Check to see if there are any enemies to spawn at this current time value
        if (m_SpawnEvents[m_CurrentSpawnTime] > 0)
        {
            for (int i = 0; i < m_SpawnEvents[m_CurrentSpawnTime]; i++)
            {
                // Spawn enemy
                Debug.LogError("Spawn enemy at " + m_CurrentSpawnTime + " seconds!");
                BaseEnemy spawnedEnemy = Instantiate(
                    m_EnemyToSpawn,
                    transform.position,
                    Quaternion.identity,
                    transform);

                // Set the camera for the newly spawned enemy
                spawnedEnemy.GetComponentInChildren<Canvas>().worldCamera = m_MainCamera;
                spawnedEnemy.GetComponentInChildren<CameraFacingBillboard>().SetCamera(m_MainCamera);
            }
        }

        // Decrement the current time value
        Debug.Log("Current spawn time: " + m_CurrentSpawnTime + " seconds");
        m_CurrentSpawnTime--;

        if (m_CurrentSpawnTime < 0)
        {
            // If the current time value has gone negative, then the spawn window
            // is over, so stop the coroutine
            StopCoroutine(m_CheckSpawnCoroutine);
            m_CheckSpawnCoroutine = null;
        }
        else
        {
            // Otherwise, repeat the coroutine by starting it again
            m_CheckSpawnCoroutine = StartCoroutine(CheckSpawn());
        }
    }

    public void EnableSpawner(
        bool enabled,
        int newTotalNumEnemiesSpawned = DEFAULT_TOTAL_NUM_ENEMIES_SPAWNED,
        int newSpawnWindowLength = DEFAULT_SPAWN_WINDOW_LENGTH)
    {
        if (enabled)
        {
            // Prevent enabling a spawner that's already spawning
            if (m_CheckSpawnCoroutine != null) return;

            // Set the new values before resetting the spawner
            m_TotalNumEnemiesSpawned = newTotalNumEnemiesSpawned;
            m_SpawnWindowLength = newSpawnWindowLength;

            ResetSpawner();
            m_CheckSpawnCoroutine = StartCoroutine(CheckSpawn());
        }
        else
        {
            // Prevent disabling a spawner that hasn't started spawning yet
            if (m_CheckSpawnCoroutine == null) return;

            StopCoroutine(m_CheckSpawnCoroutine);
            m_CheckSpawnCoroutine = null;
        }
    }

    private void ResetSpawner()
    {
        m_CurrentSpawnTime = m_SpawnWindowLength - 1;

        // Initialize the dictionary to the size of the max spawn time and set the
        // values to zero to start
        m_SpawnEvents = new Dictionary<int, int>();
        for (int i = 0; i < m_SpawnWindowLength; i++)
        {
            m_SpawnEvents.Add(i, 0);
        }

        // Fill the dictionary with random time values at which enemies may be spawned
        for (int i = 0; i < m_TotalNumEnemiesSpawned; i++)
        {
            int randomTime = Random.Range(0, m_SpawnWindowLength);
            m_SpawnEvents[randomTime]++;
        }
    }
}
