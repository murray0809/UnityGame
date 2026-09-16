using UnityEngine;
using System.Collections.Generic;

/// <summary>
/// 敵の出現を管理するスポナー。
/// Waveの敵リストを受け取り、一定間隔で1体ずつ生成する。
/// 生存中の敵の数も数えており、Waveが終わったかどうかを判定する。
/// </summary>
public class EnemySpawner : MonoBehaviour
{
    /// <summary>生成する敵のPrefab。見た目やステータスはEnemyDataで切り替える。</summary>
    [SerializeField]
    private GameObject enemyPrefab;

    /// <summary>敵を生成する位置。</summary>
    [SerializeField]
    private Transform spawnPoint;

    /// <summary>敵を生成する間隔（秒）。</summary>
    [SerializeField]
    private float spawnInterval = 2f;

    /// <summary>このWaveでまだ生成していない敵の数。</summary>
    private int remainingEnemies;

    /// <summary>生成済みで、まだ撃破もゴールもしていない敵の数。</summary>
    private int activeEnemies;

    /// <summary>前回の生成からの経過時間。</summary>
    private float spawnTimer;

    /// <summary>現在、敵を生成中かどうか。</summary>
    private bool isSpawning;

    /// <summary>現在生成中の敵データ。</summary>
    private EnemyData currentEnemyData;

    /// <summary>これから生成する敵の順番待ちキュー。</summary>
    private Queue<EnemyData> enemyQueue = new Queue<EnemyData>();

    /// <summary>ゲームオーバーイベントの購読に使用する。</summary>
    private GameController gameController;

    /// <summary>
    /// GameControllerの参照を取得する。
    /// </summary>
    private void Awake()
    {
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
    /// 無効化されたときにイベントの購読を解除する。
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
        // 敗北後は新たな敵をスポーンさせない
        isSpawning = false;
        enemyQueue.Clear();
    }

    /// <summary>
    /// 生成中であれば、一定間隔ごとに敵を1体生成する。
    /// </summary>
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

    /// <summary>
    /// Waveを開始する。指定された敵の種類と数から出現リストを作り、生成を始める。
    /// </summary>
    /// <param name="enemies">このWaveで出現させる敵の種類と数（可変長）</param>
    public void StartWave(params EnemyWaveData[] enemies)
    {
        enemyQueue.Clear();

        // 種類ごとの体数を展開して、1体ずつのリストにする
        List<EnemyData> spawnList = new List<EnemyData>();

        foreach (EnemyWaveData enemy in enemies)
        {
            for (int i = 0; i < enemy.count; i++)
            {
                spawnList.Add(enemy.enemyData);
            }
        }

        // 出現順をシャッフルして、種類ごとに固まらないようにする（Fisher-Yatesシャッフル）
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

        // タイマーを間隔分進めておき、Wave開始直後に1体目を出す
        spawnTimer = spawnInterval;
    }

    /// <summary>
    /// キューの先頭の敵を1体生成する。
    /// </summary>
    private void SpawnEnemy()
    {
        // 生成する敵がもういなければ生成を終了する
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

        // 共通Prefabに敵の種類ごとのデータを渡してステータスと見た目を決める
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

    /// <summary>
    /// 敵が撃破またはゴールしてフィールドからいなくなったときに呼び出す。
    /// </summary>
    public void OnEnemyDefeated()
    {
        activeEnemies--;

        // 多重呼び出しなどでマイナスにならないよう下限を0にする
        if (activeEnemies < 0)
        {
            activeEnemies = 0;
        }
    }

    /// <summary>
    /// 現在のWaveが終了したかどうかを判定する。
    /// </summary>
    /// <returns>生成がすべて終わり、フィールドに敵がいなければtrue</returns>
    public bool IsWaveFinished()
    {
        return !isSpawning && activeEnemies <= 0;
    }
}
