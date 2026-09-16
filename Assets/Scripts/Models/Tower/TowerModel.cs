/// <summary>
/// 設置済みタワー1基分のステータスと強化状態を管理するModel。
/// TowerDataの初期値をもとに生成され、強化によって値が変化する。
/// </summary>
public class TowerModel
{
    /// <summary>設置コスト。</summary>
    public int Cost { get; private set; }

    /// <summary>現在の攻撃力。</summary>
    public int AttackPower { get; private set; }

    /// <summary>攻撃間隔（秒）。</summary>
    public float AttackInterval { get; private set; }

    /// <summary>現在の攻撃範囲の半径。</summary>
    public float AttackRange { get; private set; }

    /// <summary>次の強化に必要な所持金。</summary>
    public int UpgradeCost { get; private set; }

    /// <summary>現在の強化レベル（1〜5）。</summary>
    public int Level { get; private set; }

    /// <summary>強化1回あたりの攻撃力上昇量。</summary>
    private int upgradeAttackPower;

    /// <summary>強化1回あたりの攻撃範囲上昇量。</summary>
    private float upgradeAttackRange;

    /// <summary>
    /// タワーの初期ステータスと強化時の上昇量を指定してModelを生成する。
    /// </summary>
    /// <param name="cost">設置コスト</param>
    /// <param name="attackPower">初期攻撃力</param>
    /// <param name="attackInterval">攻撃間隔（秒）</param>
    /// <param name="attackRange">初期攻撃範囲</param>
    /// <param name="upgradeAttackPower">強化1回あたりの攻撃力上昇量</param>
    /// <param name="upgradeAttackRange">強化1回あたりの攻撃範囲上昇量</param>
    public TowerModel(
        int cost,
        int attackPower,
        float attackInterval,
        float attackRange,
        int upgradeAttackPower,
        float upgradeAttackRange)
    {
        Cost = cost;
        AttackPower = attackPower;
        AttackInterval = attackInterval;
        AttackRange = attackRange;

        // 初回の強化コストとレベルの初期値
        UpgradeCost = 70;
        Level = 1;

        this.upgradeAttackPower = upgradeAttackPower;
        this.upgradeAttackRange = upgradeAttackRange;
    }

    /// <summary>
    /// タワーを1段階強化する。最大レベル（5）の場合は何もしない。
    /// 所持金の支払いは呼び出し側（TowerController）で行う。
    /// </summary>
    public void Upgrade()
    {
        // 最大レベルに達していれば強化しない
        if (Level >= 5)
        {
            return;
        }

        AttackPower += upgradeAttackPower;
        AttackRange += upgradeAttackRange;

        // レベルが上がるごとに次の強化コストを増やす
        UpgradeCost += 45;
        Level++;
    }
}
