using UnityEngine;

public class GameController : MonoBehaviour
{
    private GameModel model;

    [SerializeField]
    private GameView view;

    [SerializeField]
    private GameUI gameUI;

    private void Awake()
    {
        model = new GameModel(20, 200);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void DamageLife(int damage)
    {
        model.DecreaseLife(damage);

        Debug.Log("Life: " + model.Life);

        UpdateUI();

        if (model.Life <= 0)
        {
            GameOver();
        }
    }

    public bool TryBuyTower(int cost)
    {
        bool success = model.TrySpendMoney(cost);

        if (success)
        {
            Debug.Log("Tower purchased! Money: " + model.Money);
            UpdateUI();
        }
        else
        {
            Debug.Log("Not enough money!");
        }

        return success;
    }

    public void AddMoney(int amount)
    {
        model.AddMoney(amount);

        Debug.Log("Money: " + model.Money);

        UpdateUI();
    }

    private void UpdateUI()
    {
        if (gameUI == null)
        {
            return;
        }

        gameUI.UpdateLife(model.Life);
        gameUI.UpdateMoney(model.Money);
    }

    private void GameOver()
    {
        Debug.Log("Game Over!");
    }
}