/// <summary>
/// 敵1体分のステータス（HP・移動速度・報酬・ゴール時ダメージ）を保持するModel。
/// ダメージ計算や死亡判定などのロジックのみを持ち、見た目や移動処理はControllerに任せる。
/// </summary>
public class EnemyModel
{
    /// <summary>現在のHP。0未満にはならない。</summary>
    public int HP { get; private set; }

    /// <summary>1秒あたりの移動距離（ワールド単位）。</summary>
    public float Speed { get; private set; }

    /// <summary>撃破時にプレイヤーが得られる所持金。</summary>
    public int Reward { get; private set; }

    /// <summary>ゴール到達時にプレイヤーのライフへ与えるダメージ。</summary>
    public int GoalDamage { get; private set; }

    /// <summary>
    /// 敵のステータスを指定してModelを生成する。
    /// </summary>
    /// <param name="hp">最大HP</param>
    /// <param name="speed">移動速度</param>
    /// <param name="reward">撃破報酬</param>
    /// <param name="goalDamage">ゴール到達時のダメージ</param>
    public EnemyModel(int hp, float speed, int reward, int goalDamage)
    {
        HP = hp;
        Speed = speed;
        Reward = reward;
        GoalDamage = goalDamage;
    }

    /// <summary>
    /// ダメージを受けてHPを減らす。
    /// </summary>
    /// <param name="damage">受けるダメージ量</param>
    public void TakeDamage(int damage)
    {
        HP -= damage;

        // HPがマイナス表示にならないよう0で下限を止める
        if (HP < 0)
        {
            HP = 0;
        }
    }

    /// <summary>
    /// 敵が倒されているかどうかを判定する。
    /// </summary>
    /// <returns>HPが0以下ならtrue</returns>
    public bool IsDead()
    {
        return HP <= 0;
    }
}
