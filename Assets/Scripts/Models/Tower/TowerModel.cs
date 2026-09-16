public class TowerModel
{
    public int Cost { get; private set; }
    public int AttackPower { get; private set; }
    public float AttackInterval { get; private set; }
    public float AttackRange { get; private set; }
    public int UpgradeCost { get; private set; }
    public int Level { get; private set; }

    private int upgradeAttackPower;
    private float upgradeAttackRange;

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

        UpgradeCost = 70;
        Level = 1;

        this.upgradeAttackPower = upgradeAttackPower;
        this.upgradeAttackRange = upgradeAttackRange;
    }

    public void Upgrade()
    {
        if (Level >= 5)
        {
            return;
        }

        AttackPower += upgradeAttackPower;
        AttackRange += upgradeAttackRange;

        UpgradeCost += 45;
        Level++;
    }
}