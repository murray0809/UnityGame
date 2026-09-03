using UnityEngine;

public class WaveController : MonoBehaviour
{
    private WaveModel model;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameUI gameUI;

    [SerializeField]
    private EnemyData normalEnemyData;

    [SerializeField]
    private EnemyData fastEnemyData;

    [SerializeField]
    private EnemyData tankEnemyData;

    [SerializeField]
    private EnemyData bossEnemyData;

    private void Awake()
    {
        model = new WaveModel(5);
    }

    private void Start()
    {
        StartNextWave();
    }

    private void Update()
    {
        if (enemySpawner.IsWaveFinished())
        {
            StartNextWave();
        }
    }

    private void StartNextWave()
    {
        if (model.IsFinished())
        {
            Debug.Log("Game Clear!");
            enabled = false;
            return;
        }

        model.StartNextWave();

        Debug.Log("Wave " + model.CurrentWave + " Start!");

        UpdateUI();

        if (model.CurrentWave == 1)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 5
                }
            );
        }
        else if (model.CurrentWave == 2)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 8
                }
            );
        }
        else if (model.CurrentWave == 3)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 5
                },
                new EnemyWaveData
                {
                    enemyData = fastEnemyData,
                    count = 3
                }
            );
        }
        else if (model.CurrentWave == 4)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 8
                },
                new EnemyWaveData
                {
                    enemyData = fastEnemyData,
                    count = 5
                },
                new EnemyWaveData
                {
                    enemyData = tankEnemyData,
                    count = 2
                }
            );
        }
        else if (model.CurrentWave == 5)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 10
                },
                new EnemyWaveData
                {
                    enemyData = fastEnemyData,
                    count = 5
                },
                new EnemyWaveData
                {
                    enemyData = tankEnemyData,
                    count = 3
                },
                new EnemyWaveData
                {
                    enemyData = bossEnemyData,
                    count = 1
                }
            );
        }
    }

    private void UpdateUI()
    {
        if (gameUI == null)
        {
            return;
        }

        gameUI.UpdateWave(
            model.CurrentWave,
            model.TotalWaves
        );
    }
}