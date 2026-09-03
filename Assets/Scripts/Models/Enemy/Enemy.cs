using UnityEngine;

[CreateAssetMenu(
    fileName = "EnemyData",
    menuName = "Tiny Defense/Enemy Data"
)]
public class EnemyData : ScriptableObject
{
    [Header("Enemy Status")]
    public int maxHp = 100;
    public float moveSpeed = 2f;
    public int reward = 10;
    public int goalDamage = 1;
}