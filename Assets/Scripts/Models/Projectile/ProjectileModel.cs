public class ProjectileModel
{
    public float MoveSpeed { get; private set; }
    public int Damage { get; private set; }

    public ProjectileModel(float moveSpeed, int damage)
    {
        MoveSpeed = moveSpeed;
        Damage = damage;
    }
}