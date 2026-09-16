/// <summary>
/// タワーが発射する弾のパラメータ（移動速度・ダメージ）を保持するModel。
/// </summary>
public class ProjectileModel
{
    /// <summary>弾の移動速度（ワールド単位/秒）。</summary>
    public float MoveSpeed { get; private set; }

    /// <summary>命中時に敵へ与えるダメージ。</summary>
    public int Damage { get; private set; }

    /// <summary>
    /// 移動速度とダメージを指定してModelを生成する。
    /// </summary>
    /// <param name="moveSpeed">移動速度</param>
    /// <param name="damage">ダメージ量</param>
    public ProjectileModel(float moveSpeed, int damage)
    {
        MoveSpeed = moveSpeed;
        Damage = damage;
    }
}
