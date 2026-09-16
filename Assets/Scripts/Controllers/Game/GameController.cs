using System;   // ← 追加
using UnityEngine;
using UnityEngine.SceneManagement;   // ← 追加

public class GameController : MonoBehaviour
{
    private GameModel model;

    [SerializeField]
    private GameView view;

    [SerializeField]
    private GameUI gameUI;

    public bool IsGameEnded { get; private set; }   // ← 追加

    // ゲーム終了（敗北）を通知するイベント
    public event Action OnGameOver;   // ← 追加

    // ゲーム終了（クリア）を通知するイベント
    public event Action OnGameClear;   // ← 追加

    private void Awake()
    {
        model = new GameModel(15, 150);
    }

    private void Start()
    {
        UpdateUI();
    }

    public void DamageLife(int damage)
    {
        if (IsGameEnded) return;   // ← 追加

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
        if (IsGameEnded) return false;   // ← 追加

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
        if (IsGameEnded) return;   // ← 追加

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

    // After
    private void GameOver()
    {
        IsGameEnded = true;
        Debug.Log("Game Over!");

        if (gameUI != null)
        {
            gameUI.ShowResult("GAME OVER");
        }

        OnGameOver?.Invoke();
    }

    public void GameClear()
    {
        if (IsGameEnded) return;

        IsGameEnded = true;
        Debug.Log("Game Clear!");

        if (gameUI != null)
        {
            gameUI.ShowResult("GAME CLEAR");
        }

        OnGameClear?.Invoke();
    }

    // RetryButtonのOnClickから呼び出す
    public void RestartGame()
    {
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.name);
    }
}