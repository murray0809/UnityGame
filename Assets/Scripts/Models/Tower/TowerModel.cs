public class TowerModel
{
    public int Cost { get; private set; }
    public int AttackPower { get; private set; }
    public float AttackInterval { get; private set; }
    public float AttackRange { get; private set; }

    public TowerModel(
        int cost,
        int attackPower,
        float attackInterval,
        float attackRange)
    {
        Cost = cost;
        AttackPower = attackPower;
        AttackInterval = attackInterval;
        AttackRange = attackRange;
    }
}