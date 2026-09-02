using UnityEngine;

public class WaveController : MonoBehaviour
{
    private WaveModel model;

    [SerializeField]
    private EnemySpawner enemySpawner;

    [SerializeField]
    private GameUI gameUI;

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
            enemySpawner.StartWave(5);
        }
        else if (model.CurrentWave == 2)
        {
            enemySpawner.StartWave(8);
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