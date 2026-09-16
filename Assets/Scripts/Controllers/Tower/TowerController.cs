using UnityEngine;

/// <summary>
/// 設置済みタワー1基の挙動を管理するController。
/// 一定間隔で攻撃範囲内の敵を探して弾を発射し、強化の処理も担当する。
/// </summary>
public class TowerController : MonoBehaviour
{
    /// <summary>攻撃力や攻撃範囲、強化レベルを保持するModel。</summary>
    private TowerModel model;

    /// <summary>タワーの見た目を担当するView。</summary>
    private TowerView view;

    /// <summary>前回の攻撃からの経過時間。</summary>
    private float attackTimer;

    /// <summary>タワーの初期ステータスを定義したデータ。</summary>
    [SerializeField]
    private TowerData towerData;

    /// <summary>発射する弾のPrefab。</summary>
    [SerializeField]
    private GameObject projectilePrefab;

    /// <summary>範囲攻撃を行う魔法タワーかどうか。</summary>
    [SerializeField]
    private bool isMagicTower;

    /// <summary>敵の方向へ向きを反転させるためのSpriteRenderer。</summary>
    private SpriteRenderer spriteRenderer;

    /// <summary>ゲーム終了判定と強化時の支払いに使用する。</summary>
    private GameController gameController;

    /// <summary>
    /// TowerDataからModelを生成し、各コンポーネントの参照を取得する。
    /// </summary>
    private void Awake()
    {
        model = new TowerModel(
            towerData.cost,
            towerData.attackPower,
            towerData.attackInterval,
            towerData.attackRange,
            towerData.upgradeAttackPower,
            towerData.upgradeAttackRange
        );

        view = GetComponent<TowerView>();

        spriteRenderer = GetComponent<SpriteRenderer>();

        gameController = FindFirstObjectByType<GameController>();
    }

    /// <summary>
    /// 攻撃間隔ごとに攻撃を行う。
    /// </summary>
    private void Update()
    {
        // ゲーム終了後は攻撃しない（Game Over後に無意味な演出が続くのを防ぐ）。
        if (gameController != null && gameController.IsGameEnded)
        {
            return;
        }

        attackTimer += Time.deltaTime;

        // 攻撃間隔に達するまで待つ
        if (attackTimer < model.AttackInterval)
        {
            return;
        }

        attackTimer = 0f;

        Attack();
    }

    /// <summary>
    /// 攻撃対象を選び、その敵に向けて弾を発射する。
    /// </summary>
    private void Attack()
    {
        EnemyController target = FindTarget();

        // 範囲内に敵がいなければ攻撃しない
        if (target == null)
        {
            return;
        }

        Vector3 direction =
            target.transform.position - transform.position;

        // 敵が左右どちらにいるかでスプライトの向きを反転する
        // （ほぼ真上・真下の場合は向きを変えずにちらつきを防ぐ）
        if (Mathf.Abs(direction.x) > 0.01f)
        {
            spriteRenderer.flipX = direction.x < 0f;
        }

        GameObject projectileObject = Instantiate(
            projectilePrefab,
            transform.position,
            Quaternion.identity
        );

        // 弾に攻撃対象・ダメージ・攻撃タイプを渡す
        ProjectileController projectile =
            projectileObject.GetComponent<ProjectileController>();

        if (projectile != null)
        {
            projectile.Initialize(
                target,
                model.AttackPower,
                isMagicTower
            );
        }
    }

    /// <summary>
    /// 所持金を支払ってタワーを強化する。TowerUpgradeUIの強化ボタンから呼ばれる。
    /// </summary>
    public void Upgrade()
    {
        if (gameController == null)
        {
            return;
        }

        // 所持金が足りなければ強化しない
        if (!gameController.TryBuyTower(model.UpgradeCost))
        {
            Debug.Log("Moneyが足りません。");
            return;
        }

        model.Upgrade();

        Debug.Log(
            "Tower Upgraded! " +
            "AttackPower: " + model.AttackPower +
            " / AttackRange: " + model.AttackRange
        );
    }

    /// <summary>
    /// 攻撃範囲内の敵から攻撃対象を選ぶ。
    /// 最もゴールに近い（ウェイポイントを多く進んでいる）敵を優先し、
    /// 同じ進行度の敵が複数いる場合はタワーに最も近い敵を選ぶ。
    /// </summary>
    /// <returns>攻撃対象の敵。範囲内にいなければnull</returns>
    private EnemyController FindTarget()
    {
        EnemyController[] enemies =
            FindObjectsByType<EnemyController>(
                FindObjectsSortMode.None
            );

        EnemyController target = null;
        int furthestWaypoint = -1;
        float closestDistance = float.MaxValue;

        foreach (EnemyController enemy in enemies)
        {
            if (enemy == null)
            {
                continue;
            }

            float distance =
                Vector3.Distance(
                    transform.position,
                    enemy.transform.position
                );

            // 攻撃範囲外の敵は対象外
            if (distance > model.AttackRange)
            {
                continue;
            }

            if (enemy.CurrentWaypointIndex > furthestWaypoint)
            {
                // よりゴールに近い敵が見つかったので対象を更新する
                furthestWaypoint = enemy.CurrentWaypointIndex;
                closestDistance = distance;
                target = enemy;
            }
            else if (
                enemy.CurrentWaypointIndex == furthestWaypoint &&
                distance < closestDistance)
            {
                // 進行度が同じならタワーに近い敵を優先する
                closestDistance = distance;
                target = enemy;
            }
        }

        return target;
    }

    /// <summary>
    /// Sceneビューでタワーを選択したときに、攻撃範囲を円で表示する（デバッグ用）。
    /// </summary>
    private void OnDrawGizmosSelected()
    {
        // 再生前はModelが生成されていないため描画しない
        if (model == null)
        {
            return;
        }

        Gizmos.DrawWireSphere(
            transform.position,
            model.AttackRange
        );
    }

    /// <summary>
    /// 強化UIでステータスを表示するためにModelを取得する。
    /// </summary>
    /// <returns>このタワーのModel</returns>
    public TowerModel GetModel()
    {
        return model;
    }
}
