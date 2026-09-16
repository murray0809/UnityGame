using UnityEngine;

/// <summary>
/// 敵の種類ごとのパラメータを定義するScriptableObject。
/// 通常・高速・タンク・ボスなどの敵をアセットとして作り分け、
/// コードを変更せずにInspector上でバランス調整できるようにしている。
/// </summary>
[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Tiny Defense/Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    /// <summary>最大HP。</summary>
    [Header("Enemy Status")]
    public int maxHp = 100;

    /// <summary>移動速度（ワールド単位/秒）。</summary>
    public float moveSpeed = 2f;

    /// <summary>撃破時に得られる所持金。</summary>
    public int reward = 10;

    /// <summary>ゴール到達時にプレイヤーのライフへ与えるダメージ。</summary>
    public int goalDamage = 1;

    /// <summary>この種類の敵に表示するスプライト。</summary>
    [Header("Appearance")]
    public Sprite sprite;
}
