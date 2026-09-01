public class GameModel
{
    public int Life { get; private set; }
    public int Money { get; private set; }
    public int CurrentWave { get; private set; }

    public GameModel(int life, int money)
    {
        Life = life;
        Money = money;
        CurrentWave = 0;
    }

    public void DecreaseLife(int damage)
    {
        Life -= damage;
    }

    public void AddMoney(int amount)
    {
        Money += amount;
    }

    public bool TrySpendMoney(int amount)
    {
        if (Money < amount)
        {
            return false;
        }

        Money -= amount;
        return true;
    }

    public void SetWave(int wave)
    {
        CurrentWave = wave;
    }
}