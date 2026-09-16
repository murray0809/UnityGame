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

    private GameController gameController;   // Å© í«â¡

    private void Awake()
    {
        model = new WaveModel(5);

        gameController = FindFirstObjectByType<GameController>();   // Å© í«â¡
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
        // îsñkéûÇÕÇªÇÍà»è„WaveÇêiçsÇ≥ÇπÇ»Ç¢
        enabled = false;
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
            if (gameController != null)
            {
                gameController.GameClear();
            }

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
                    count = 8
                }
            );
        }
        else if (model.CurrentWave == 2)
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
                    count = 6
                }
            );
        }
        else if (model.CurrentWave == 3)
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
                    count = 7
                },
                new EnemyWaveData
                {
                    enemyData = tankEnemyData,
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
                    count = 14
                },
                new EnemyWaveData
                {
                    enemyData = fastEnemyData,
                    count = 9
                },
                new EnemyWaveData
                {
                    enemyData = tankEnemyData,
                    count = 5
                }
            );
        }
        else if (model.CurrentWave == 5)
        {
            enemySpawner.StartWave(
                new EnemyWaveData
                {
                    enemyData = normalEnemyData,
                    count = 16
                },
                new EnemyWaveData
                {
                    enemyData = fastEnemyData,
                    count = 10
                },
                new EnemyWaveData
                {
                    enemyData = tankEnemyData,
                    count = 7
                },
                new EnemyWaveData
                {
                    enemyData = bossEnemyData,
                    count = 2
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