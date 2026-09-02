public class EnemyModel
{
    public int HP { get; private set; }
    public float Speed { get; private set; }
    public int Reward { get; private set; }
    public int GoalDamage { get; private set; }

    public EnemyModel(int hp, float speed, int reward, int goalDamage)
    {
        HP = hp;
        Speed = speed;
        Reward = reward;
        GoalDamage = goalDamage;
    }

    public void TakeDamage(int damage)
    {
        HP -= damage;

        if (HP < 0)
        {
            HP = 0;
        }
    }

    public bool IsDead()
    {
        return HP <= 0;
    }
}