/// <summary>
/// ゲーム全体の進行状態（ライフ・所持金・現在のWave）を保持するModel。
/// Unityに依存しない純粋なC#クラスとして実装し、ロジックとUI/シーンを分離している。
/// </summary>
public class GameModel
{
    /// <summary>プレイヤーの残りライフ。0以下になるとゲームオーバー。</summary>
    public int Life { get; private set; }

    /// <summary>タワーの購入・強化に使う所持金。</summary>
    public int Money { get; private set; }

    /// <summary>現在進行中のWave番号。</summary>
    public int CurrentWave { get; private set; }

    /// <summary>
    /// 初期ライフと初期所持金を指定してModelを生成する。
    /// </summary>
    /// <param name="life">初期ライフ</param>
    /// <param name="money">初期所持金</param>
    public GameModel(int life, int money)
    {
        Life = life;
        Money = money;
        CurrentWave = 0;
    }

    /// <summary>
    /// 敵がゴールに到達した際などにライフを減らす。
    /// </summary>
    /// <param name="damage">減少させるライフ量</param>
    public void DecreaseLife(int damage)
    {
        Life -= damage;
    }

    /// <summary>
    /// 敵撃破の報酬などで所持金を増やす。
    /// </summary>
    /// <param name="amount">加算する金額</param>
    public void AddMoney(int amount)
    {
        Money += amount;
    }

    /// <summary>
    /// 所持金が足りていれば支払いを行う。
    /// </summary>
    /// <param name="amount">支払う金額</param>
    /// <returns>支払いに成功した場合はtrue、所持金不足の場合はfalse</returns>
    public bool TrySpendMoney(int amount)
    {
        // 所持金不足の場合は何も減らさずに失敗を返す
        if (Money < amount)
        {
            return false;
        }

        Money -= amount;
        return true;
    }

    /// <summary>
    /// 現在のWave番号を設定する。
    /// </summary>
    /// <param name="wave">設定するWave番号</param>
    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }
}
