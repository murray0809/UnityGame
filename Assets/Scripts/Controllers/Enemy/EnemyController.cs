using UnityEngine;

/// <summary>
/// 敵1体の挙動を管理するController。
/// ウェイポイントに沿った移動、被ダメージ、撃破・ゴール到達時の処理を担当する。
/// </summary>
public class EnemyController : MonoBehaviour
{
    /// <summary>HPや移動速度などのステータスを保持するModel。</summary>
    private EnemyModel model;

    /// <summary>スプライトなどの見た目を担当するView。</summary>
    private EnemyView view;

    /// <summary>この敵のステータスと見た目の元になるデータ。スポナーから設定される。</summary>
    [SerializeField]
    private EnemyData enemyData;

    /// <summary>移動経路を提供するController。</summary>
    private WaypointController waypointController;

    /// <summary>次に向かうウェイポイントの番号。</summary>
    private int currentWaypointIndex = 0;

    /// <summary>
    /// 次に向かうウェイポイントの番号。
    /// 値が大きいほどゴールに近いため、タワーの攻撃対象の選択に使われる。
    /// </summary>
    public int CurrentWaypointIndex => currentWaypointIndex;

    /// <summary>撃破またはゴール処理が済んでいるかどうか。</summary>
    private bool isDefeated = false;

    /// <summary>
    /// コンポーネントと経路の参照を取得する。
    /// </summary>
    private void Awake()
    {
        view = GetComponent<EnemyView>();

        waypointController = FindFirstObjectByType<WaypointController>();
    }

    /// <summary>
    /// EnemyDataからModelを生成し、見た目を反映する。
    /// Initializeで設定されたデータを使うため、AwakeではなくStartで行う。
    /// </summary>
    private void Start()
    {
        if (enemyData == null)
        {
            Debug.LogError("EnemyData is not assigned!");
            return;
        }

        model = new EnemyModel(
            enemyData.maxHp,
            enemyData.moveSpeed,
            enemyData.reward,
            enemyData.goalDamage
        );

        view.SetSprite(enemyData.sprite);
    }

    /// <summary>
    /// 毎フレーム、次のウェイポイントに向かって移動する。
    /// </summary>
    private void Update()
    {
        MoveToWaypoint();
    }

    /// <summary>
    /// スポナーから生成直後に呼ばれ、この敵の種類を設定する。
    /// </summary>
    /// <param name="data">敵の種類ごとのデータ</param>
    public void Initialize(EnemyData data)
    {
        enemyData = data;
    }

    /// <summary>
    /// 現在のウェイポイントへ移動し、到着したら次の地点へ切り替える。
    /// 最後の地点に到着した場合はゴール処理を行う。
    /// </summary>
    private void MoveToWaypoint()
    {
        Transform targetWaypoint =
            waypointController.GetWaypoint(currentWaypointIndex);

        if (targetWaypoint == null)
        {
            return;
        }

        Vector3 direction =
            targetWaypoint.position - transform.position;

        // 正規化した方向に速度とdeltaTimeを掛け、フレームレートに依存しない移動にする
        transform.position +=
            direction.normalized *
            model.Speed *
            Time.deltaTime;

        // 十分近づいたら到着とみなし、次のウェイポイントへ
        if (Vector3.Distance(
                transform.position,
                targetWaypoint.position) < 0.1f)
        {
            currentWaypointIndex++;

            if (currentWaypointIndex >= waypointController.WaypointCount)
            {
                ReachGoal();
            }
        }
    }

    /// <summary>
    /// 弾が命中したときにダメージを受ける。HPが0になったら撃破処理を行う。
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void TakeDamage(int damage)
    {
        // 既に撃破処理済みなら何もしない（同フレーム内の多重ヒット対策）
        if (isDefeated)
        {
            return;
        }

        model.TakeDamage(damage);

        Debug.Log("Enemy HP: " + model.HP);

        if (model.IsDead())
        {
            Die();
        }
    }

    /// <summary>
    /// 撃破時の処理。スポナーに通知し、報酬を加算してから自身を破棄する。
    /// </summary>
    private void Die()
    {
        // Destroyは即時ではないため、フラグで二重処理を防ぐ
        if (isDefeated)
        {
            return;
        }

        isDefeated = true;

        // Wave終了判定のため、生存数を減らしてもらう
        EnemySpawner enemySpawner =
            FindFirstObjectByType<EnemySpawner>();

        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDefeated();
        }

        // 撃破報酬を所持金に加算する
        GameController gameController =
            FindFirstObjectByType<GameController>();

        if (gameController != null)
        {
            gameController.AddMoney(model.Reward);
        }

        Debug.Log("Enemy Defeated!");

        Destroy(gameObject);
    }

    /// <summary>
    /// ゴール到達時の処理。プレイヤーのライフを減らし、スポナーに通知してから自身を破棄する。
    /// </summary>
    private void ReachGoal()
    {
        // 撃破と同じフレームでゴールした場合に二重処理しない
        if (isDefeated)
        {
            return;
        }

        isDefeated = true;

        GameController gameController =
            FindFirstObjectByType<GameController>();

        if (gameController != null)
        {
            gameController.DamageLife(model.GoalDamage);
        }

        // ゴールした敵もフィールドからいなくなるので生存数を減らす
        EnemySpawner enemySpawner =
            FindFirstObjectByType<EnemySpawner>();

        if (enemySpawner != null)
        {
            enemySpawner.OnEnemyDefeated();
        }

        Debug.Log("Goal!");

        Destroy(gameObject);
    }
}
