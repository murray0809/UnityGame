using UnityEngine;

public class EnemySpawner : MonoBehaviour
{
    [SerializeField]
    private GameObject enemyPrefab;

    [SerializeField]
    private Transform spawnPoint;

    [SerializeField]
    private float spawnInterval = 2f;

    private int remainingEnemies;
    private int activeEnemies;

    private float spawnTimer;
    private bool isSpawning;

    private void Update()
    {
        if (!isSpawning)
        {
            return;
        }

        spawnTimer += Time.deltaTime;

        if (spawnTimer >= spawnInterval)
        {
            SpawnEnemy();
            spawnTimer = 0f;
        }
    }

    public void StartWave(int enemyCount)
    {
        remainingEnemies = enemyCount;
        isSpawning = true;
        spawnTimer = spawnInterval;
    }

    private void SpawnEnemy()
    {
        Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        remainingEnemies--;
        activeEnemies++;

        if (remainingEnemies <= 0)
        {
            isSpawning = false;
        }
    }

    public void OnEnemyDefeated()
    {
        activeEnemies--;

        if (activeEnemies < 0)
        {
            activeEnemies = 0;
        }
    }

    public bool IsWaveFinished()
    {
        return !isSpawning && activeEnemies <= 0;
    }
}