using UnityEngine;

[CreateAssetMenu(
    fileName = "TowerData",
    menuName = "Tiny Defense/Tower Data"
)]
public class TowerData : ScriptableObject
{
    [Header("Tower Status")]
    public int cost = 50;
    public int attackPower = 25;
    public float attackInterval = 0.6f;
    public float attackRange = 3f;

    [Header("Upgrade")]
    public int upgradeAttackPower = 10;
    public float upgradeAttackRange = 0.5f;
}