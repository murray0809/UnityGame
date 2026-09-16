using UnityEngine;

/// <summary>
/// タワーが発射した弾の挙動を管理するController。
/// 対象の敵を追尾して移動し、命中時にダメージを与える。
/// 魔法弾の場合は着弾地点の周囲の敵すべてにダメージを与える範囲攻撃になる。
/// </summary>
public class ProjectileController : MonoBehaviour
{
    /// <summary>移動速度とダメージを保持するModel。</summary>
    private ProjectileModel model;

    /// <summary>追尾する敵。</summary>
    private EnemyController target;

    /// <summary>範囲攻撃を行う魔法弾かどうか。</summary>
    private bool isMagic;

    /// <summary>魔法弾の爆発範囲の半径。</summary>
    [SerializeField]
    private float magicAttackRange = 1.5f;

    /// <summary>魔法弾の着弾時に生成する爆発エフェクトのPrefab。</summary>
    [SerializeField]
    private GameObject magicExplosionPrefab;

    /// <summary>弾の画像を描画するSpriteRenderer。</summary>
    [SerializeField]
    private SpriteRenderer spriteRenderer;

    /// <summary>通常弾のスプライト。</summary>
    [SerializeField]
    private Sprite normalSprite;

    /// <summary>魔法弾のスプライト。</summary>
    [SerializeField]
    private Sprite magicSprite;

    /// <summary>
    /// Inspectorで未設定の場合はSpriteRendererを自動取得する。
    /// </summary>
    private void Awake()
    {
        if (spriteRenderer == null)
        {
            spriteRenderer = GetComponent<SpriteRenderer>();
        }
    }

    /// <summary>
    /// 発射直後にタワーから呼ばれ、追尾対象・ダメージ・弾の種類を設定する。
    /// </summary>
    /// <param name="target">追尾する敵</param>
    /// <param name="damage">命中時のダメージ</param>
    /// <param name="isMagic">魔法弾（範囲攻撃）かどうか</param>
    public void Initialize(
        EnemyController target,
        int damage,
        bool isMagic)
    {
        this.target = target;
        this.isMagic = isMagic;

        // 弾速は全タワー共通で8
        model = new ProjectileModel(
            8f,
            damage
        );

        ApplySprite();
    }

    /// <summary>
    /// 弾の種類に応じてスプライトを切り替える。
    /// 1つのPrefabで通常弾と魔法弾の両方を表現するために使用する。
    /// </summary>
    private void ApplySprite()
    {
        if (spriteRenderer == null)
        {
            return;
        }

        Sprite sprite = isMagic ? magicSprite : normalSprite;

        if (sprite != null)
        {
            spriteRenderer.sprite = sprite;
        }
    }

    /// <summary>
    /// 毎フレーム、対象の敵に向かって移動し、十分近づいたら命中処理を行う。
    /// </summary>
    private void Update()
    {
        // 飛んでいる間に対象が撃破・ゴールして消えた場合は弾も消す
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        Vector3 direction =
            target.transform.position - transform.position;

        // 毎フレーム現在位置から方向を計算し直すことで、動く敵を追尾する
        transform.position +=
            direction.normalized *
            model.MoveSpeed *
            Time.deltaTime;

        // 敵とほぼ重なっているときは角度が不安定になるため回転させない
        if (direction.sqrMagnitude > 0.0001f)
        {
            // 進行方向の角度（度数法）を求める
            float angle =
                Mathf.Atan2(direction.y, direction.x) *
                Mathf.Rad2Deg;

            // スプライトは「上向き」が正面なので-90度補正する
            transform.rotation =
                Quaternion.Euler(0f, 0f, angle - 90f);
        }

        // 十分近づいたら命中とみなす
        if (Vector3.Distance(
                transform.position,
                target.transform.position) < 0.1f)
        {
            HitTarget();
        }
    }

    /// <summary>
    /// 命中時の処理。通常弾は対象のみ、魔法弾は範囲内の敵すべてにダメージを与える。
    /// </summary>
    private void HitTarget()
    {
        if (target == null)
        {
            Destroy(gameObject);
            return;
        }

        if (isMagic)
        {
            // 着弾地点に爆発エフェクトを生成し、見た目の大きさを攻撃範囲に合わせる
            if (magicExplosionPrefab != null)
            {
                GameObject effect =
                    Instantiate(
                        magicExplosionPrefab,
                        target.transform.position,
                        Quaternion.identity
                    );

                MagicExplosionEffect explosion =
                    effect.GetComponent<MagicExplosionEffect>();

                if (explosion != null)
                {
                    explosion.Initialize(magicAttackRange);
                }
            }

            EnemyController[] enemies =
                FindObjectsByType<EnemyController>(
                    FindObjectsSortMode.None
                );

            // 着弾地点から爆発範囲内にいる敵すべてにダメージを与える
            foreach (EnemyController enemy in enemies)
            {
                if (enemy == null)
                {
                    continue;
                }

                float distance =
                    Vector3.Distance(
                        target.transform.position,
                        enemy.transform.position
                    );

                if (distance <= magicAttackRange)
                {
                    enemy.TakeDamage(model.Damage);
                }
            }
        }
        else
        {
            // 通常弾は対象の敵1体にのみダメージを与える
            target.TakeDamage(model.Damage);
        }

        Destroy(gameObject);
    }
}
