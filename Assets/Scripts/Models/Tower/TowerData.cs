using UnityEngine;

/// <summary>
/// タワーの種類ごとの初期パラメータを定義するScriptableObject。
/// 通常タワーと魔法タワーをアセットとして作り分け、Inspector上でバランス調整できるようにしている。
/// </summary>
[CreateAssetMenu(
    fileName = "TowerData",
    menuName = "Tiny Defense/Tower Data"
)]
public class TowerData : ScriptableObject
{
    /// <summary>設置に必要な所持金。</summary>
    [Header("Tower Status")]
    public int cost = 50;

    /// <summary>1発あたりの攻撃力。</summary>
    public int attackPower = 25;

    /// <summary>攻撃間隔（秒）。</summary>
    public float attackInterval = 0.6f;

    /// <summary>攻撃範囲の半径（ワールド単位）。</summary>
    public float attackRange = 3f;

    /// <summary>強化1回あたりの攻撃力の上昇量。</summary>
    [Header("Upgrade")]
    public int upgradeAttackPower = 10;

    /// <summary>強化1回あたりの攻撃範囲の上昇量。</summary>
    public float upgradeAttackRange = 0.5f;
}
