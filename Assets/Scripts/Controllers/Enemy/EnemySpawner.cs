using UnityEngine;
using System.Collections.Generic;

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

    private EnemyData currentEnemyData;

    private Queue<EnemyData> enemyQueue = new Queue<EnemyData>();

    private GameController gameController;   // Å© í«â¡

    private void Awake()
    {
        gameController = FindFirstObjectByType<GameController>();
    }

    private void OnEnable()
    {
        if (gameController != null)
        {
            gameController.OnGameOver += HandleGameOver;
        }
    }

    private void OnDisable()
    {
        if (gameController != null)
        {
            gameController.OnGameOver -= HandleGameOver;
        }
    }

    private void HandleGameOver()
    {
        // îsñkå„ÇÕêVÇΩÇ»ìGÇÉXÉ|Å[ÉìÇ≥ÇπÇ»Ç¢
        isSpawning = false;
        enemyQueue.Clear();
    }

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

    public void StartWave(params EnemyWaveData[] enemies)
    {
        enemyQueue.Clear();

        foreach (EnemyWaveData enemy in enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                enemyQueue.Enqueue(enemy.enemyData);
            }
        }

        remainingEnemies = enemyQueue.Count;

        isSpawning = true;
        spawnTimer = spawnInterval;
    }

    private void SpawnEnemy()
    {
        if (enemyQueue.Count <= 0)
        {
            isSpawning = false;
            return;
        }

        EnemyData enemyData = enemyQueue.Dequeue();

        GameObject enemyObject = Instantiate(
            enemyPrefab,
            spawnPoint.position,
            Quaternion.identity
        );

        EnemyController enemyController =
            enemyObject.GetComponent<EnemyController>();

        if (enemyController != null)
        {
            enemyController.Initialize(enemyData);
        }

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