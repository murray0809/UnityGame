using UnityEngine;

/// <summary>
/// Waveの進行を管理するController。
/// 各Waveの敵構成を決めてEnemySpawnerに出現を依頼し、
/// 全Waveを終えたらGameControllerにゲームクリアを通知する。
/// </summary>
public class WaveController : MonoBehaviour
{
    /// <summary>現在のWave番号と総Wave数を保持するModel。</summary>
    private WaveModel model;

    /// <summary>敵の出現を担当するスポナー。</summary>
    [SerializeField]
    private EnemySpawner enemySpawner;

    /// <summary>Wave数を表示するHUD。</summary>
    [SerializeField]
    private GameUI gameUI;

    /// <summary>通常の敵のデータ。</summary>
    [SerializeField]
    private EnemyData normalEnemyData;

    /// <summary>移動が速い敵のデータ。</summary>
    [SerializeField]
    private EnemyData fastEnemyData;

    /// <summary>HPが高い敵のデータ。</summary>
    [SerializeField]
    private EnemyData tankEnemyData;

    /// <summary>最終Waveに登場するボスのデータ。</summary>
    [SerializeField]
    private EnemyData bossEnemyData;

    /// <summary>ゲームクリアの通知とゲームオーバーの購読に使用する。</summary>
    private GameController gameController;

    /// <summary>
    /// 全5WaveのModelを生成し、GameControllerの参照を取得する。
    /// </summary>
    private void Awake()
    {
        model = new WaveModel(5);

        gameController = FindFirstObjectByType<GameController>();
    }

    /// <summary>
    /// 有効化されたときにゲームオーバーイベントを購読する。
    /// </summary>
    private void OnEnable()
    {
        if (gameController != null)
        {
            gameController.OnGameOver += HandleGameOver;
        }
    }

    /// <summary>
    /// 無効化されたときにイベントの購読を解除する（解除漏れによる参照残りを防ぐ）。
    /// </summary>
    private void OnDisable()
    {
        if (gameController != null)
        {
            gameController.OnGameOver -= HandleGameOver;
        }
    }

    /// <summary>
    /// ゲームオーバー時に呼ばれる処理。
    /// </summary>
    private void HandleGameOver()
    {
        // 敗北時はそれ以上Waveを進行させない
        enabled = false;
    }

    /// <summary>
    /// ゲーム開始時（TitleControllerで有効化されたとき）に最初のWaveを開始する。
    /// </summary>
    private void Start()
    {
        StartNextWave();
    }

    /// <summary>
    /// 現在のWaveの敵がすべていなくなったら、次のWaveへ進める。
    /// </summary>
    private void Update()
    {
        if (enemySpawner.IsWaveFinished())
        {
            StartNextWave();
        }
    }

    /// <summary>
    /// 次のWaveを開始する。全Waveを終えている場合はゲームクリアにする。
    /// </summary>
    private void StartNextWave()
    {
        // 最終Waveまで終わっていればクリア
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

        // Waveごとの敵構成。Waveが進むほど敵の数と種類を増やして難易度を上げる
        if (model.CurrentWave == 1)
        {
            // Wave1: 通常の敵のみ
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
            // Wave2: 高速な敵が登場
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
            // Wave3: 高HPのタンクが登場
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
            // Wave4: 全体的に数を増やす
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
            // Wave5（最終）: ボスが登場
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

    /// <summary>
    /// 現在のWave番号をHUDに反映する。
    /// </summary>
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
