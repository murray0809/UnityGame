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

    private GameController gameController;   // ← 追加

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
        // 敗北後は新たな敵をスポーンさせない
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

        List<EnemyData> spawnList = new List<EnemyData>();

        foreach (EnemyWaveData enemy in enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                spawnList.Add(enemy.enemyData);
            }
        }

        // 出現順をシャッフルして、種類ごとに固まらないようにする
        for (int i = spawnList.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (spawnList[i], spawnList[j]) = (spawnList[j], spawnList[i]);
        }

        foreach (EnemyData enemyData in spawnList)
        {
            enemyQueue.Enqueue(enemyData);
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